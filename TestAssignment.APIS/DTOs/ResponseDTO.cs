using Newtonsoft.Json;

namespace TestAssignment.APIS.DTOs
{
    public class ResponseDTO
    {
        [JsonProperty("country_name")]
        public string country_name { get; set; }

        [JsonProperty("country_code3")]
        public string country_code3 { get; set; }

        [JsonProperty("current_time")]
        public string TimeStamp { get; set; }
    }
}
