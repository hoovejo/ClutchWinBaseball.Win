using ClutchWinBaseball.Portable.Common;
using Newtonsoft.Json;

namespace ClutchWinBaseball.Portable.DataModel
{
    public class TeamModel
    {
        public string GetDisplayName()
        {
            return Location + Config.Space + Name;
        }

        public string GetDetail()
        {
            return LeagueId;
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("year_id")]
        public int YearId { get; set; }
        [JsonProperty("team_id")]
        public string TeamId { get; set; }
        [JsonProperty("team_type")]
        public string TeamType { get; set; }
        [JsonProperty("league_id")]
        public string LeagueId { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
