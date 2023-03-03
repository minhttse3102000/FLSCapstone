using Newtonsoft.Json;
using Reso.Core.Attributes;
using System;
using System.Collections.Generic;

namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class DepartmentViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("DepartmentName")]
        public string DepartmentName { get; set; }
        [JsonProperty("DepartmentGroupId")]
        public string DepartmentGroupId { get; set; }
        [JsonProperty("DepartmentGroupName")]
        public string DepartmentGroupName { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }

        
    }
}
