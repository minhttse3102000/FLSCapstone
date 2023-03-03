using Newtonsoft.Json;
using Reso.Core.Attributes;
using System;
using System.Collections.Generic;
namespace BEAPICapstoneProjectFLS.Requests.ScheduleRequest
{
    [Serializable]
    public class CreateScheduleRequest
    {
        [JsonProperty("IsPublic")]
        public int? IsPublic { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("Description")]
        public string? Description { get; set; }
        [JsonProperty("DateCreate")]
        public DateTime? DateCreate { get; set; }
    }
}
