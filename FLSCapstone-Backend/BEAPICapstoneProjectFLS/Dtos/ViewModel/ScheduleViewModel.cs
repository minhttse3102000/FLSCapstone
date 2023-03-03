using Newtonsoft.Json;
using Reso.Core.Attributes;
using System;
using System.Collections.Generic;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class ScheduleViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("IsPublic")]
        public int? IsPublic { get; set; }
        [JsonProperty("SemesterId")]
        public string? SemesterId { get; set; }
        [JsonProperty("Term")]
        public string? Term { get; set; }
        [JsonProperty("Description")]
        public string? Description { get; set; }
        [JsonProperty("DateCreate")]
        public DateTime? DateCreate { get; set; }
        [JsonProperty("FormatDateCreate")]
        public string? FormatDateCreate { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
