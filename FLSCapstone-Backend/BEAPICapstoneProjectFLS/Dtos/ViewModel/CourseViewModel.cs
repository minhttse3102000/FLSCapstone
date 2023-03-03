using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class CourseViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("SubjectId")]
        public string? SubjectId { get; set; }
        [JsonProperty("SubjectName")]
        public string? SubjectName { get; set; }
        [JsonProperty("SemesterId")]
        public string? SemesterId { get; set; }
        [JsonProperty("Term")]
        public string? Term { get; set; }
        [JsonProperty("Description")]
        public string? Description { get; set; }
        [JsonProperty("SlotAmount")]
        public int? SlotAmount { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
