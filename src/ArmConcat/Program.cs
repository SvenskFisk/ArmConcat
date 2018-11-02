using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ArmConcat
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = JToken.Parse(File.ReadAllText(args.Length == 0 ? "settings.json" : args[0]));
            var outTemplatePath = (string)settings["outputTemplateFile"];
            var outParametersPath = (string)settings["outputParametersFile"];

            var inputs = ((JArray)settings["inputs"])
                .Select(x => (key: (string)x["key"], templateFile: (string)x["templateFile"], parametersFile: (string)x["parametersFile"]))
                .ToArray();

            var tc = new TemplateConcatenator();
            foreach (var input in inputs)
            {
                tc.Add(input.key, File.ReadAllText(input.templateFile));
            }

            File.WriteAllText(outTemplatePath, tc.Build());

            if (!string.IsNullOrWhiteSpace(outParametersPath))
            {
                var pc = new ParametersConcatenator();
                foreach (var input in inputs.Where(x => !string.IsNullOrWhiteSpace(x.parametersFile)))
                {
                    pc.Add(input.key, File.ReadAllText(input.parametersFile));
                }

                File.WriteAllText(outParametersPath, pc.Build());
            }

            System.Console.WriteLine("Done!");
        }
    }
}
