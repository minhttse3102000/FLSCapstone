using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class LecturerSlotConfig
    {
        public string Id { get; set; }
        public string SlotTypeId { get; set; }
        public string LecturerId { get; set; }
        public string SemesterId { get; set; }
        public int? PreferenceLevel { get; set; }
        public int? IsEnable { get; set; }
        public int Status { get; set; }

        public virtual User Lecturer { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual SlotType SlotType { get; set; }
    }
}
