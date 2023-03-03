using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class CourseAssign
    {
        public string Id { get; set; }
        public string LecturerId { get; set; }
        public string CourseId { get; set; }
        public string SlotTypeId { get; set; }
        public string ScheduleId { get; set; }
        public int? IsAssign { get; set; }
        public int Status { get; set; }

        public virtual Course Course { get; set; }
        public virtual User Lecturer { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual SlotType SlotType { get; set; }
    }
}
