using System.ComponentModel.DataAnnotations;

namespace Lexicora.Api.Features.Users.CreateUser;

// Make sure invalid error is not logged (because this is a possible occurence and no concern for the app).
public record CreateUserRequest(
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    string Email,

    [Required]
    [RegularExpression("^[a-zA-Z0-9_.-]+$", ErrorMessage = "Username can only contain lowercase letters, numbers, underscores, dots, and hyphens.")]
    [StringLength(30, MinimumLength = 3)]
    string Username,

    [Required]
    [MaxLength(100)]
    string FirstName,

    [Required]
    [MaxLength(100)]
    string LastName,

    [MaxLength(100)]
    string? DisplayName = null);
