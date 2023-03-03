using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BEAPICapstoneProjectFLS.Entities
{
    public partial class FLSCapstoneDatabaseContext : DbContext
    {
        public FLSCapstoneDatabaseContext()
        {
        }

        public FLSCapstoneDatabaseContext(DbContextOptions<FLSCapstoneDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseAssign> CourseAssigns { get; set; }
        public virtual DbSet<CourseGroupItem> CourseGroupItems { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DepartmentGroup> DepartmentGroups { get; set; }
        public virtual DbSet<LecturerCourseGroup> LecturerCourseGroups { get; set; }
        public virtual DbSet<LecturerSlotConfig> LecturerSlotConfigs { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RoomSemester> RoomSemesters { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<SlotType> SlotTypes { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<SubjectOfLecturer> SubjectOfLecturers { get; set; }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAndRole> UserAndRoles { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Data Source=DESKTOP-K0PJHFR\\SQLEXPRESS;Initial Catalog=FLSCapstoneDatabase;Persist Security Info=True;User ID=sa;password=se141129");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

 

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SemesterID");

                entity.Property(e => e.SubjectId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SubjectID");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__Course__Semester__45F365D3");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__Course__SubjectI__46E78A0C");
            });

            modelBuilder.Entity<CourseAssign>(entity =>
            {
                entity.ToTable("CourseAssign");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CourseID");

                entity.Property(e => e.IsAssign).HasColumnName("isAssign");

                entity.Property(e => e.LecturerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LecturerID");

                entity.Property(e => e.ScheduleId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ScheduleID");

                entity.Property(e => e.SlotTypeId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SlotTypeID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseAssigns)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__CourseAss__Cours__47DBAE45");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.CourseAssigns)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__CourseAss__Lectu__48CFD27E");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.CourseAssigns)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK__CourseAss__Sched__49C3F6B7");

                entity.HasOne(d => d.SlotType)
                    .WithMany(p => p.CourseAssigns)
                    .HasForeignKey(d => d.SlotTypeId)
                    .HasConstraintName("FK__CourseAss__SlotT__4AB81AF0");
            });

            modelBuilder.Entity<CourseGroupItem>(entity =>
            {
                entity.ToTable("CourseGroupItem");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.CourseId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CourseID");

                entity.Property(e => e.LecturerCourseGroupId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LecturerCourseGroupID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseGroupItems)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__CourseGro__Cours__4BAC3F29");

                entity.HasOne(d => d.LecturerCourseGroup)
                    .WithMany(p => p.CourseGroupItems)
                    .HasForeignKey(d => d.LecturerCourseGroupId)
                    .HasConstraintName("FK__CourseGro__Lectu__4CA06362");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.DepartmentGroupId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DepartmentGroupID");

                entity.Property(e => e.DepartmentName).HasMaxLength(100);

                entity.HasOne(d => d.DepartmentGroup)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.DepartmentGroupId)
                    .HasConstraintName("FK__Departmen__Depar__4D94879B");
            });

            modelBuilder.Entity<DepartmentGroup>(entity =>
            {
                entity.ToTable("DepartmentGroup");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.DepartmentGroupName).HasMaxLength(100);
            });

            modelBuilder.Entity<LecturerCourseGroup>(entity =>
            {
                entity.ToTable("LecturerCourseGroup");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.GroupName).HasMaxLength(150);

                entity.Property(e => e.LecturerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LecturerID");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SemesterID");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.LecturerCourseGroups)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__LecturerC__Lectu__4E88ABD4");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.LecturerCourseGroups)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__LecturerC__Semes__4F7CD00D");
            });

            modelBuilder.Entity<LecturerSlotConfig>(entity =>
            {
                entity.ToTable("LecturerSlotConfig");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.LecturerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LecturerID");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SemesterID");

                entity.Property(e => e.SlotTypeId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SlotTypeID");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.LecturerSlotConfigs)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__LecturerS__Lectu__5070F446");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.LecturerSlotConfigs)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__LecturerS__Semes__5165187F");

                entity.HasOne(d => d.SlotType)
                    .WithMany(p => p.LecturerSlotConfigs)
                    .HasForeignKey(d => d.SlotTypeId)
                    .HasConstraintName("FK__LecturerS__SlotT__52593CB8");
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.DateRespone).HasColumnType("datetime");

                entity.Property(e => e.DepartmentManagerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DepartmentManagerID");

                entity.Property(e => e.Description).HasMaxLength(350);

                entity.Property(e => e.LecturerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LecturerID");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SemesterID");

                entity.Property(e => e.SubjectId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SubjectID");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.HasOne(d => d.DepartmentManager)
                    .WithMany(p => p.RequestDepartmentManagers)
                    .HasForeignKey(d => d.DepartmentManagerId)
                    .HasConstraintName("FK__Request__Departm__534D60F1");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.RequestLecturers)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__Request__Lecture__5441852A");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__Request__Semeste__151B244E");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__Request__Subject__14270015");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<RoomSemester>(entity =>
            {
                entity.ToTable("RoomSemester");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.RoomTypeId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("RoomTypeID");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SemesterID");

                entity.HasOne(d => d.RoomType)
                    .WithMany(p => p.RoomSemesters)
                    .HasForeignKey(d => d.RoomTypeId)
                    .HasConstraintName("FK__RoomSemes__RoomT__5535A963");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.RoomSemesters)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__RoomSemes__Semes__5629CD9C");
            });

            modelBuilder.Entity<RoomType>(entity =>
            {
                entity.ToTable("RoomType");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.RoomTypeName).HasMaxLength(100);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedule");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SemesterID");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__Schedule__Semest__571DF1D5");

                entity.Property(e => e.Description).HasMaxLength(1500);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.DateEnd).HasColumnType("date");

                entity.Property(e => e.DateStart).HasColumnType("date");

                entity.Property(e => e.Term).HasMaxLength(30);
            });

            modelBuilder.Entity<SlotType>(entity =>
            {
                entity.ToTable("SlotType");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SemesterID");

                entity.Property(e => e.SlotTypeCode).HasMaxLength(30).IsUnicode(false);

                entity.Property(e => e.TimeEnd).HasColumnType("time(0)");

                entity.Property(e => e.TimeStart).HasColumnType("time(0)");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.SlotTypes)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__SlotType__Semest__5812160E");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.DepartmentId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DepartmentID");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.SubjectName).HasMaxLength(100);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__Subject__Departm__59063A47");
            });

            modelBuilder.Entity<SubjectOfLecturer>(entity =>
            {
                entity.ToTable("SubjectOfLecturer");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.DepartmentManagerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DepartmentManagerID");

                entity.Property(e => e.IsEnable).HasColumnName("isEnable");

                entity.Property(e => e.LecturerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LecturerID");

                entity.Property(e => e.SemesterId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SemesterID");

                entity.Property(e => e.SubjectId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SubjectID");

                entity.HasOne(d => d.DepartmentManager)
                    .WithMany(p => p.SubjectOfLecturerDepartmentManagers)
                    .HasForeignKey(d => d.DepartmentManagerId)
                    .HasConstraintName("FK__SubjectOf__Depar__59FA5E80");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.SubjectOfLecturerLecturers)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK__SubjectOf__Lectu__5AEE82B9");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.SubjectOfLecturers)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__SubjectOf__Semes__5BE2A6F2");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectOfLecturers)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__SubjectOf__Subje__5CD6CB2B");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UQ__User__A9D1053452FD6B52")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.DepartmentId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("DepartmentID");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("DOB");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idcard)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("IDCard")
                    .IsFixedLength(true);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__User__Department__5DCAEF64");
            });

            modelBuilder.Entity<UserAndRole>(entity =>
            {
                entity.ToTable("UserAndRole");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.RoleId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("RoleID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserAndRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__UserAndRo__RoleI__5EBF139D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAndRoles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserAndRo__UserI__5FB337D6");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");

                entity.Property(e => e.Id)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
