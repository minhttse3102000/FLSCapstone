using System;
using System.Collections.Generic;
namespace BEAPICapstoneProjectFLS.Entities
{
    public class User
    {
        public User()
        {
            CourseAssigns = new HashSet<CourseAssign>();
            LecturerCourseGroups = new HashSet<LecturerCourseGroup>();
            LecturerSlotConfigs = new HashSet<LecturerSlotConfig>();
            RefreshTokens = new HashSet<RefreshToken>();
            RequestDepartmentManagers = new HashSet<Request>();
            RequestLecturers = new HashSet<Request>();
            SubjectOfLecturerDepartmentManagers = new HashSet<SubjectOfLecturer>();
            SubjectOfLecturerLecturers = new HashSet<SubjectOfLecturer>();
            UserAndRoles = new HashSet<UserAndRole>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public int? Gender { get; set; }
        public string Idcard { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int? PriorityLecturer { get; set; }
        public int? IsFullTime { get; set; }
        public string DepartmentId { get; set; }
        public int Status { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<CourseAssign> CourseAssigns { get; set; }
        public virtual ICollection<LecturerCourseGroup> LecturerCourseGroups { get; set; }
        public virtual ICollection<LecturerSlotConfig> LecturerSlotConfigs { get; set; }
        public virtual ICollection<Request> RequestDepartmentManagers { get; set; }
        public virtual ICollection<Request> RequestLecturers { get; set; }
        public virtual ICollection<SubjectOfLecturer> SubjectOfLecturerDepartmentManagers { get; set; }
        public virtual ICollection<SubjectOfLecturer> SubjectOfLecturerLecturers { get; set; }
        public virtual ICollection<UserAndRole> UserAndRoles { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
