using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.SubjectRequest
{
    [Serializable]
    public class CreateSubjectRequest
    {
        [JsonProperty("Id")]
        [Required]
        public string Id { get; set; }
        [JsonProperty("SubjectName")]
        public string SubjectName { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("DepartmentId")]
        public string DepartmentId { get; set; }
    }
}
