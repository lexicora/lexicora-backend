namespace Lexicora.Domain.Common;

public class UlidConfigurations
{
    public static readonly Ulid.GenerationOptions BatchOptions = new()
    {
        Monotonicity = Ulid.GenerationOptions.MonotonicityOptions.MonotonicIncrement
    };
}
