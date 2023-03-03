using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.LecturerSlotConfigRequest
{
    [Serializable]
    public class UpdateLecturerSlotConfigRequest
    {
        [JsonProperty("SlotTypeId")]
        public string? SlotTypeId { get; set; }
        [JsonProperty("LecturerId")]
        public string? LecturerId { get; set; }
        [JsonProperty("SemesterId")]
        public string? SemesterId { get; set; }
        [JsonProperty("PreferenceLevel")]
        public int? PreferenceLevel { get; set; }
        [JsonProperty("IsEnable")]
        public int? IsEnable { get; set; }

    }
}
