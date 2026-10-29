# Lexicora Backend

The backend API for [Lexicora](https://lexicora.github.io/), a browser extension that captures web pages and turns them into structured, searchable knowledge, organized into topics.

Built with **.NET 10**, **ASP.NET Core Minimal APIs**, **PostgreSQL**, **EF Core** and **Dapper.AOT**.

> **This project is archived.** It was built while Lexicora was planned as a cloud service. The extension ended up local-first, so this backend was never deployed. It's published as a reference for the patterns and techniques below. Everything in it works end to end: users, topics and entries (captured pages with their [BlockNote](https://www.blocknotejs.org/) documents). See [scope](#scope) for what was deliberately left out.

**Try it without installing anything but the .NET SDK:**

```bash
dotnet run --project src/Apps/Lexicora.Api --launch-profile demo
```

This starts a throwaway PostgreSQL server alongside the API, seeds sample data and opens the interactive API reference. [More options below](#getting-started).

## Highlights

The interesting parts of this codebase, and where to find them.

### Reflection-free endpoint registration with static abstract interface members

Every endpoint is a small, self-contained class implementing [`IEndpoint`](src/Apps/Lexicora.Api/Contracts/Endpoints/IEndpoint.cs), and every feature groups its endpoints in an [`IEndpointConfiguration`](src/Apps/Lexicora.Api/Contracts/Endpoints/IEndpointConfiguration.cs). Both contracts use C# **static abstract interface members**, so registration is a plain static call:

```csharp
public class GetTopicByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder group)
    {
        group.MapGet("/{id:ulid}", HandleAsync)
            .WithName("GetTopicById")
            .WithSummary("Get topic by id");
    }

    private static async Task<Results<Ok<GetTopicByIdResponse>, NotFound>> HandleAsync(
        string id, NpgsqlDataSource dataSource, CancellationToken cancellationToken) { /* ... */ }
}
```

Unlike the common "scan the assembly for `IEndpoint` implementations" approach, there is no reflection, no `Activator.CreateInstance` and no DI registration of endpoint instances. Everything is resolved at compile time. That keeps startup fast and works with trimming. Because every handler is a statically known method, the [Request Delegate Generator](https://learn.microsoft.com/aspnet/core/fundamentals/aot/rdg) (enabled in this project) generates the parameter binding and response writing for all of them at compile time. Development-only endpoints are mapped conditionally in the same place ([`TopicsConfiguration`](src/Apps/Lexicora.Api/Features/Topics/TopicsConfiguration.cs)).

### Vertical slices

Each use case lives in its own folder with its endpoint, request, response and mapping code ([`Features/`](src/Apps/Lexicora.Api/Features)). Changing a feature means touching one folder, and slices don't share DTOs, so each response is shaped for exactly one query.

### EF Core for writes, Dapper.AOT for reads

Commands use **EF Core** for change tracking, transactions and migrations. Queries use **Dapper.AOT**: hand-written SQL that projects straight into the response DTO, with no entity materialization and no mapping step. The [Dapper.AOT](https://aot.dapperlib.dev/) source generator intercepts the calls at compile time instead of emitting IL at runtime. (Multi-result reads via `QueryMultiple` aren't intercepted by Dapper.AOT and use Dapper's runtime mapping, which is why their DTOs use init properties instead of positional records.) Both share a single pooled `NpgsqlDataSource` ([`Program.cs`](src/Apps/Lexicora.Api/Program.cs)).

### AOT-friendly by design

The API layer avoids runtime reflection and code generation throughout:

- Reflection-based `System.Text.Json` is disabled solution-wide ([`Directory.Build.props`](Directory.Build.props)). All serialization goes through source-generated `JsonSerializerContext`s, one per feature ([`Infrastructure/Json/Contexts`](src/Apps/Lexicora.Api/Infrastructure/Json/Contexts)).
- Request validation uses the .NET 10 **source-generated validation** (`AddValidation()`), driven by data annotations on the request records.
- All regular expressions are `[GeneratedRegex]`.

The remaining blocker for a full Native AOT publish is EF Core, whose AOT support (precompiled queries) is still experimental. That's also why writes are the only place EF Core is used.

### ULIDs, end to end

Identifiers are [ULIDs](https://github.com/ulid/spec): generated in the application, stored in native 16-byte Postgres `uuid` columns, and exposed as lowercase, 26-character strings.

- They are **time-ordered**, so `ORDER BY id` sorts by creation time and inserts stay index-friendly.
- [`Ulid.TryParseStrict`](src/Domain/Lexicora.Domain/Common/UlidExtensions.cs) rejects Crockford aliases (`I`, `L`, `O`, `U`) and 128-bit overflow. It uses vectorized `SearchValues<char>` and doesn't allocate. It backs both the `{id:ulid}` [route constraint](src/Apps/Lexicora.Api/Infrastructure/RouteConstraints/UlidConstraint.cs) and the `[StrictUlid]` validation attribute.
- A custom [JSON converter](src/Apps/Lexicora.Api/Infrastructure/Json/Converters/LowercaseUlidJsonConverter.cs) formats and lowercases on the stack (`stackalloc` + `Ascii.ToLower`). Response DTOs keep the raw `Guid` that Dapper reads, and expose the ULID as a computed property, so no mapping layer is needed.

### BlockNote documents, stored as a tree, served without parsing

An entry's content is a [BlockNote](https://www.blocknotejs.org/) document: nested blocks with free-form `props` and `content`. The API accepts the editor's document as-is and returns it in the same shape, so the extension can save and load without any conversion.

- Each block is its own row (with `jsonb` props and content and a self-referencing parent), so blocks can be queried, searched or edited individually. Orders are spaced out (`1000`, `2000`, ...), so a block can be moved between two others without renumbering its siblings.
- Reading an entry takes one round trip for the entry and all of its blocks. The tree is rebuilt in a single O(n) pass ([`GetEntryByIdEndpoint`](src/Apps/Lexicora.Api/Features/Entries/GetEntryById/GetEntryByIdEndpoint.cs)).
- The `jsonb` columns are read as text and written into the response verbatim by a [`RawJsonConverter`](src/Apps/Lexicora.Api/Infrastructure/Json/Converters/RawJsonConverter.cs), so block content is never parsed or re-serialized on the way out.
- Saving a document replaces its blocks with a single bulk `DELETE` and the new inserts, in one transaction ([`UpdateEntryEndpoint`](src/Apps/Lexicora.Api/Features/Entries/UpdateEntry/UpdateEntryEndpoint.cs)).

### Human-readable, stable slugs

Topics and entries get slugs like `distributed-systems-nvddzkrxxbkv`. [`Utilities.CreateSlug`](src/Domain/Lexicora.Domain/Common/Utilities.cs) builds the slug in a single `stackalloc` buffer with one final string allocation. The suffix comes from the entity's ULID, so it stays unique, and renaming a topic keeps the suffix.

### Validation in depth

The same rules are enforced at every layer: route constraints (`ulid`, `slug`), source-generated request validation, and Postgres `CHECK` constraints for email, username, slug and tag count ([`Schema/`](src/Persistence/Lexicora.Persistence.Postgres/Schema)). Emails, usernames and slugs use `citext`, so uniqueness and lookups are case-insensitive in the database itself.

### Standard error responses

Every error is an [RFC 9457](https://www.rfc-editor.org/rfc/rfc9457) `application/problem+json` response. That covers validation failures, conflicts, unknown references and malformed JSON, which is mapped to a 400 instead of a 500 and isn't logged as a server error.

## Project structure

```
src/
├── Apps/Lexicora.Api                      # ASP.NET Core Minimal API (composition root)
│   ├── Contracts/Endpoints                # IEndpoint / IEndpointConfiguration
│   ├── Features/{Users,Topics,Entries}/<UseCase>  # Vertical slices
│   └── Infrastructure                     # JSON contexts and converters, route constraints, embedded database
├── Domain/Lexicora.Domain                 # Entities, BlockNote document model, ULID and slug utilities
├── Persistence/Lexicora.Persistence.Postgres  # DbContext, Fluent API schema, migrations
└── Infrastructure/Lexicora.Infrastructure     # Scaffolded for external services (auth, email, AI, CDN), left empty
tests/
└── Lexicora.UnitTests                     # xUnit v3 on Microsoft Testing Platform
```

## Getting started

**Prerequisites:** the [.NET 10 SDK](https://dotnet.microsoft.com/download). Docker or PostgreSQL is optional.

### Option 1: Demo mode, no database needed

```bash
dotnet run --project src/Apps/Lexicora.Api --launch-profile demo
```

The API downloads PostgreSQL once (about 50 MB, kept in its git-ignored `bin` folder, so nothing is written outside the repository), starts a private server on a free port, applies the migrations and seeds sample data. The database is deleted when the API stops. It's a real PostgreSQL rather than an in-memory substitute, because the read side uses Postgres-specific SQL (`citext`, arrays, `jsonb`), which has to behave exactly as in production. Any other profile can use it too, by setting `Database:Embedded=true`. ([`EmbeddedPostgres`](src/Apps/Lexicora.Api/Infrastructure/Database/EmbeddedPostgres.cs))

### Option 2: Your own PostgreSQL

```bash
# Start PostgreSQL (credentials match appsettings.Development.json)
docker compose up -d

# Run the API. In Development, pending migrations are applied on startup.
dotnet run --project src/Apps/Lexicora.Api
```

Either way, open the interactive API reference at **http://localhost:5177/scalar/v1**, or step through [`Lexicora.Api.http`](src/Apps/Lexicora.Api/Lexicora.Api.http) in Visual Studio or Rider.

To use a different database, override the connection string with user secrets or an environment variable:

```bash
export ConnectionStrings__Postgres="Host=...;Database=...;Username=...;Password=..."
```

### Tests

```bash
dotnet test
```

### Migrations

`dotnet-ef` is pinned as a local tool:

```bash
dotnet tool restore
cd src/Apps/Lexicora.Api
dotnet ef migrations add <Name> --project ../../Persistence/Lexicora.Persistence.Postgres
```

### Container image

The API uses the .NET SDK's built-in container support, so no Dockerfile is needed:

```bash
dotnet publish src/Apps/Lexicora.Api -t:PublishContainer
```

## API

| Method   | Route                          | Description                                         |
|----------|--------------------------------|-----------------------------------------------------|
| `POST`   | `/users`                       | Create a user                                       |
| `GET`    | `/users/{id}`                  | Get a user by ULID                                  |
| `GET`    | `/users/by-email/{email}`      | Get a user by email (case-insensitive)              |
| `GET`    | `/users/by-username/{username}`| Get a user by username (case-insensitive)           |
| `PUT`    | `/users/{id}`                  | Update a user                                       |
| `DELETE` | `/users/{id}`                  | Delete a user and, by cascade, their topics         |
| `POST`   | `/topics`                      | Create a topic for a user                           |
| `GET`    | `/topics/{id}`                 | Get a topic by ULID                                 |
| `GET`    | `/topics/by-slug/{slug}`       | Get a topic by slug                                 |
| `GET`    | `/topics/by-user/{userId}`     | Get all topics of a user                            |
| `PUT`    | `/topics/{id}`                 | Update a topic (regenerates the slug from the name) |
| `DELETE` | `/topics/{id}`                 | Delete a topic and, by cascade, its entries         |
| `POST`   | `/entries`                     | Save a page with its BlockNote document, into an existing or a new topic |
| `GET`    | `/entries/{id}`                | Get an entry, including its document                |
| `GET`    | `/entries/by-topic/{topicId}`  | Get all entries of a topic (without documents)      |
| `PUT`    | `/entries/{id}`                | Update an entry, optionally replacing its document  |
| `DELETE` | `/entries/{id}`                | Delete an entry                                     |
| `GET`    | `/users/all`, `/topics/all`, `/entries/all` | List everything, **Development only**  |

## Scope

This is a reference project, not a production service. It intentionally does not include:

- **Authentication and authorization.** User identity was meant to come from an external provider (Supabase), so every endpoint is open. Don't deploy it as is.
- Pagination, filtering and search on list endpoints.
- Typed validation of block content. Blocks are stored as BlockNote sends them. A polymorphic, source-generation friendly model of BlockNote's format exists in [`Domain/Entities/BlockNote`](src/Domain/Lexicora.Domain/Entities/BlockNote), but the API doesn't use it.

## License

[MIT](LICENSE.txt). Use anything here as a starting point for your own projects.
