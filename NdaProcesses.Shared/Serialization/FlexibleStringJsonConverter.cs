using System.Text.Json;
using System.Text.Json.Serialization;

namespace NDAProcesses.Shared.Serialization;

public sealed class FlexibleStringJsonConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => reader.GetRawText(),
            JsonTokenType.True or JsonTokenType.False => reader.GetBoolean().ToString(),
            _ => throw new JsonException($"Unexpected token {reader.TokenType} when parsing a string value.")
        };
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value);
    }
}
