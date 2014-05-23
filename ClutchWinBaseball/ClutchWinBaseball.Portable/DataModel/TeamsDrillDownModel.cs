using Newtonsoft.Json;

namespace ClutchWinBaseball.Portable.DataModel
{
    public partial class TeamsDrillDownModel
    {
        [JsonProperty("game_date")]
        public string GameDate { get; set; }
        [JsonProperty("team_abbr")]
        public string Team { get; set; }
        [JsonProperty("opp_abbr")]
        public string Opponent { get; set; }
        [JsonProperty("win")]
        public string Win { get; set; }
        [JsonProperty("loss")]
        public string Loss { get; set; }
        [JsonProperty("score")]
        public string RunsFor { get; set; }
        [JsonProperty("opp_score")]
        public string RunsAgainst { get; set; }
    }
}
