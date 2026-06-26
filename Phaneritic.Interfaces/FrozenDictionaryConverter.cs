using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Frozen;

namespace Phaneritic.Interfaces;

public class FrozenDictionaryConverter<TKey, TItem>
    : JsonConverter<FrozenDictionary<TKey, TItem>>
    where TKey: notnull
{
    public override FrozenDictionary<TKey, TItem>? Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options)
    {
        var _items = JsonSerializer.Deserialize<Dictionary<TKey, TItem>>(ref reader, options);
        return _items?.ToFrozenDictionary() ?? FrozenDictionary<TKey, TItem>.Empty;
    }

    public override void Write(
        Utf8JsonWriter writer, 
        FrozenDictionary<TKey, TItem> value, 
        JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToDictionary(), options);
    }
}
