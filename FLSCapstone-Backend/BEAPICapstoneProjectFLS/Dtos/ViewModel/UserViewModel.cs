using Newtonsoft.Json;
using Reso.Core.Attributes;
using System;
using System.Collections.Generic;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class UserViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("Email")]
        public string? Email { get; set; }
        [JsonProperty("Dob")]
        public DateTime? Dob { get; set; }
        [JsonProperty("Gender")]
        public int? Gender { get; set; }
        [JsonProperty("Idcard")]
        public string? Idcard { get; set; }
        [JsonProperty("Address")]
        public string? Address { get; set; }
        [JsonProperty("Phone")]
        public string? Phone { get; set; }
        [JsonProperty("PriorityLecturer")]
        public int? PriorityLecturer { get; set; }
        [JsonProperty("IsFullTime")]
        public int? IsFullTime { get; set; }
        [JsonProperty("DepartmentId")]
        public string? DepartmentId { get; set; }
        [JsonProperty("DepartmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("RoleIDs")]
        [Contain]
        public List<string> RoleIDs { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
        [JsonProperty("DateOfBirthFormatted")]
        public string? DateOfBirthFormatted { get; set; }
    }
}
