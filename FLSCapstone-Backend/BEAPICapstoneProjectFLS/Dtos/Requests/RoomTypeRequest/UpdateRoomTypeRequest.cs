using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.Requests.RoomTypeRequest
{
    [Serializable]
    public class UpdateRoomTypeRequest
    {
        [JsonProperty("RoomTypeName")]
        public string RoomTypeName { get; set; }
        [JsonProperty("Capacity")]
        public int? Capacity { get; set; }
    }
}
