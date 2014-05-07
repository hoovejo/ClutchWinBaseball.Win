using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClutchWinBaseball.Portable.DataModel
{
    [JsonObject]
    public partial class PlayersResultModel
    {
        [JsonProperty("fieldnames")]
        public List<FieldName> Fieldnames { get; set; }
        [JsonProperty("rows")]
        public List<Row> Rows { get; set; }
    }

    public partial class PlayersResultModel
    {
        [JsonObject]
        public class FieldName
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }

    public partial class PlayersResultModel
    {
        [JsonObject]
        public class Row
        {
            [JsonProperty("year")]
            public string Year { get; set; }
            [JsonProperty("Type")]
            public string Type { get; set; }
            [JsonProperty("G")]
            public string Games { get; set; }
            [JsonProperty("AB")]
            public string AtBat { get; set; }
            [JsonProperty("H")]
            public string Hit { get; set; }
            [JsonProperty("2B")]
            public string SecondBase { get; set; }
            [JsonProperty("3B")]
            public string ThirdBase { get; set; }
            [JsonProperty("HR")]
            public string HomeRun { get; set; }
            [JsonProperty("RBI")]
            public string RunBattedIn { get; set; }
            [JsonProperty("SO")]
            public string StrikeOut { get; set; }
            [JsonProperty("BB")]
            public string BaseBall { get; set; }
            [JsonProperty("AVG")]
            public string Average { get; set; }
        }
    }
}
