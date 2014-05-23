using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClutchWinBaseball.Portable.DataModel
{
    public class TeamsResultModel
    {
        [JsonProperty("season")]
        public string Year { get; set; }
        [JsonProperty("team_abbr")]
        public string Team { get; set; }
        [JsonProperty("opp_abbr")]
        public string Opponent { get; set; }
        [JsonProperty("win")]
        public string Wins { get; set; }
        [JsonProperty("loss")]
        public string Losses { get; set; }
        [JsonProperty("score")]
        public string RunsFor { get; set; }
        [JsonProperty("opp_score")]
        public string RunsAgainst { get; set; }
    }
}
