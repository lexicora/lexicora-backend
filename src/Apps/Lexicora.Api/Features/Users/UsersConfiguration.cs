using Lexicora.Api.Contracts.Endpoints;
using Lexicora.Api.Features.Users.CreateUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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

        CreateUserEndpoint.Map(usersGroup);
    }
}
