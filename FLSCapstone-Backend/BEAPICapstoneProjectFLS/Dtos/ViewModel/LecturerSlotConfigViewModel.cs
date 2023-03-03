using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class LecturerSlotConfigViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("SlotTypeId")]
        public string SlotTypeId { get; set; }
        [JsonProperty("SlotTypeCode")]
        public string? SlotTypeCode { get; set; }
        [JsonProperty("LecturerId")]
        public string LecturerId { get; set; }
        [JsonProperty("LecturerName")]
        public string LecturerName { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("Term")]
        public string Term { get; set; }
        [JsonProperty("PreferenceLevel")]
        public int? PreferenceLevel { get; set; }
        [JsonProperty("IsEnable")]
        public int? IsEnable { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
