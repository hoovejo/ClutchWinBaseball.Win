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

        [JsonProperty("player_retro_id")]
        public string RetroPlayerId { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }

}
