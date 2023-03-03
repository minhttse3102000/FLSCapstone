using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.AdminRequest
{
    [Serializable]
    public class CreateAdminRequest
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Email")]
        public string Email { get; set; }
        [JsonProperty("Dob")]
        public DateTime? Dob { get; set; }
        [JsonProperty("Gender")]
        public int? Gender { get; set; }
        [JsonProperty("Idcard")]
        public string Idcard { get; set; }
        [JsonProperty("Address")]
        public string Address { get; set; }
        [JsonProperty("Phone")]
        public string Phone { get; set; }

    }
}
