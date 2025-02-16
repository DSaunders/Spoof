using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Models;

public class JsonBody
{
    private readonly Dictionary<string, object> _properties;

    private JsonBody(Dictionary<string, object> props)
    {
        _properties = props;
    }

    public override string ToString() =>
        JsonConvert.SerializeObject(_properties, Formatting.Indented);

    public object this[string key] => Get(key);

    private object Get(string key)
    {
        var props = key.Split(".");
        var current = _properties;

        var ptr = 0;
        while (ptr < props.Length)
        {
            var requestedProperty = props[ptr];

            // The property we want doesn't exist
            if (!current!.ContainsKey(requestedProperty))
                return string.Empty;

            // We want a nested property, keep going down the chain
            if (current[requestedProperty] is Dictionary<string, object> dict)
            {
                current = dict;
                ptr++;
                continue;
            }

            // Property is a list 
            if (current[requestedProperty] is List<object> list)
            {
                ptr++;
                var index = int.Parse(props[ptr]);

                if (index >= list.Count)
                    return string.Empty;

                var item = list[index];

                if (item is Dictionary<string, object>)
                    current = list[index] as Dictionary<string, object>;
                else
                    return item;

                ptr++;
                continue;
            }

            // This is a value type, so we can't go any further, but we're
            // not at the end of the query. We're not expecting that
            if (ptr != props.Length - 1)
                return string.Empty;

            // We've reached the end of the query, and found a value type
            // ... this is the happy path
            return current[requestedProperty];
        }

        return JsonConvert.SerializeObject(current, Formatting.Indented);
    }

    public static implicit operator JsonBody(JObject token) => new(JTokenToDictionary(token));

    private static Dictionary<string, object> JTokenToDictionary(JToken token)
    {
        var dict = new Dictionary<string, object>();

        foreach (var prop in token.Children<JProperty>())
        {
            var value = ParseJToken(prop.Value);
            dict.Add(prop.Name, value);
        }

        return dict;
    }

    private static object ParseJToken(JToken prop)
    {
        switch (prop.Type)
        {
            case JTokenType.String:
            case JTokenType.Guid:
                return prop.Value<string>()!;

            case JTokenType.Date:
                return prop.Value<DateTime>()!;

            case JTokenType.Integer:
                return prop.Value<int>();

            case JTokenType.Boolean:
                return prop.Value<bool>();

            case JTokenType.Float:
                return prop.Value<float>();

            case JTokenType.Array:
                var array = new List<object>();
                foreach (var item in prop.Children())
                {
                    if (item is JObject itemProp)
                        array.Add(JTokenToDictionary(itemProp));
                    else
                        array.Add(ParseJToken(item));
                }

                return array;

            case JTokenType.Object:
                var childDict = JTokenToDictionary(prop);
                return childDict;
        }

        return string.Empty;
    }
}