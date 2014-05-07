using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClutchWinBaseball.Portable.DataModel
{
    [JsonObject]
    public partial class TeamsDrillDownModel
    {
        [JsonProperty("fieldnames")]
        public List<FieldName> Fieldnames { get; set; }
        [JsonProperty("rows")]
        public List<Row> Rows { get; set; }
    }

    public partial class TeamsDrillDownModel
    {
        [JsonObject]
        public class FieldName
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }

    public partial class TeamsDrillDownModel
    {
        [JsonObject]
        public class Row
        {
            private string _gameDate;
            [JsonProperty("Game Date")]
            public string GameDate
            {
                get { return DateTime.ParseExact(_gameDate, "yyyy-MM-dd", CultureInfo.CurrentCulture).ToString("yyyy/MM/dd"); }
                set { _gameDate = value; }
            }
            [JsonProperty("team")]
            public string Team { get; set; }
            [JsonProperty("opponent")]
            public string Opponent { get; set; }
            [JsonProperty("win")]
            public string Win { get; set; }
            [JsonProperty("loss")]
            public string Loss { get; set; }
            [JsonProperty("runs_for")]
            public string RunsFor { get; set; }
            [JsonProperty("runs_against")]
            public string RunsAgainst { get; set; }
        }
    }
}
