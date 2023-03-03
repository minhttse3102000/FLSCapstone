using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class SlotType
    {
        public SlotType()
        {
            CourseAssigns = new HashSet<CourseAssign>();
            LecturerSlotConfigs = new HashSet<LecturerSlotConfig>();
        }

        public SlotType(string id, string slotTypeCode, TimeSpan timeStart, TimeSpan timeEnd, int slotNumber, int dateOfWeek, string semesterId, int status, Semester semester)
        {
            Id = id;
            SlotTypeCode = slotTypeCode;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            SlotNumber = slotNumber;
            DateOfWeek = dateOfWeek;
            SemesterId = semesterId;
            Status = status;
            Semester = semester;
        }

        public string Id { get; set; }
        public string SlotTypeCode { get; set; }
        public TimeSpan? TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public int? SlotNumber { get; set; }
        public int? DateOfWeek { get; set; }
        public string SemesterId { get; set; }
        public int Status { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual ICollection<CourseAssign> CourseAssigns { get; set; }
        public virtual ICollection<LecturerSlotConfig> LecturerSlotConfigs { get; set; }
    }
}
