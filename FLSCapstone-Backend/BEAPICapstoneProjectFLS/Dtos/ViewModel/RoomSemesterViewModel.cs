using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class RoomSemesterViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("Term")]
        public string Term { get; set; }
        [JsonProperty("RoomTypeId")]
        public string RoomTypeId { get; set; }
        [JsonProperty("RoomTypeNameT")]
        public string RoomTypeNameT { get; set; }
        [JsonProperty("Quantity")]
        public int? Quantity { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
