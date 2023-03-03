using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.RoleRequest
{
    [Serializable]
    public class CreateRoleRequest
    {
        [JsonProperty("RoleName")]
        public string RoleName { get; set; }
    }
}
