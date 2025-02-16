using Newtonsoft.Json.Linq;

namespace Runner.Scaffolding;

internal class ScaffoldTemplate
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public JToken Route { get; set; } = new JObject();
}