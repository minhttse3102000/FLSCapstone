using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.RoomSemesterRequest
{
    [Serializable]
    public class CreateRoomSemesterRequest
    {
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("RoomTypeId")]
        public string RoomTypeId { get; set; }
        [JsonProperty("Quantity")]
        public int? Quantity { get; set; }
    }
}
