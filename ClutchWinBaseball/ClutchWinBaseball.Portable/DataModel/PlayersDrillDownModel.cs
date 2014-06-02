using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ClutchWinBaseball.Portable.DataModel
{
    public class PlayersDrillDownModel
    {
        [JsonProperty("game_date")]
        public string GameDate { get; set; }
        [JsonProperty("ab")]
        public string AtBat { get; set; }
        [JsonProperty("h")]
        public string Hit { get; set; }
        [JsonProperty("bb")]
        public string Walks { get; set; }
        [JsonProperty("k")]
        public string StrikeOut { get; set; }
        [JsonProperty("h_2b")]
        public string SecondBase { get; set; }
        [JsonProperty("h_3b")]
        public string ThirdBase { get; set; }
        [JsonProperty("hr")]
        public string HomeRun { get; set; }
        [JsonProperty("rbi_ct")]
        public string RunBattedIn { get; set; }

        public double Average { get { return (double.Parse(Hit) <= 0.00) ? 0.00 : (double.Parse(Hit) / double.Parse(AtBat)); } }
    }
}
