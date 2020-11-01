using Newtonsoft.Json;

namespace soa_01.Models
{
    public class Parkirisca
    {
        [JsonProperty("U_delovnik")] public string UDelovnik { get; set; }

        [JsonProperty("Cena_splosno")] public object CenaSplosno { get; set; }

        [JsonProperty("Ime")] public string Ime { get; set; }

        [JsonProperty("A_St_Mest")] public int? AStMest { get; set; }

        [JsonProperty("Invalidi_St_mest")] public int? InvalidiStMest { get; set; }

        [JsonProperty("Cena_ura_Eur")] public string CenaUraEur { get; set; }

        [JsonProperty("KoordinataX")] public int? KoordinataX { get; set; }

        [JsonProperty("Opis")] public string Opis { get; set; }

        [JsonProperty("ID_Parkirisca")] public int IDParkirisca { get; set; }

        [JsonProperty("U_splosno")] public string USplosno { get; set; }

        [JsonProperty("zasedenost")] public Zasedenost Zasedenost { get; set; }

        [JsonProperty("U_sobota")] public string USobota { get; set; }

        [JsonProperty("St_mest")] public int StMest { get; set; }

        [JsonProperty("KoordinataX_wgs")] public double KoordinataXWgs { get; set; }

        [JsonProperty("Cena_dan_Eur")] public object CenaDanEur { get; set; }

        [JsonProperty("ID_ParkiriscaNC")] public object IDParkiriscaNC { get; set; }

        [JsonProperty("KoordinataY")] public int? KoordinataY { get; set; }

        [JsonProperty("Cena_mesecna_Eur")] public object CenaMesecnaEur { get; set; }

        [JsonProperty("KoordinataY_wgs")] public double KoordinataYWgs { get; set; }

        [JsonProperty("Upravljalec")] public string Upravljalec { get; set; }

        [JsonProperty("Tip_parkirisca")] public string TipParkirisca { get; set; }
    }
}