using Newtonsoft.Json;

namespace ClutchWinBaseball.Portable.DataModel
{
    public class YearModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
