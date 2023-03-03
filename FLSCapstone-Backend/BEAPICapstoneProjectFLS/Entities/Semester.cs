using System;
using System.Collections.Generic;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class Semester
    {
        public Semester()
        {
            Courses = new HashSet<Course>();
            LecturerCourseGroups = new HashSet<LecturerCourseGroup>();
            LecturerSlotConfigs = new HashSet<LecturerSlotConfig>();
            Requests = new HashSet<Request>();
            RoomSemesters = new HashSet<RoomSemester>();
            Schedules = new HashSet<Schedule>();
            SlotTypes = new HashSet<SlotType>();
            SubjectOfLecturers = new HashSet<SubjectOfLecturer>();
        }

        public string Id { get; set; }
        public string Term { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int? State { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<LecturerCourseGroup> LecturerCourseGroups { get; set; }
        public virtual ICollection<LecturerSlotConfig> LecturerSlotConfigs { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<RoomSemester> RoomSemesters { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<SlotType> SlotTypes { get; set; }
        public virtual ICollection<SubjectOfLecturer> SubjectOfLecturers { get; set; }
    }
}
