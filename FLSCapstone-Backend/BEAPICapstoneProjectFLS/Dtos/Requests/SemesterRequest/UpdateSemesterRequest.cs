using System;
using Newtonsoft.Json;

namespace BEAPICapstoneProjectFLS.Requests.SemesterRequest
{
    [Serializable]
    public class UpdateSemesterRequest
    {
        [JsonProperty("Term")]
        public string Term { get; set; }
        [JsonProperty("DateStart")]
        public DateTime? DateStart { get; set; }
        [JsonProperty("DateEnd")]
        public DateTime? DateEnd { get; set; }
        [JsonProperty("State")]
        public int? State { get; set; }
    }
}
