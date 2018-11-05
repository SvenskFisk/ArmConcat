using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ArmConcat
{
    class Program
    {
        static int Main(string[] args)
        {
            var returnCode = 0;
            if (!ValidateArgs(args, out returnCode))
            {
                return returnCode;
            }

            var outFoldler = args[0];

            var inputs = Enumerable.Range(0, (args.Length - 1) / 2)
                .Select(x => (key: args[x * 2 + 1].Substring(1), folder: args[x * 2 + 2]))
                .ToArray();

            var tc = new TemplateConcatenator();
            foreach (var input in inputs)
            {
                tc.Add(input.key, File.ReadAllText(Path.Combine(input.folder, "template.json")));
            }

            File.WriteAllText(Path.Combine(outFoldler, "template.json"), tc.Build());

            var pc = new ParametersConcatenator();
            foreach (var input in inputs.Where(x => File.Exists(Path.Combine(x.folder, "parameters.json"))))
            {
                pc.Add(input.key, File.ReadAllText(Path.Combine(input.folder, "parameters.json")));
            }

            File.WriteAllText(Path.Combine(outFoldler, "parameters.json"), pc.Build());

            System.Console.WriteLine("Done!");
            return 0;
        }

        static bool ValidateArgs(string[] args, out int returnCode)
        {
            returnCode = -1;
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: dotnet ArmConcat.dll <out-directory> -<key 1> <in-directory 1> -<key 2> <in-directory 2>...");
                return false;
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Invalid out-folder: " + args[0]);
                return false;
            }

            for (int i = 0; i < (args.Length - 1) / 2; i++)
            {
                if (!args[i * 2 + 1].StartsWith("-"))
                {
                    Console.WriteLine("Invalid key: " + args[i * 2 + 1]);
                    return false;
                }

                if (!Directory.Exists(args[i * 2 + 2]))
                {
                    Console.WriteLine("Invalid in-directory: " + args[i * 2 + 2]);
                    return false;
                }

                var file = Path.Combine(args[i * 2 + 2], "template.json");
                if (!File.Exists(file))
                {
                    Console.WriteLine("Missing template file: " + file);
                    return false;
                }
            }

            returnCode = 0;
            return true;
        }
    }
}
