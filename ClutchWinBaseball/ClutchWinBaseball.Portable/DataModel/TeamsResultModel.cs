using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClutchWinBaseball.Portable.DataModel
{
    [JsonObject]
    public partial class TeamsResultModel
    {
        [JsonProperty("fieldnames")]
        public List<FieldName> Fieldnames { get; set; }
        [JsonProperty("rows")]
        public List<Row> Rows { get; set; }
    }

    public partial class TeamsResultModel
    {
        [JsonObject]
        public class FieldName
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }

    public partial class TeamsResultModel
    {
        [JsonObject]
        public class Row
        {
            [JsonProperty("year")]
            public string Year { get; set; }
            [JsonProperty("games")]
            public string Games { get; set; }
            [JsonProperty("team")]
            public string Team { get; set; }
            [JsonProperty("opponent")]
            public string Opponent { get; set; }
            [JsonProperty("wins")]
            public string Wins { get; set; }
            [JsonProperty("losses")]
            public string Losses { get; set; }
            [JsonProperty("runs_for")]
            public string RunsFor { get; set; }
            [JsonProperty("runs_against")]
            public string RunsAgainst { get; set; }
        }
    }
}
