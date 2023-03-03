using Newtonsoft.Json;
using Reso.Core.Attributes;
using System;
using System.Collections.Generic;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class RoleViewModel
    {
        [JsonProperty("Id")]
        public string? Id { get; set; }
        [JsonProperty("RoleName")]
        public string? RoleName { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
