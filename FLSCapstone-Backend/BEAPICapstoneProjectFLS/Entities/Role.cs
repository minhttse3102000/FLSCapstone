using System;
using System.Collections.Generic;
namespace BEAPICapstoneProjectFLS.Entities
{
    public class Role
    {
        public Role()
        {
            UserAndRoles = new HashSet<UserAndRole>();
        }

        public string Id { get; set; }
        public string RoleName { get; set; }
        public int Status { get; set; }

        public virtual ICollection<UserAndRole> UserAndRoles { get; set; }
    }
}
