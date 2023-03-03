using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.DepartmentRequest
{
    [Serializable]
    public class CreateDepartmentRequest
    {
        [JsonProperty("Id")]
        [Required]
        public string Id { get; set; }
        [JsonProperty("DepartmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("DepartmentGroupId")]
        public string DepartmentGroupId { get; set; }
    }
}
