using System.ComponentModel.DataAnnotations;

namespace Lexicora.Api.Features.Users.UpdateUser;

public record UpdateUserRequest(
    [property: Required]
    [property: EmailAddress]
    [property: RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$", ErrorMessage = "Invalid e-mail format.")] // Improvement over the default EmailAddressAttribute, which is kind of weak.
    [property: MaxLength(254)]
    string Email,

    [property: Required]
    [property: StringLength(30, MinimumLength = 3)]
    [property: RegularExpression("^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._]))*[a-zA-Z0-9]$", ErrorMessage = "Username can only contain letters, numbers, underscores, dots. Also, it must start and end with a letter or number.")]
    string Username,

    [property: Required]
    [property: MaxLength(150)]
    string FirstName,

    [property: Required]
    [property: MaxLength(150)]
    string LastName,

    [property: MaxLength(200)] // Maybe increase to 255 (db limit).
    string? DisplayName = null);
