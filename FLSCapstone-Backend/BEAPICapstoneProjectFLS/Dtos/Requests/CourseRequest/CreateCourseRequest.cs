using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.CourseRequest
{
    [Serializable]
    public class CreateCourseRequest
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("SubjectId")]
        public string? SubjectId { get; set; }
        [JsonProperty("SemesterId")]
        public string? SemesterId { get; set; }
        [JsonProperty("Description")]
        public string? Description { get; set; }
        [JsonProperty("SlotAmount")]
        public int? SlotAmount { get; set; }
    }
}
