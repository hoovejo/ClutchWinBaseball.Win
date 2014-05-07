using ClutchWinBaseball.Portable.Common;
using Newtonsoft.Json;

namespace ClutchWinBaseball.Portable.DataModel
{
    public class BatterModel
    {
        public string GetDisplayName()
        {
            return FirstName + Config.Space + LastName;
        }

        [JsonProperty("game_type")]
        public string GameType { get; set; }
        [JsonProperty("year_id")]
        public int YearId { get; set; }
        [JsonProperty("retro_team_id")]
        public string RetroTeamId { get; set; }
        [JsonProperty("retro_player_id")]
        public string RetroPlayerId { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("bat_hand")]
        public string BatHand { get; set; }
        [JsonProperty("pit_hand")]
        public string PitHand { get; set; }
        [JsonProperty("rep_team_id")]
        public string RepTeamId { get; set; }
        [JsonProperty("pos_tx")]
        public string PosTx { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
    }

}
