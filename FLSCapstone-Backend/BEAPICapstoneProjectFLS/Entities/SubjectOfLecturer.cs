using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class SubjectOfLecturer
    {
        public string Id { get; set; }
        public string DepartmentManagerId { get; set; }
        public string SemesterId { get; set; }
        public string SubjectId { get; set; }
        public string LecturerId { get; set; }
        public int? FavoritePoint { get; set; }
        public int? FeedbackPoint { get; set; }
        public int? MaxCourseSubject { get; set; }
        public int? IsEnable { get; set; }
        public int Status { get; set; }

        public virtual User DepartmentManager { get; set; }
        public virtual User Lecturer { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
