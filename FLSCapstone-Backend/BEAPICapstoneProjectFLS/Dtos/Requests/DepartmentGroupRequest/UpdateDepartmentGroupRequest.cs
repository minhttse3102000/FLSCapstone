using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest
{
    [Serializable]
    public class UpdateDepartmentGroupRequest
    {
        [JsonProperty("DepartmentGroupName")]
        public string DepartmentGroupName { get; set; }
    }
}
