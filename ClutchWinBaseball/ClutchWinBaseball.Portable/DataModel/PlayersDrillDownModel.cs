using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClutchWinBaseball.Portable.DataModel
{
    [JsonObject]
    public partial class PlayersDrillDownModel
    {
        [JsonProperty("fieldnames")]
        public List<FieldName> Fieldnames { get; set; }
        [JsonProperty("rows")]
        public List<Row> Rows { get; set; }
    }

    public partial class PlayersDrillDownModel
    {
        [JsonObject]
        public class FieldName
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }

    public partial class PlayersDrillDownModel
    {
        [JsonObject]
        public class Row
        {
            private string _gameDate;
            [JsonProperty("Game Date")]
            public string GameDate
            {
                get { return DateTime.ParseExact(_gameDate, "yyyy-MM-ddThh:mm:ssZ", CultureInfo.CurrentCulture).ToString("yyyy/MM/dd"); }
                set { _gameDate = value; }
            }
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
