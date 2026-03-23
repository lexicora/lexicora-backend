using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Users.CreateUser;
using Lexicora.Api.Features.Users.DeleteUser;
using Lexicora.Api.Features.Users.GetUserByEmail;
using Lexicora.Api.Features.Users.GetUserById;
using Lexicora.Api.Features.Users.GetUserByUsername;

namespace Lexicora.Api.Features.Users;

public class UsersConfiguration : IEndpointConfiguration
{
    public static void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var usersGroup = builder.MapGroup("/users")
            .WithTags("Users");

        // READ (GET)
        GetUserByIdEndpoint.Map(usersGroup);
        GetUserByEmailEndpoint.Map(usersGroup);
        GetUserByUsernameEndpoint.Map(usersGroup);

        // CREATE (POST)
        CreateUserEndpoint.Map(usersGroup);

        // UPDATE (PUT/PATCH) - To be implemented

        // DELETE
        DeleteUserEndpoint.Map(usersGroup);
    }
}
