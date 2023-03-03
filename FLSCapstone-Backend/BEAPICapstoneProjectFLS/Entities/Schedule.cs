using System;
using System.Collections.Generic;
namespace BEAPICapstoneProjectFLS.Entities
{
    public class Schedule
    {
        public Schedule()
        {
            CourseAssigns = new HashSet<CourseAssign>();
        }

        public string Id { get; set; }
        public int? IsPublic { get; set; }
        public string SemesterId { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreate { get; set; }
        public int Status { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual ICollection<CourseAssign> CourseAssigns { get; set; }
    }
}
