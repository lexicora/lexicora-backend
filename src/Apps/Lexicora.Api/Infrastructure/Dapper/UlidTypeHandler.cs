using System.Data;

using Dapper;

namespace Lexicora.Api.Infrastructure.Dapper;

public class UlidTypeHandler : SqlMapper.TypeHandler<Ulid>
{
    // Reading FROM the database (Guid -> Ulid)
    public override Ulid Parse(object value)
    {
        return value switch
        {
            Guid guid => new Ulid(guid),
            string str => Ulid.Parse(str),
            byte[] bytes => new Ulid(bytes),
            _ => throw new InvalidCastException($"Cannot convert {value?.GetType()} to Ulid.")
        };
    }

    // Writing TO the database (Ulid -> Guid)
    public override void SetValue(IDbDataParameter parameter, Ulid value)
    {
        parameter.DbType = DbType.Guid;
        parameter.Value = value.ToGuid();
    }
}
