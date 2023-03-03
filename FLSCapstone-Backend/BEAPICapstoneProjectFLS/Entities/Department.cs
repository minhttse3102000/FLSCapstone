using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class Department
    {
        public Department()
        {
            Subjects = new HashSet<Subject>();
            Users = new HashSet<User>();
        }

        public string Id { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentGroupId { get; set; }
        public int Status { get; set; }

        public virtual DepartmentGroup DepartmentGroup { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
