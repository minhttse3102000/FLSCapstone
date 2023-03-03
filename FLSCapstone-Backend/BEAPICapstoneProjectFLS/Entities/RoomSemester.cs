using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class RoomSemester
    {
        public string Id { get; set; }
        public string SemesterId { get; set; }
        public string RoomTypeId { get; set; }
        public int? Quantity { get; set; }
        public int Status { get; set; }

        public virtual RoomType RoomType { get; set; }
        public virtual Semester Semester { get; set; }
    }
}
