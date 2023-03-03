using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class CourseAssignViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("LecturerId")]
        public string? LecturerId { get; set; }
        [JsonProperty("LecturerName")]
        public string? LecturerName { get; set; }
        [JsonProperty("CourseId")]
        public string? CourseId { get; set; }
        [JsonProperty("SlotTypeId")]
        public string? SlotTypeId { get; set; }
        [JsonProperty("SlotTypeCode")]
        public string? SlotTypeCode { get; set; }
        [JsonProperty("Duration")]
        public string? Duration { get; set; }
        [JsonProperty("ConvertDateOfWeek")]
        public string? ConvertDateOfWeek { get; set; }
        [JsonProperty("ScheduleId")]
        public string? ScheduleId { get; set; }
        [JsonProperty("isAssign")]
        public int? isAssign { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
