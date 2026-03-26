using System.Text.Json.Serialization;

using Lexicora.Api.Features.Users.CreateUser;
using Lexicora.Api.Features.Users.GetAllUsers;
using Lexicora.Api.Features.Users.GetUserByEmail;
using Lexicora.Api.Features.Users.GetUserById;
using Lexicora.Api.Features.Users.GetUserByUsername;
using Lexicora.Api.Features.Users.UpdateUser;

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
[JsonSerializable(typeof(GetUserByIdResponse))]
[JsonSerializable(typeof(GetUserByEmailResponse))]
[JsonSerializable(typeof(GetUserByUsernameResponse))]
[JsonSerializable(typeof(UpdateUserRequest))]
[JsonSerializable(typeof(UpdateUserResponse))]
[JsonSerializable(typeof(GetAllUsersResponseCollection))]
internal partial class UsersJsonContext : JsonSerializerContext;
