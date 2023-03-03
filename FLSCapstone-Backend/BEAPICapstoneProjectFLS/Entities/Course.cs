using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class Course
    {
        public Course()
        {
            CourseAssigns = new HashSet<CourseAssign>();
            CourseGroupItems = new HashSet<CourseGroupItem>();
        }

        public string Id { get; set; }
        public string SubjectId { get; set; }
        public string SemesterId { get; set; }
        public string Description { get; set; }
        public int? SlotAmount { get; set; }
        public int Status { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<CourseAssign> CourseAssigns { get; set; }
        public virtual ICollection<CourseGroupItem> CourseGroupItems { get; set; }
    }
}
