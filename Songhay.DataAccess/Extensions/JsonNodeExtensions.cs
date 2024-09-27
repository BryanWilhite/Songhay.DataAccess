using System.Text.Json;
using System.Text.Json.Nodes;

namespace Songhay.DataAccess.Extensions;

public static class JsonNodeExtensions
{
    public static JsonNode? ToJsonNode(this object boxed)
    {
        string json = JsonSerializer.Serialize(boxed);

        return JsonNode.Parse(json);
    }
}