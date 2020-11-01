using Newtonsoft.Json;

namespace soa_01.Models
{
    public class Zasedenost
    {
        [JsonProperty("Cas")] public string Cas { get; set; }

        [JsonProperty("ID_ParkiriscaNC")] public int IDParkiriscaNC { get; set; }

        [JsonProperty("Cas_timestamp")] public int CasTimestamp { get; set; }

        [JsonProperty("P_kratkotrajniki")] public object PKratkotrajniki { get; set; }
    }
}