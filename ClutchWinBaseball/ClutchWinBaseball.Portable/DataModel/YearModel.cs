using Newtonsoft.Json;

namespace ClutchWinBaseball.Portable.DataModel
{
    public class YearModel
    {
        [JsonProperty("season")]
        public int Id { get; set; }
    }
}
