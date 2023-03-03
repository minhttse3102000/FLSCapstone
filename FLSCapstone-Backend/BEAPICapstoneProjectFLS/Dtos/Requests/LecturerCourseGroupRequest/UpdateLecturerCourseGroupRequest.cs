using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.LecturerCourseGroupRequest
{
    [Serializable]
    public class UpdateLecturerCourseGroupRequest
    {
        [JsonProperty("LecturerId")]
        public string LecturerId { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("GroupName")]
        public string GroupName { get; set; }
        [JsonProperty("MinCourse")]
        public int? MinCourse { get; set; }
        [JsonProperty("MaxCourse")]
        public int? MaxCourse { get; set; }
    }
}
