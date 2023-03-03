using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class Request
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateRespone { get; set; }
        public string LecturerId { get; set; }
        public string DepartmentManagerId { get; set; }
        public int Status { get; set; }
        public string SubjectId { get; set; }
        public string SemesterId { get; set; }
        public int? ResponseState { get; set; }

        public virtual User DepartmentManager { get; set; }
        public virtual User Lecturer { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
