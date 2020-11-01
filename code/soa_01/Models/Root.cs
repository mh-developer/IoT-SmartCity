using Newtonsoft.Json;
using System.Collections.Generic;

namespace soa_01.Models
{
    public class Root
    {
        [JsonProperty("updated")] public double Updated { get; set; }

        [JsonProperty("copyright")] public string Copyright { get; set; }

        [JsonProperty("Parkirisca")] public List<Parkirisca> Parkirisca { get; set; }
    }
}