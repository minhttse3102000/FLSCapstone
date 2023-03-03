using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class CourseAssign
    {
        [JsonProperty("Id")]
        public string ID { get; set; }
        [JsonProperty("LecturerId")]
        public string LecturerID { get; set; }
        [JsonProperty("CourseId")]
        public string CourseID { get; set; }
        [JsonProperty("SlotTypeId")]
        public string SlotTypeID { get; set; }
        [JsonProperty("isAssign")]
        public int isAssign { get; set; }
        [JsonProperty("ScheduleId")]
        public string ScheduleId { get; set; }
        [JsonProperty("Status")]
        public int status { get; set; }
        public double point { get; set; }
        public CourseAssign()
        {
        }

        public CourseAssign(string Id, string lecturerID, string courseID, string slotTypeID, string scheduleId, int status, double point)
        {
            ID = Id;
            LecturerID = lecturerID;
            CourseID = courseID;
            SlotTypeID = slotTypeID;
            isAssign = 0;
            ScheduleId = scheduleId;
            this.status = status;
            this.point = point;
        }

        public override string ToString()
        {
            return "CourseAssign: " + ID+"------"+LecturerID+"-------"+ CourseID+"--------"+ SlotTypeID;
        }
    }
}
