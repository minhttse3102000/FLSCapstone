using Reso.Core.Attributes;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class DepartmentGroupViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("DepartmentGroupName")]
        public string DepartmentGroupName { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }

        [JsonProperty("DepartmentIds")]
        [Contain]
        public List<string> DepartmentIds { get; set; }
    }
}
