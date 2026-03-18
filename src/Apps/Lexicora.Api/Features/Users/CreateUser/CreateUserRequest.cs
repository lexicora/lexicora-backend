using System.ComponentModel.DataAnnotations;

namespace Lexicora.Api.Features.Users.CreateUser;

public record CreateUserRequest(
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    string Email,

    [Required]
    [RegularExpression("^[a-zA-Z0-9_.-]+$", ErrorMessage = "Username can only contain letters, numbers, underscores, dots, and hyphens.")]
    [StringLength(30, MinimumLength = 3)]
    string Username,

    [Required]
    [MaxLength(150)]
    string FirstName,

    [Required]
    [MaxLength(150)]
    string LastName,

    [MaxLength(200)]
    string? DisplayName = null);
