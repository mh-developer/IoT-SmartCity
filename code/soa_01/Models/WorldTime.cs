using Newtonsoft.Json;
using System;

namespace soa_01.Models
{
    public class WorldTime
    {
        [JsonProperty("abbreviation")] public string Abbreviation;

        [JsonProperty("client_ip")] public string ClientIp;

        [JsonProperty("datetime")] public DateTime Datetime;

        [JsonProperty("day_of_week")] public int DayOfWeek;

        [JsonProperty("day_of_year")] public int DayOfYear;

        [JsonProperty("dst")] public bool Dst;

        [JsonProperty("dst_from")] public object DstFrom;

        [JsonProperty("dst_offset")] public int DstOffset;

        [JsonProperty("dst_until")] public object DstUntil;

        [JsonProperty("raw_offset")] public int RawOffset;

        [JsonProperty("timezone")] public string Timezone;

        [JsonProperty("unixtime")] public int Unixtime;

        [JsonProperty("utc_datetime")] public DateTime UtcDatetime;

        [JsonProperty("utc_offset")] public string UtcOffset;

        [JsonProperty("week_number")] public int WeekNumber;
    }
}