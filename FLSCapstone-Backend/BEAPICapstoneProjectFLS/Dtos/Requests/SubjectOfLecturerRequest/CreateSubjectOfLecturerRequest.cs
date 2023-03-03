using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.SubjectOfLecturerRequest
{
    [Serializable]
    public class CreateSubjectOfLecturerRequest
    {
        [JsonProperty("DepartmentManagerId")]
        public string DepartmentManagerId { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("SubjectId")]
        public string SubjectId { get; set; }
        [JsonProperty("LecturerId")]
        public string LecturerId { get; set; }
        [JsonProperty("FavoritePoint")]
        public int? FavoritePoint { get; set; }
        [JsonProperty("FeedbackPoint")]
        public int? FeedbackPoint { get; set; }
        [JsonProperty("MaxCourseSubject")]
        public int? MaxCourseSubject { get; set; }
        [JsonProperty("isEnable")]
        public int? isEnable { get; set; }
    }
}
