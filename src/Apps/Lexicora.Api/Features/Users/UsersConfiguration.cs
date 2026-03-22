using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Users.CreateUser;
using Lexicora.Api.Features.Users.GetUserByEmail;
using Lexicora.Api.Features.Users.GetUserById;
using Lexicora.Api.Features.Users.GetUserByUsername;

using Scalar.AspNetCore;

namespace Lexicora.Api.Features.Users;

public class UsersConfiguration : IEndpointConfiguration
{
    public static void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var usersGroup = builder.MapGroup("/users")
            .WithTags("Users")
            .WithDescription("Endpoints for managing users.")
            .WithBadge("Users", color: "green");

        // READ (GET)
        GetUserByIdEndpoint.Map(usersGroup);
        GetUserByEmailEndpoint.Map(usersGroup);
        GetUserByUsernameEndpoint.Map(usersGroup);

        // CREATE (POST)
        CreateUserEndpoint.Map(usersGroup);

        // UPDATE (PUT/PATCH) - To be implemented

        // DELETE - To be implemented
    }
}
