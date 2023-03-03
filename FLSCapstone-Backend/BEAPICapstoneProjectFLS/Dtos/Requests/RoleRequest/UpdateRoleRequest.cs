using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.RoleRequest
{
    [Serializable]
    public class UpdateRoleRequest
    {
        [JsonProperty("RoleName")]
        public string RoleName { get; set; }
    }
}
