using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class DepartmentGroup
    {
        public DepartmentGroup()
        {
            Departments = new HashSet<Department>();
        }

        public string Id { get; set; }
        public string DepartmentGroupName { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
