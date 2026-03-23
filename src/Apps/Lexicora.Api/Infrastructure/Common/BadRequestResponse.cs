using System.Text.Json.Serialization;

namespace Lexicora.Api.Infrastructure.Common;

public record BadRequestResponse
{
    public BadRequestResponse(string? detail = null, string? error = null)
    {
        Detail = detail;
        Error = error;
    }

    public string Type => "https://tools.ietf.org/html/rfc7231#section-6.5.1";
    public string Title => "Bad Request";
    public int Status => 400;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Detail { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; init; }

    //[property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    //Dictionary<string, object?>? Extensions = null
}
