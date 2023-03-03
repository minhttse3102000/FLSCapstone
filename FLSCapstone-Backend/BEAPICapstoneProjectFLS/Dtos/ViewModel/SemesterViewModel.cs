using System;
using Newtonsoft.Json;

namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class SemesterViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("Term")]
        public string? Term { get; set; }
        [JsonProperty("DateStart")]
        public DateTime? DateStart { get; set; }
        [JsonProperty("DateStartFormat")]
        public string? DateStartFormat { get; set; }

        [JsonProperty("DateEnd")]
        public DateTime? DateEnd { get; set; }
        [JsonProperty("DateEndFormat")]
        public string? DateEndFormat { get; set; }
        [JsonProperty("State")]
        public int? State { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
        [JsonProperty("DateStatus")]
        public string? DateStatus { get; set; }

    }
}
