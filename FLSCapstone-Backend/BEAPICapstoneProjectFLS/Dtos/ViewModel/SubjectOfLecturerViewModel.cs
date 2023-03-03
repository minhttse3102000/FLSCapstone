using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class SubjectOfLecturerViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("DepartmentManagerId")]
        public string DepartmentManagerId { get; set; }
        [JsonProperty("DepartmentManagerName")]
        public string DepartmentManagerName { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("Term")]
        public string Term { get; set; }
        [JsonProperty("SubjectId")]
        public string SubjectId { get; set; }
        [JsonProperty("SubjectName")]
        public string SubjectName { get; set; }
        [JsonProperty("LecturerId")]
        public string LecturerId { get; set; }
        [JsonProperty("LecturerName")]
        public string LecturerName { get; set; }
        [JsonProperty("FavoritePoint")]
        public int? FavoritePoint { get; set; }
        [JsonProperty("FeedbackPoint")]
        public int? FeedbackPoint { get; set; }
        [JsonProperty("MaxCourseSubject")]
        public int? MaxCourseSubject { get; set; }
        [JsonProperty("isEnable")]
        public int? isEnable { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
