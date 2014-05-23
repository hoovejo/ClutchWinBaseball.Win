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

        [JsonProperty("team_abbr")]
        public string TeamId { get; set; }
        [JsonProperty("league")]
        public string LeagueId { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
