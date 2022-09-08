namespace SingleTasker.Common;

using Newtonsoft.Json;

public class SerializationSettings
{
    public static JsonSerializerSettings Configuration => new() { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.Indented };
}
