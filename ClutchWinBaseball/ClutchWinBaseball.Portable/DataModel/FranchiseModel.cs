using ClutchWinBaseball.Portable.Common;
using Newtonsoft.Json;

namespace ClutchWinBaseball.Portable.DataModel
{
    public class FranchiseModel
    {
        public string GetDisplayName()
        {
            return Location + Config.Space + Name;
        }

        public string GetDetail()
        {
            return LeagueId;
        }

        [JsonProperty("franchise_abbr")]
        public string RetroId { get; set; }
        [JsonProperty("league")]
        public string LeagueId { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
