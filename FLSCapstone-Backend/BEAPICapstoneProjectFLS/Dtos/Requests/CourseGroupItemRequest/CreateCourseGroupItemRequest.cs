using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.CourseGroupItemRequest
{
    [Serializable]
    public class CreateCourseGroupItemRequest
    {
        [JsonProperty("LecturerCourseGroupId")]
        public string LecturerCourseGroupId { get; set; }
        [JsonProperty("CourseId")]
        public string CourseId { get; set; }
        [JsonProperty("PriorityCourse")]
        public int? PriorityCourse { get; set; }
    }
}
