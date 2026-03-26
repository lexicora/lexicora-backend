using System.ComponentModel.DataAnnotations;

namespace Lexicora.Api.Infrastructure.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    // TODO: Debug this method to potentially figure out why no custom message is returned.
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argToValidate = context.Arguments.OfType<T>().FirstOrDefault();
        if (argToValidate is null) return await next(context);

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(argToValidate);

        var isValid = Validator.TryValidateObject(argToValidate, validationContext, validationResults, true);

        if (isValid) return await next(context);

        var errors = validationResults
            .GroupBy(e => e.MemberNames.FirstOrDefault() ?? string.Empty, e => e.ErrorMessage ?? string.Empty)
            .ToDictionary(g => g.Key, g => g.ToArray());

        // Returns a 400 Bad Request gracefully. Nothing is written to the Error logs.
        return TypedResults.ValidationProblem(errors);
    }
}
