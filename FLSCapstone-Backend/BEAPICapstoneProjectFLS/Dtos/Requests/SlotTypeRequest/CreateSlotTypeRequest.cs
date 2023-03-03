using System;
using Newtonsoft.Json;

namespace BEAPICapstoneProjectFLS.Requests.SlotTypeRequest
{
    [Serializable]
    public class CreateSlotTypeRequest
    {
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
        [JsonProperty("SemesterId")]
        public string? SemesterId { get; set; }
    }
}
