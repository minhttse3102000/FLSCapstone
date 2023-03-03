using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class RoomType
    {
        public RoomType()
        {
            RoomSemesters = new HashSet<RoomSemester>();
        }

        public string Id { get; set; }
        public string RoomTypeName { get; set; }
        public int? Capacity { get; set; }
        public int Status { get; set; }

        public virtual ICollection<RoomSemester> RoomSemesters { get; set; }
    }
}
