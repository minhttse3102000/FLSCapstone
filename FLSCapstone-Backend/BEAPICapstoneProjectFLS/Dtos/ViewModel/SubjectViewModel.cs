using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class SubjectViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("SubjectName")]
        public string? SubjectName { get; set; }
        [JsonProperty("Description")]
        public string? Description { get; set; }
        [JsonProperty("DepartmentId")]
        public string? DepartmentId { get; set; }
        [JsonProperty("DepartmentName")]
        public string? DepartmentName { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
