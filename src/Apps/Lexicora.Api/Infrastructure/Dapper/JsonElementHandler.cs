using System.Data;
using System.Text.Json;

using Dapper;

namespace Lexicora.Api.Infrastructure.Dapper;

public class JsonElementHandler : SqlMapper.TypeHandler<JsonElement>
{
    // How to save it to the DB
    public override void SetValue(IDbDataParameter parameter, JsonElement value)
    {
        parameter.Value = value.GetRawText();
        parameter.DbType = DbType.Object; // Npgsql will see this and map to jsonb

        // --- Alternative ---

        //// If you are using Npgsql 8+ and ConfigureJsonOptions, 
        //// you can often just do this:
        //// parameter.Value = value; 

        //// But for the most compatible "Manual" way:
        //parameter.Value = value.GetRawText();

        //if (parameter is Npgsql.NpgsqlParameter npgsqlParameter)
        //{
        //    npgsqlParameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
        //}
        //else
        //{
        //    parameter.DbType = DbType.Object;
        //}
    }

    // How to read it from the DB
    public override JsonElement Parse(object value)
    {
        return value switch
        {
            string json => JsonSerializer.Deserialize<JsonElement>(json),
            // If the driver already returned a JsonElement (some Npgsql versions do this)
            JsonElement element => element,
            _ => default
        };
    }
}
