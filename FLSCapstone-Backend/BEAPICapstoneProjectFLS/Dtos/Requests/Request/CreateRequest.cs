using System;
using Newtonsoft.Json;

namespace BEAPICapstoneProjectFLS.Requests.Request
{
    [Serializable]
    public class CreateRequest
    {
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("LecturerId")]
        public string LecturerId { get; set; }
        [JsonProperty("DepartmentManagerId")]
        public string DepartmentManagerId { get; set; }
        [JsonProperty("SubjectId")]
        public string? SubjectId { get; set; }
        [JsonProperty("SemesterId")]
        public string? SemesterId { get; set; }
    }
}
