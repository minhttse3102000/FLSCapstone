using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class LecturerCourseGroupViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("LecturerId")]
        public string LecturerId { get; set; }
        [JsonProperty("LecturerName")]
        public string LecturerName { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("Term")]
        public string Term { get; set; }
        [JsonProperty("GroupName")]
        public string GroupName { get; set; }
        [JsonProperty("MinCourse")]
        public int? MinCourse { get; set; }
        [JsonProperty("MaxCourse")]
        public int? MaxCourse { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
