using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Phaneritic.Interfaces;

public class FrozenSetConverter<TItem> 
    : JsonConverter<FrozenSet<TItem>>
{
    public override FrozenSet<TItem>? Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options)
    {
        var _items = JsonSerializer.Deserialize<TItem[]>(ref reader, options);
        return _items?.ToFrozenSet() ?? [];
    }

    public override void Write(
        Utf8JsonWriter writer, 
        FrozenSet<TItem> value, 
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var _item in value)
        {
            JsonSerializer.Serialize(writer, _item, options); 
        }
        writer.WriteEndArray();
    }
}
