using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class SlotType
    {
        [JsonProperty("Id")]
        public string ID { get; set; }
        [JsonProperty("SlotTypeCode")]
        public string SlotTypeCode { get; set; }
        [JsonProperty("TimeStart")]
        public string TimeStart { get; set; }
        [JsonProperty("TimeEnd")]
        public string TimeEnd { get; set; }
        [JsonProperty("SlotNumber")]
        public int SlotNumber { get; set; }
        [JsonProperty("ConvertDateOfWeek")]
        public string DateOfWeek { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterId { get; set; }
        [JsonProperty("Status")]
        public int status { get; set; }

        public SlotType()
        {
        }

        public SlotType(string Id, string slotTypeCode, string timeStart, string timeEnd, int slotNumber, string dateOfWeek, int status)
        {
            ID = Id;
            SlotTypeCode= slotTypeCode;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            SlotNumber = slotNumber;
            DateOfWeek = dateOfWeek;
            this.status = status;
        }
    }
}
