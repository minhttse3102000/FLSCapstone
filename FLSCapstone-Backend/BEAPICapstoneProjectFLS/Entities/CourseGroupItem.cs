using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class CourseGroupItem
    {
        public string Id { get; set; }
        public string LecturerCourseGroupId { get; set; }
        public string CourseId { get; set; }
        public int? PriorityCourse { get; set; }
        public int Status { get; set; }

        public virtual Course Course { get; set; }
        public virtual LecturerCourseGroup LecturerCourseGroup { get; set; }
    }
}
