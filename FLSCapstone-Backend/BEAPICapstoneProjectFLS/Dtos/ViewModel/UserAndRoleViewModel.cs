using Newtonsoft.Json;
using Reso.Core.Attributes;
using System;
using System.Collections.Generic;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class UserAndRoleViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("UserId")]
        public string? UserId { get; set; }
        [JsonProperty("UserName")]
        public string? UserName { get; set; }
        [JsonProperty("RoleId")]
        public string? RoleId { get; set; }
        [JsonProperty("RoleName")]
        public string? RoleName { get; set; }

        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
