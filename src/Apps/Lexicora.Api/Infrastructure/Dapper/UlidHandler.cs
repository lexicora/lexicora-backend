using System.Data;

using Dapper;

namespace Lexicora.Api.Infrastructure.Dapper;

public class UlidHandler : SqlMapper.TypeHandler<Ulid>
{
    // Writing TO the database (Ulid -> Guid)
    public override void SetValue(IDbDataParameter parameter, Ulid value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value.ToGuid();
    }

    // Reading FROM the database (Guid -> Ulid)
    public override Ulid Parse(object value)
    {
        switch (value)
        {
            // 1. Handle Nulls
            case null:
            case DBNull:
                return default;
            // 2. Handle the case where Npgsql returns the Guid we need
            case Guid g:
                return Ulid.New(g);
            // 3. Handle string/byte conversions
            case string s:
                return Ulid.Parse(s);
            case byte[] b:
                return Ulid.New(b);
            default:
                // 4. Fallback for weirdness
                try
                {
                    return Ulid.Parse(value.ToString());
                }
                catch
                {
                    throw new InvalidCastException($"Dapper Handler: Cannot convert {value.GetType()} to Ulid.");
                }

                break;
        }
    }
}
