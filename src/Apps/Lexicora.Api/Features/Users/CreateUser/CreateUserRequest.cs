using System.ComponentModel.DataAnnotations;

namespace Lexicora.Api.Features.Users.CreateUser;

public record CreateUserRequest(
    [property: Required]
    [property: EmailAddress]
    [property: MinLength(6, ErrorMessage = "Email is too short.")]
    [property: MaxLength(254, ErrorMessage = "Email is too long.")]
    [property: RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", ErrorMessage = "Invalid email format.")] // Improvement over the default EmailAddressAttribute, which is kind of weak.
    string Email,

    [property: Required]
    [property: StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters.")]
    [property: RegularExpression("^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._]))*[a-zA-Z0-9]$", ErrorMessage = "Username can only contain letters, numbers, underscores, dots. Also, it must start and end with a letter or number.")]
    string Username,

    [property: Required]
    [property: MaxLength(150)]
    string FirstName,

    [property: Required]
    [property: MaxLength(150)]
    string LastName,

    [property: MaxLength(200)]
    string? DisplayName = null);
