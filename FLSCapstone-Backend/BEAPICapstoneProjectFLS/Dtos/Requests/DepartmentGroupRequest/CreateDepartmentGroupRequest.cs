using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest
{
    [Serializable]
    public class CreateDepartmentGroupRequest
    {
        [JsonProperty("DepartmentGroupName")]
        public string DepartmentGroupName { get; set; }
    }
}
