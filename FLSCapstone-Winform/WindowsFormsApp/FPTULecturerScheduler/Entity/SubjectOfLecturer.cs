using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler.Entity
{
    public class SubjectOfLecturer
    {
        [JsonProperty("Id")]
        public string ID { get; set; }
        [JsonProperty("SemesterId")]
        public string SemesterID { get; set; }
        [JsonProperty("SubjectId")]
        public string SubjectID { get; set; }
        [JsonProperty("LecturerId")]
        public string LecturerID { get; set; }
        [JsonProperty("FavoritePoint")]
        public int FavoritePoint { get; set; }
        [JsonProperty("FeedbackPoint")]
        public int FeedbackPoint { get; set; }
        [JsonProperty("MaxCourseSubject")]
        public int MaxCourseSubject { get; set; }
        [JsonProperty("isEnable")]
        public int isEnable { get; set; }   
        [JsonProperty("Status")]
        public int status { get; set; }

        public SubjectOfLecturer()
        {
        }

        public SubjectOfLecturer(string Id, string semesterID, string subjectID, string lecturerID, int favoritePoint, int feedbackPoint,int maxCourseSubject, int status)
        {
            ID = Id;
            SemesterID = semesterID;
            SubjectID = subjectID;
            LecturerID = lecturerID;
            FavoritePoint = favoritePoint;
            FeedbackPoint = feedbackPoint;
            MaxCourseSubject = maxCourseSubject;
            this.status = status;
        }


    }
}
