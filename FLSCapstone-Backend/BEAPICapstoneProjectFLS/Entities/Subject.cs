using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class Subject
    {
        public Subject()
        {
            Courses = new HashSet<Course>();
            Requests = new HashSet<Request>();
            SubjectOfLecturers = new HashSet<SubjectOfLecturer>();
        }

        public string Id { get; set; }
        public string SubjectName { get; set; }
        public string Description { get; set; }
        public string DepartmentId { get; set; }
        public int Status { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<SubjectOfLecturer> SubjectOfLecturers { get; set; }
    }
}
