using System.IO;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ArmConcat.UnitTests
{
    public class ParametersConcatenatorTest
    {
        [Fact]
        public void Build_TwoParameterFilesAdded_ReturnsConcatinatedJson()
        {
            // arrange
            var parametersJson = File.ReadAllText(@"TestFiles\parameters.json");

            // act
            var target = new ParametersConcatenator();
            target.Add("key1", parametersJson);
            target.Add("key2", parametersJson);

            var result = target.Build();

            // assert
            var r = JToken.Parse(result);
            Assert.NotNull(r["parameters"]?["key1_apa"]);
            Assert.Equal("banan", r["parameters"]["key1_apa"]["value"]);
            Assert.NotNull(r["parameters"]?["key2_apa"]);
            Assert.Equal("banan", r["parameters"]["key2_apa"]["value"]);
        }
    }
}