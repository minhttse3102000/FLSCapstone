using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class CourseGroupItem
    {
        [JsonProperty("Id")]
        public string ID { get; set; }
        [JsonProperty("LecturerCourseGroupId")]
        public string LecturerCourseGroupID { get; set; }
        [JsonProperty("CourseId")]
        public string CourseID { get; set; }
        [JsonProperty("PriorityCourse")]
        public int Priority { get; set; }
        [JsonProperty("Status")]
        public int status { get; set; }

        public CourseGroupItem()
        {
        }

        public CourseGroupItem(string courseGroupItemID, string lecturerCourseGroupID, string courseID, int priority, int status)
        {
            ID = courseGroupItemID;
            LecturerCourseGroupID = lecturerCourseGroupID;
            CourseID = courseID;
            Priority = priority;
            this.status = status;
        }
    }
}
