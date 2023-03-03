using System;
using Newtonsoft.Json;

namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class SlotTypeViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("SlotTypeCode")]
        public string SlotTypeCode { get; set; }
        [JsonProperty("TimeStart")]
        public TimeSpan? TimeStart { get; set; }
        [JsonProperty("TimeEnd")]
        public TimeSpan? TimeEnd { get; set; }
        [JsonProperty("SlotNumber")]
        public int? SlotNumber { get; set; }
        [JsonProperty("DateOfWeek")]
        public int? DateOfWeek { get; set; }
        [JsonProperty("Duration")]
        public string? Duration { get; set; }
        [JsonProperty("SemesterId")]
        public string? SemesterId { get; set; }
        [JsonProperty("Term")]
        public string? Term { get; set; }
        [JsonProperty("ConvertDateOfWeek")]
        public string? ConvertDateOfWeek { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
