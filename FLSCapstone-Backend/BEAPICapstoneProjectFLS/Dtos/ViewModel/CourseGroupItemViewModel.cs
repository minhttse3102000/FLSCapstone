using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class CourseGroupItemViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("LecturerCourseGroupId")]
        public string LecturerCourseGroupId { get; set; }
        [JsonProperty("GroupName")]
        public string GroupName { get; set; }
        [JsonProperty("CourseId")]
        public string CourseId { get; set; }
        [JsonProperty("PriorityCourse")]
        public int? PriorityCourse { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
