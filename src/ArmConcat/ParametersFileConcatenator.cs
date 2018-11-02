using Newtonsoft.Json.Linq;

namespace ArmConcat
{
    internal class ParametersConcatenator
    {
        private const string BaseContent =
@"{
  ""$schema"": ""https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"",
  ""contentVersion"": ""1.0.0.0"",
  ""parameters"": {}
}";

        internal JToken _template = JToken.Parse(BaseContent);

        public void Add(string key, string template)
        {
            var addTemplate = JToken.Parse(template);

            // update parameter property
            var parameters = (JObject)_template["parameters"];
            foreach (JProperty addParameter in addTemplate["parameters"].Children())
            {
                parameters.Add($"{key}_{addParameter.Name}", addParameter.Value);
            }
        }

        public string Build()
        {
            return _template.ToString();
        }
    }
}