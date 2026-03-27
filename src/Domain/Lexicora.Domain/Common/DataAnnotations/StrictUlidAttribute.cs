using System.ComponentModel.DataAnnotations;

namespace Lexicora.Domain.Common.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class StrictUlidAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Return success for null to let the [Required] attribute handle nullability separation.
        if (value is null)
        {
            return ValidationResult.Success;
        }

        // Convert value to string to validate its length and characters via the struct extension.
        // If it was already parsed into a Ulid or passed as a string/Span, .ToString() retrieves the 26-char base32 string.
        var stringValue = value.ToString();

        if (stringValue != null && Ulid.TryParseStrict(stringValue, out _))
        {
            return ValidationResult.Success;
        }

        // Return failed validation result with either a custom or default ErrorMessage
        return new ValidationResult(ErrorMessage ?? "The field must be a valid, strict Crockford-formatted ULID.");
    }
}
