using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class RoomSemester
    {
        [JsonProperty("Id")]
        public string ID { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("RoomTypeId")]
        public string RoomTypeId { get; set; }
        [JsonProperty("Quantity")]
        public int Quantity { get; set; }
        [JsonProperty("Status")]
        public int status { get; set; }
    }
}
