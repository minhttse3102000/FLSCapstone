using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.UserAndRoleRequest
{
    [Serializable]
    public class UpdateUserAndRoleRequest
    {
        [JsonProperty("UserId")]
        public string? UserId { get; set; }
        [JsonProperty("RoleId")]
        public string? RoleId { get; set; }
    }
}
