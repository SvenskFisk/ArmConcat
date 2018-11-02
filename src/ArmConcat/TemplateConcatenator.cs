using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace ArmConcat
{
    internal class TemplateConcatenator
    {
        internal const string BaseContent =
@"{
    ""$schema"": ""http://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#"",
    ""contentVersion"": ""1.0.0.0"",
    ""parameters"": {},
    ""variables"": {},
    ""resources"": []
}";

        internal JToken _template = JToken.Parse(BaseContent);

        public void Add(string key, string template)
        {
            var addTemplate = JToken.Parse(template);

            // update parameter property
            var parameters = (JObject)_template["parameters"];
            var parameterNames = new List<string>();
            foreach (JProperty addParameter in addTemplate["parameters"].Children())
            {
                parameters.Add($"{key}_{addParameter.Name}", addParameter.Value);
                parameterNames.Add(addParameter.Name);
            }

            // update resources array
            var resources = (JArray)_template["resources"];
            foreach (JObject addResource in addTemplate["resources"])
            {
                var resourceString = addResource.ToString();
                foreach (var parameterName in parameterNames)
                {
                    resourceString = resourceString.Replace($"parameters('{parameterName}')", $"parameters('{key}_{parameterName}')");
                }

                var replacedResource = JToken.Parse(resourceString);
                resources.Add(replacedResource);
            }
        }

        public string Build()
        {
            return _template.ToString();
        }
    }
}