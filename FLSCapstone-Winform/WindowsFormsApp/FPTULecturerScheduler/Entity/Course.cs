using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class Course
    {
        [JsonProperty("Id")]
        public string ID { get; set; }
        [JsonProperty("SubjectId")]
        public string SubjectID { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterID { get; set; }
        [JsonProperty("SlotTypeId")]
        public string SlotTypeID { get; set; }
        [JsonProperty("Description")]
        public string Description { get; set; }
        [JsonProperty("SlotAmount")]
        public int SlotAmount { get; set; }
        [JsonProperty("Status")]
        public int status { get; set; }
        public Course()
        {
        }

        public Course(string Id, string subjectID, string semesterID, string slotTypeID, string description, int slotAmount, int status)
        {
            ID = Id;
            SubjectID = subjectID;
            SemesterID = semesterID;
            SlotTypeID = slotTypeID;
            Description = description;
            SlotAmount = slotAmount;
            this.status = status;
        }


        public override string ToString()
        {
            return ID+"-------"+SubjectID+"--------------"+ SemesterID + "--------------" + SlotTypeID + "--------------" + Description + "--------------" + SlotAmount;
        }
    }
}
