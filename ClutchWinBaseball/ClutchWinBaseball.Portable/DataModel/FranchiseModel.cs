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

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("retro_id")]
        public string RetroId { get; set; }
        [JsonProperty("league_id")]
        public string LeagueId { get; set; }
        [JsonProperty("division_id")]
        public string DivisionId { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("alternate_name")]
        public object AlternateName { get; set; }
        [JsonProperty("first_game_dt")]
        public string FirstGameDt { get; set; }
        [JsonProperty("last_game_dt")]
        public object LastGameDt { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }
}
