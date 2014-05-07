using ClutchWinBaseball.Portable.Common;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ClutchWinBaseball.Portable.DataModel
{
    [JsonObject]
    public partial class PitcherModel
    {
        [JsonProperty("fieldnames")]
        public List<FieldName> Fieldnames { get; set; }
        [JsonProperty("rows")]
        public List<Row> Rows { get; set; }
    }

    public partial class PitcherModel
    {
        [JsonObject]
        public class FieldName
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }

    public partial class PitcherModel
    {
        [JsonObject]
        public class Row
        {
            public string GetDisplayName()
            {
                return FirstName + Config.Space + LastName;
            }

            [JsonProperty("retro_id")]
            public string RetroId { get; set; }
            [JsonProperty("last_name")]
            public string LastName { get; set; }
            [JsonProperty("first_name")]
            public string FirstName { get; set; }
        }
    }
}
