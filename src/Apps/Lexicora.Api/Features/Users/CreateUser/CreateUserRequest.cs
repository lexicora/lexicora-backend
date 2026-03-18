using System.ComponentModel.DataAnnotations;

namespace Lexicora.Api.Features.Users.CreateUser;

public record CreateUserRequest(
    [property: Required]
    [property: EmailAddress]
    [property: MaxLength(255)]
    string Email,

    [property: Required]
    [property: RegularExpression("^[a-zA-Z0-9_.-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, dots, and hyphens.")]
    [property: StringLength(30, MinimumLength = 3)]
    string Username,

    [property: Required]
    [property: MaxLength(150)]
    string FirstName,

    [property: Required]
    [property: MaxLength(150)]
    string LastName,

    [property: MaxLength(200)] // Maybe increase to 255 (db limit).
    string? DisplayName = null);
