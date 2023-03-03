using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class LecturerSlotConfig
    {
        [JsonProperty("Id")]
        public string ID { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterID { get; set; }
        [JsonProperty("LecturerId")]
        public string LecturerID { get; set; }
        [JsonProperty("SlotTypeId")]
        public string SlotTypeID { get; set; }
        [JsonProperty("PreferenceLevel")]
        public int PreferenceLevel { get; set; }
        [JsonProperty("IsEnable")]
        public int IsEnable { get; set; }
        [JsonProperty("Status")]
        public int status { get; set; }

        public LecturerSlotConfig()
        {
        }

        public LecturerSlotConfig(string Id, string semesterID, string lecturerID, string slotTypeID, int preferenceLevel, int isEnable, int status)
        {
            ID = Id;
            SemesterID = semesterID;
            LecturerID = lecturerID;
            SlotTypeID = slotTypeID;
            PreferenceLevel = preferenceLevel;
            IsEnable = isEnable;
            this.status = status;
        }
    }
}
