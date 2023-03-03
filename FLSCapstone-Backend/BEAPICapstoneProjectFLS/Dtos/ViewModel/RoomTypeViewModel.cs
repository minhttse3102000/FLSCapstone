using System;
using Newtonsoft.Json;
namespace BEAPICapstoneProjectFLS.ViewModel
{
    [Serializable]
    public class RoomTypeViewModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("RoomTypeName")]
        public string RoomTypeName { get; set; }
        [JsonProperty("Capacity")]
        public int? Capacity { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
