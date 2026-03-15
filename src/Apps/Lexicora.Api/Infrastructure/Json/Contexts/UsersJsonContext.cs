using System.Text.Json.Serialization;

using Lexicora.Api.Features.Users.CreateUser;

namespace Lexicora.Api.Infrastructure.Json.Contexts;

#if DEBUG
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
#else
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = false)]
#endif
[JsonSerializable(typeof(CreateUserRequest))]
[JsonSerializable(typeof(CreateUserResponse))]
internal partial class UsersJsonContext : JsonSerializerContext;
