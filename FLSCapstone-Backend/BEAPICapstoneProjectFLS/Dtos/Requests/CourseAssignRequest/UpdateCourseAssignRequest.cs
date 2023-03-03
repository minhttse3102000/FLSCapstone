using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.CourseAssignRequest
{
    [Serializable]
    public class UpdateCourseAssignRequest
    {
        [JsonProperty("LecturerId")]
        public string? LecturerId { get; set; }
        [JsonProperty("CourseId")]
        public string? CourseId { get; set; }
        [JsonProperty("SlotTypeId")]
        public string? SlotTypeId { get; set; }
        [JsonProperty("ScheduleId")]
        public string? ScheduleId { get; set; }
        [JsonProperty("isAssign")]
        public int? isAssign { get; set; }

    }
}
