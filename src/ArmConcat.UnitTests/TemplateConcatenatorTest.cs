using System.IO;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ArmConcat.UnitTests
{
    public class TemplateConcatenatorTest
    {
        [Fact]
        public void Build_TwoTemplatesAdded_ReturnsConcatinatedJson()
        {
            // arrange
            var templateJson = File.ReadAllText(@"TestFiles\template.json");

            // act
            var target = new TemplateConcatenator();
            target.Add("key1", templateJson);
            target.Add("key2", templateJson);

            var result = target.Build();

            // assert
            var r = JToken.Parse(result);
            Assert.NotNull(r["parameters"]?["key1_apa"]);
            Assert.NotNull(r["parameters"]?["key2_apa"]);
            Assert.Equal("[parameters('key1_apa')]", r["resources"][0]["name"]);
            Assert.Equal("[parameters('key2_apa')]", r["resources"][1]["name"]);
        }
    }
}