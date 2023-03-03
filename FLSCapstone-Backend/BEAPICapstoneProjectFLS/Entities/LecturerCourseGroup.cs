using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class LecturerCourseGroup
    {
        public LecturerCourseGroup()
        {
            CourseGroupItems = new HashSet<CourseGroupItem>();
        }

        public string Id { get; set; }
        public string LecturerId { get; set; }
        public string SemesterId { get; set; }
        public string GroupName { get; set; }
        public int? MinCourse { get; set; }
        public int? MaxCourse { get; set; }
        public int Status { get; set; }

        public virtual User Lecturer { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<CourseGroupItem> CourseGroupItems { get; set; }
    }
}
