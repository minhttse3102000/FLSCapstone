using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.CourseRequest
{
    [Serializable]
    public class CourseOfLecturer
    {
        [JsonProperty("LecturerID")]
        public string LecturerID { get; set; }
        [JsonProperty("AvailableCourse")]
        public int AvailableCourse { get; set; }
        [JsonProperty("MinCourse")]
        public int MinCourse { get; set; }
        [JsonProperty("MaxCourse")]
        public int MaxCourse { get; set; }
    }
}
