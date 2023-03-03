using FPTULecturerScheduler.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTULecturerScheduler
{

    public class LecturerScheduler
    {
        public static double HESO_PRIORITYCOURSE ;
        
        public static double HESO_FEEDBACKPOINT ;
        //public static double HESO_PRIORITYLECTURER = 0.5;
        public static double HESO_FAVORITEPOINTSUBJECT ;
        public static double HESO_FAVORITEPOINTSLOT;
        public static int MaxCourseSubjectDiff = 3;



        public static int MaxCourseSlot { get; set; }
        public static List<Course> Courses { get; set; }
        public static List<CourseAssign> CourseAssigns { get; set; }
        public static List<CourseGroupItem> CourseGroupItems { get; set; }
        public static List<Department> Departments { get; set; }
        public static List<Lecturer> Lecturers { get; set; }
        public static List<LecturerCourseGroup> LecturerCourseGroups { get; set; }
        public static List<LecturerSlotConfig> LecturerSlotConfigs { get; set; }
        public static Semester Semester { get; set; }
        public static List<SlotType> SlotTypes { get; set; }
        public static List<Subject> Subjects { get; set; }
        public static List<SubjectOfLecturer> SubjectOfLecturers { get; set; }
        public static List<CourseAssign> Run()
        {
            List<CourseAssign> courseAssign = ThuatToanXepLich(Courses, CourseGroupItems, LecturerCourseGroups, Lecturers,
                LecturerSlotConfigs, Semester, SlotTypes, Subjects, SubjectOfLecturers, MaxCourseSlot, CourseAssigns);
            return courseAssign;
        }


        public static LecturerCoursePoint GetCourseLecturer(List<Course> courses, List<Subject> subjects, List<LecturerCourseGroup> lecturerCourseGroups, List<CourseGroupItem> courseGroupItems, List<SubjectOfLecturer> subjectOfLecturers, List<Lecturer> lecturers, string semesterID, List<CourseAssign> courseAssigns, List<CourseAssign> scheduler)
        {
            LecturerCoursePoint lecturerCoursePoint = new LecturerCoursePoint();

            foreach (Course course in Courses)
            {
                int check = 0;
                foreach (CourseAssign courseAssign in courseAssigns)
                {
                    if (course.ID == courseAssign.CourseID)// khong lay course da duoc assign
                    {
                        check = 1;
                        break;
                    }
                }
                foreach (CourseAssign scheduler1 in scheduler)
                {
                    if (course.ID == scheduler1.CourseID)// khong lay course da duoc xep lich
                    {
                        check = 1;
                        break;
                    }
                }
                if (check == 0)
                {
                    List<Lecturer> lecturersPickCourse = GetLecturerPickCourse(courses, course, subjects, lecturerCourseGroups, courseGroupItems, subjectOfLecturers, lecturers, semesterID, scheduler);
                    if (lecturersPickCourse.Count > 0)
                    {                       
                        foreach (Lecturer lecturer in lecturersPickCourse)
                        {
                            double lecturerCoursePoint_temp = TinhDiemLecturer_Course(lecturerCourseGroups, courseGroupItems, subjectOfLecturers, lecturer, semesterID, course);
                            if (lecturerCoursePoint_temp > lecturerCoursePoint.Score)
                            {
                                lecturerCoursePoint.Lecturer = lecturer;
                                lecturerCoursePoint.Course = course;
                                lecturerCoursePoint.Score = lecturerCoursePoint_temp;
                            }
                            else if (lecturerCoursePoint_temp == lecturerCoursePoint.Score) //diem bang nhau thi so sanh diem uu tien cua giang vien
                            {
                                if (lecturer.PriorityLecturer > lecturerCoursePoint.Lecturer.PriorityLecturer)
                                {
                                    lecturerCoursePoint.Lecturer = lecturer;
                                    lecturerCoursePoint.Course = course;
                                    lecturerCoursePoint.Score = lecturerCoursePoint_temp;
                                }
                            }
                        }
                    }
                }
            }
            return lecturerCoursePoint;
        }


        public static List<Course> GetCourseNotAssign(List<Course> courses, List<CourseAssign> scheduler)
        {
            List<Course> CourseNotAssign = new List<Course>();

            foreach (Course course in courses)
            {
                int check = 0;
                foreach (CourseAssign scheduler1 in scheduler)
                {
                    if (course.ID == scheduler1.CourseID)// khong lay course da duoc xep lich
                    {
                        check = 1;
                        break;
                    }
                }
                if (check == 0)
                {
                    CourseNotAssign.Add(course);
                }
            }
            return CourseNotAssign;
        }


        public static Lecturer GetLecturerFewCourse(string semesterID, List<Subject> subjects, List<Course> courses, List<Lecturer> lecturers, List<SubjectOfLecturer> subjectOfLecturers,  List<LecturerCourseGroup> lecturerCourseGroups, List<CourseAssign> scheduler, Course course)
        {
            LecturerCoursePoint lecturerCoursePoint = new LecturerCoursePoint();
            lecturerCoursePoint.Score = -99;
            lecturerCoursePoint.Lecturer = null;

            var subjectOfCourse = from subject in subjects
                                  where subject.status == 1 && subject.ID == course.SubjectID
                                  select new Subject(subject.ID, subject.SubjectName, subject.Description, subject.status, subject.DepartmentID);

            foreach (Lecturer lecturer in lecturers)
            {

                LecturerCoursePoint lecturerCoursePoint1 = new LecturerCoursePoint();
                var minCourseSemester = from lecturerCourseGroup in lecturerCourseGroups
                                        where lecturerCourseGroup.SemesterID == semesterID && lecturerCourseGroup.LecturerID == lecturer.ID
                                        select lecturerCourseGroup.MinCourse;

                var amountCoursePresent = from courseAssign in scheduler
                                          join course1 in courses on courseAssign.CourseID equals course1.ID
                                          where course1.SemesterID == semesterID && courseAssign.LecturerID == lecturer.ID
                                          select courseAssign.ID;

                double diff = minCourseSemester.ElementAtOrDefault(0) - amountCoursePresent.Count();
                lecturerCoursePoint1.Score = diff;
                Boolean checkMaxCourseSubject = CheckMaxCourseSubject(subjectOfLecturers, scheduler, lecturer, subjects, courses, subjectOfCourse.ElementAtOrDefault(0), semesterID);
                Boolean checkMaxCourseSemester = CheckMaxCourseSemester(lecturerCourseGroups, scheduler, lecturer, courses, semesterID);
                Boolean checkMaxCourseWithSlot = CheckMaxCourseWithSlot(SlotTypes, LecturerSlotConfigs, scheduler, lecturer, courses, semesterID);
                if (lecturerCoursePoint1.Score > lecturerCoursePoint.Score & checkMaxCourseSubject==true & checkMaxCourseSemester == true & checkMaxCourseWithSlot == true)
                {
                    lecturerCoursePoint.Lecturer = lecturer;
                    lecturerCoursePoint.Score = lecturerCoursePoint1.Score;
                }
            }
            

            return lecturerCoursePoint.Lecturer;
        }


        public static CourseAssign FillSlot(List<CourseAssign> courseAssignsTemp, List<LecturerSlotConfig> lecturerSlotConfigs, CourseAssign courseAssignFillSlot, List<Course> courses, string semesterID, List<SlotType> slotTypes, int Max)
        {
            slotTypes = slotTypes.OrderBy(slot => slot.DateOfWeek).ThenBy(slot => slot.SlotNumber).ToList();
            List<SlotType> favoriteSlotType = new List<SlotType>();
            List<SlotType> normalSlotType = new List<SlotType>();
            List<SlotType> dislikeSlotType = new List<SlotType>();
            //cac slot giang vien do ua thich PreferenceLevel = 1, is enable = 1
            var favoriteSlot = from lecturerSlotConfig in lecturerSlotConfigs
                               where lecturerSlotConfig.LecturerID == courseAssignFillSlot.LecturerID && lecturerSlotConfig.SemesterID == semesterID 
                               && lecturerSlotConfig.PreferenceLevel == 1 && lecturerSlotConfig.IsEnable == 1
                               select lecturerSlotConfig.SlotTypeID;
            if(favoriteSlot.Count() > 0)
            {
                foreach (var slotID in favoriteSlot)
                {
                    var slotType = slotTypes.Where(slot => slot.ID == slotID);
                    favoriteSlotType.Add((SlotType)slotType.ElementAtOrDefault(0));
                }
            }
            



            //cac slot giang vien do trung tinh PreferenceLevel= 0 , is enable = 1
            var normalSlot = from lecturerSlotConfig in lecturerSlotConfigs
                               where lecturerSlotConfig.LecturerID == courseAssignFillSlot.LecturerID && lecturerSlotConfig.SemesterID == semesterID 
                               && lecturerSlotConfig.PreferenceLevel == 0 && lecturerSlotConfig.IsEnable == 1
                             select lecturerSlotConfig.SlotTypeID;

            if (normalSlot.Count() > 0)
            {
                foreach (var slotID in normalSlot)
                {
                    var slotType = slotTypes.Where(slot => slot.ID == slotID);
                    normalSlotType.Add((SlotType)slotType.ElementAtOrDefault(0));
                }
            }
            



            //cac slot giang vien do khong thich PreferenceLevel =-1, is enable = 1
            var dislikeSlot = from lecturerSlotConfig in lecturerSlotConfigs
                               where lecturerSlotConfig.LecturerID == courseAssignFillSlot.LecturerID && lecturerSlotConfig.SemesterID == semesterID 
                               && lecturerSlotConfig.PreferenceLevel == -1 && lecturerSlotConfig.IsEnable == 1
                               select lecturerSlotConfig.SlotTypeID;

            if (dislikeSlot.Count() > 0)
            {
                foreach (var slotID in dislikeSlot)
                {
                    var slotType = slotTypes.Where(slot => slot.ID == slotID);
                    dislikeSlotType.Add((SlotType)slotType.ElementAtOrDefault(0));
                }
            }

            favoriteSlotType = favoriteSlotType.OrderBy(slot => slot.DateOfWeek).ThenBy(slot => slot.SlotNumber).ToList();
            normalSlotType = normalSlotType.OrderBy(slot => slot.DateOfWeek).ThenBy(slot => slot.SlotNumber).ToList();
            dislikeSlotType = dislikeSlotType.OrderBy(slot => slot.DateOfWeek).ThenBy(slot => slot.SlotNumber).ToList();

            //cac slotType group student da duoc xep
            var conflictSlotCourse = from courseAssign in courseAssignsTemp
                                     join course in courses on courseAssign.CourseID equals course.ID
                                     where course.SemesterID == semesterID && courseAssign.CourseID.Split('_')[1] == courseAssignFillSlot.CourseID.Split('_')[1]
                                     select (courseAssign.CourseID, courseAssign.SlotTypeID);

            //cac slotType giang vien da duoc xep
            var conflictSlotLecturer = from courseAssign in courseAssignsTemp
                                       join course in courses on courseAssign.CourseID equals course.ID
                                       where course.SemesterID == semesterID && courseAssign.LecturerID == courseAssignFillSlot.LecturerID
                                       select (courseAssign.LecturerID, courseAssign.SlotTypeID);

            if (courseAssignsTemp.Count > 0)
            {
                int stop = 0;
                //fill slot ua thich truoc
                foreach (var SlotType in favoriteSlotType)
                {
                    //so luong course 1 slot co the day toi da (so luong phong hoc)
                    var slotMaxCourse = from courseAssign in courseAssignsTemp
                                        where courseAssign.SlotTypeID == SlotType.ID
                                        group courseAssign by courseAssign.SlotTypeID into gr
                                        let count = gr.Count()
                                        select count;


                    int check = 0;
                    //kiem tra course cua 1 group student khong the duoc day cung 1 thoi diem
                    foreach (var conflict in conflictSlotCourse)
                    {
                        if (SlotType.ID == conflict.SlotTypeID)
                        {
                            check = 1;
                            break;
                        }
                    }

                    //kiem tra so luong course 1 slot co the day toi da (so luong phong hoc)
                    if (Convert.ToInt32(slotMaxCourse.ElementAtOrDefault(0)) >= Max)
                    {
                        check = 1;
                    }

                    //kiem tra 1 giang vien chi day 1 course tai 1 thoi diem
                    foreach (var conflict in conflictSlotLecturer)
                    {
                        if (SlotType.ID == conflict.SlotTypeID)
                        {
                            check = 1;
                            break;
                        }
                    }


                    if (check == 0)
                    {
                        courseAssignFillSlot.SlotTypeID = SlotType.ID;// fill slot
                        courseAssignFillSlot.point = courseAssignFillSlot.point + HESO_FAVORITEPOINTSLOT;
                        stop = 1;
                        break;
                    }

                }
                if (stop == 0) //fill cac slot normal 
                {
                    foreach (var SlotType in normalSlotType)
                    {
                        //kiem tra so luong course 1 slot co the day toi da (so luong phong hoc)
                        var slotMaxCourse = from courseAssign in courseAssignsTemp
                                            where courseAssign.SlotTypeID == SlotType.ID
                                            group courseAssign by courseAssign.SlotTypeID into gr
                                            let count = gr.Count()
                                            select count;


                        int check = 0;

                        //kiem tra course cua 1 group student khong the duoc day cung 1 thoi diem
                        foreach (var conflict in conflictSlotCourse)
                        {
                            if (SlotType.ID == conflict.SlotTypeID)
                            {
                                check = 1;
                                break;
                            }
                        }

                        //kiem tra so luong course 1 slot co the day toi da (so luong phong hoc)
                        if (Convert.ToInt32(slotMaxCourse.ElementAtOrDefault(0)) >= Max)
                        {
                            check = 1;
                        }

                        //kiem tra 1 giang vien chi day 1 course tai 1 thoi diem
                        foreach (var conflict in conflictSlotLecturer)
                        {
                            if (SlotType.ID == conflict.SlotTypeID)
                            {
                                check = 1;
                                break;
                            }
                        }


                        if (check == 0)
                        {
                            courseAssignFillSlot.SlotTypeID = SlotType.ID;// fill slot
                            stop = 1;
                            break;
                        }

                    }
                }
                if (stop == 0) //fill cac slot khong thich 
                {
                    foreach (var SlotType in dislikeSlotType)
                    {
                        //kiem tra so luong course 1 slot co the day toi da (so luong phong hoc)
                        var slotMaxCourse = from courseAssign in courseAssignsTemp
                                            where courseAssign.SlotTypeID == SlotType.ID
                                            group courseAssign by courseAssign.SlotTypeID into gr
                                            let count = gr.Count()
                                            select count;


                        int check = 0;

                        //kiem tra course cua 1 group student khong the duoc day cung 1 thoi diem
                        foreach (var conflict in conflictSlotCourse)
                        {
                            if (SlotType.ID == conflict.SlotTypeID)
                            {
                                check = 1;
                                break;
                            }
                        }

                        //kiem tra so luong course 1 slot co the day toi da (so luong phong hoc)
                        if (Convert.ToInt32(slotMaxCourse.ElementAtOrDefault(0)) >= Max)
                        {
                            check = 1;
                        }

                        //kiem tra 1 giang vien chi day 1 course tai 1 thoi diem
                        foreach (var conflict in conflictSlotLecturer)
                        {
                            if (SlotType.ID == conflict.SlotTypeID)
                            {
                                check = 1;
                                break;
                            }
                        }


                        if (check == 0)
                        {
                            courseAssignFillSlot.SlotTypeID = SlotType.ID;// fill slot
                            courseAssignFillSlot.point = courseAssignFillSlot.point - HESO_FAVORITEPOINTSLOT;
                            stop = 1;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (favoriteSlot.Count() > 0)
                {
                    courseAssignFillSlot.SlotTypeID = favoriteSlotType.ElementAtOrDefault(0).ID;
                    courseAssignFillSlot.point = courseAssignFillSlot.point + HESO_FAVORITEPOINTSLOT;
                }
                else
                {
                    courseAssignFillSlot.SlotTypeID = normalSlotType.ElementAtOrDefault(0).ID;
                    courseAssignFillSlot.point = courseAssignFillSlot.point ;
                }              
            }
            return courseAssignFillSlot;
        }

        //static List<CourseAssign> ThuatToanXepLich(List<Course> courseDTOs, List<CourseGroupItem> courseGroupItemDTOs,
        //    List<LecturerCourseGroup> lecturerCourseGroupDTOs, List<Lecturer> lecturerDTOs, List<LecturerSlotConfig> lecturerSlotConfigDTOs, Semester semesterDTO, List<SlotType> slotTypeDTOs,
        //    List<Subject> subjectDTOs, List<SubjectOfLecturer> subjectOfLecturerDTOs, int Max)
        //{
        //    //200 con kien (200 vong for)
        //    //for n giang vien trong department
        //    //Xep lich cho tung giang vien (uu tien part time lecturer)
        //    //for n lan (random n nam trong minCourse < n < maxCourse) (1)
        //    //chon cac mon co diem toi uu nhat voi giang vien
        //    //dong vong lap, co duoc lich cua giang vien (1) va so diem phu hop cua giang vien do voi lich cua giang vien do
        //    //dong vong lap
        //    //co lich cua tat ca cac giang vien va diem phu hop cua moi giang vien voi lich cua minh
        //    //=> tu do tinh ra diem cua schedule nay, cap nhat diem va schedule neu diem lon hon schedule cu 
        //    //tim ra duoc schedule co diem cao nhat     

        //    List<CourseAssign> courseAssignDTOs = new List<CourseAssign>(); //scheduler toi uu nhat
        //    int schedulerPoint_Max = 0; // schedule'point max      

        //    for (int i = 0; i <= 200; i++)//200 con kien
        //    {
        //        List<CourseAssign> courseAssignDTOsTemp = new List<CourseAssign>(); //scheduler temp
        //        int schedulerPoint_Temp = 0; // schedule'point temp

        //        int courseAssignID_Temp = 0;
        //        List<int> listLecturersPoint_Scheduler = new List<int>();

        //        List<Lecturer> sortedListLecturers = lecturerDTOs.OrderByDescending(lecturer => lecturer.PriorityLecturer).ToList();

        //        foreach (var lecturer in sortedListLecturers)
        //        {
        //            int lecturerPoint_Scheduler = 0; //diem phu hop cua giang vien do voi scheduler cua ban than

        //            int courseAmountOfLecturer = RandomCourseAmountOfLecturer(lecturerCourseGroupDTOs, lecturer, semesterDTO);// random so luong course giang vien do se day                   
        //            //int courseAmountOfLecturer = 11;

        //            // lay cac course cao diem , course se khong duoc lay neu da duoc pick HOAC so luong maxCourseSuject dat toi da
        //            List<CourseLecturerPoint> SortlistlecturerCoursePoint = GetHighCoursesOfLecturer(subjectDTOs, lecturerCourseGroupDTOs, courseGroupItemDTOs, subjectOfLecturerDTOs, lecturerSlotConfigDTOs, courseDTOs, courseAssignDTOsTemp, lecturer, semesterDTO.ID);

        //            SortlistlecturerCoursePoint = FillCourseSlot(courseAssignDTOsTemp, lecturerSlotConfigDTOs, SortlistlecturerCoursePoint, lecturer, courseDTOs, semesterDTO.ID, slotTypeDTOs, Max);


        //            if (SortlistlecturerCoursePoint != null)
        //            {
        //                if (SortlistlecturerCoursePoint.Count() < courseAmountOfLecturer)
        //                {
        //                    courseAmountOfLecturer = SortlistlecturerCoursePoint.Count();
        //                }
        //                for (int j = 0; j < courseAmountOfLecturer; j++)
        //                {
        //                    lecturerPoint_Scheduler += SortlistlecturerCoursePoint[j].Score;// tinh diem phu hop cua giang vien do voi scheduler cua ban than
        //                    courseAssignDTOsTemp.Add(new CourseAssign("CA" + courseAssignID_Temp, lecturer.ID, SortlistlecturerCoursePoint[j].Course.ID, SortlistlecturerCoursePoint[j].Course.SlotTypeID, 1));
        //                    courseAssignID_Temp++;

        //                }

        //                listLecturersPoint_Scheduler.Add(lecturerPoint_Scheduler);
        //            }



        //        }
        //        if (listLecturersPoint_Scheduler.Count() > 0)
        //        {
        //            //tinh scheduler-department point, cap nhat scheduler moi neu diem cao hon
        //            schedulerPoint_Temp = listLecturersPoint_Scheduler.Sum();
        //            if (schedulerPoint_Max < schedulerPoint_Temp)
        //            {
        //                courseAssignDTOs = courseAssignDTOsTemp;
        //                schedulerPoint_Max = schedulerPoint_Temp;
        //            }
        //        }


        //    }
        //    Console.WriteLine(schedulerPoint_Max);
        //    return courseAssignDTOs;
        //}

        public static List<CourseAssign> ThuatToanXepLich(List<Course> courses, List<CourseGroupItem> courseGroupItems,
            List<LecturerCourseGroup> lecturerCourseGroups, List<Lecturer> lecturers, List<LecturerSlotConfig> lecturerSlotConfigs, Semester semester, List<SlotType> slotTypes,
            List<Subject> subjects, List<SubjectOfLecturer> subjectOfLecturers, int Max, List<CourseAssign> courseAssigns)
        {
            int schedulerPoint_Max = 0;

            List<CourseAssign> scheduler = new List<CourseAssign>(); //add cac course da duoc assign vao scheduler 
            foreach(var courseAssign in courseAssigns)
            {

                courseAssign.point = HESO_PRIORITYCOURSE + HESO_FEEDBACKPOINT + HESO_FAVORITEPOINTSUBJECT + HESO_FAVORITEPOINTSLOT;
                scheduler.Add(courseAssign);
            }
     
            int schedulerPoint_Temp = 0;
            int courseAssignID = 0;


            for (int i=0; i<courses.Count(); i++)
            {
                LecturerCoursePoint lecturerCoursePoint = GetCourseLecturer(courses, subjects, lecturerCourseGroups, courseGroupItems, subjectOfLecturers, lecturers, semester.ID, courseAssigns, scheduler);
                if (lecturerCoursePoint.Score > 0)
                {
                    CourseAssign courseAssign = new CourseAssign("CA" + courseAssignID++, lecturerCoursePoint.Lecturer.ID, lecturerCoursePoint.Course.ID, "","", 1, lecturerCoursePoint.Score);
                    //fill slot 
                    courseAssign = FillSlot(scheduler, lecturerSlotConfigs, courseAssign, courses, semester.ID, slotTypes, Max);
                    scheduler.Add(courseAssign);

                }
                else
                {
                    break;
                }
            }

            List<CourseAssign> scheduler1 = new List<CourseAssign>();
            foreach (CourseAssign courseAssign in scheduler)
            {
                if (courseAssign.SlotTypeID != "")
                {
                    scheduler1.Add(courseAssign);
                }
            }


            // fill cac course con lai vao cac lecturer it course
            List<Course> CourseNotAssign = GetCourseNotAssign(courses, scheduler1);
            if(CourseNotAssign.Count > 0)
            {
                foreach (Course course in CourseNotAssign)
                {                   
                    Lecturer LecturerFewCourse = GetLecturerFewCourse(semester.ID, subjects, courses, lecturers, subjectOfLecturers, lecturerCourseGroups, scheduler1, course);
                    if (LecturerFewCourse != null)
                    {
                        double lecturerCoursePoint = ((3 / 5) * HESO_FEEDBACKPOINT + (3 / 5) * HESO_FAVORITEPOINTSUBJECT);
                        CourseAssign courseAssign = new CourseAssign("CA" + courseAssignID++, LecturerFewCourse.ID, course.ID, "", "", 1, lecturerCoursePoint);
                        //fill slot 
                        courseAssign = FillSlot(scheduler1, lecturerSlotConfigs, courseAssign, courses, semester.ID, slotTypes, Max);
                        scheduler1.Add(courseAssign);
                    }
                }
            }
            



            return scheduler1;
        }

        public static List<Lecturer> GetLecturerPickCourse(List<Course> courses, Course course, List<Subject> subjects, List<LecturerCourseGroup> lecturerCourseGroups, List<CourseGroupItem> courseGroupItems, List<SubjectOfLecturer> subjectOfLecturers, List<Lecturer> lecturers, string semesterID, List<CourseAssign> courseAssignsTemp)// lay  giang vien co the pick course
        {
            //danh sach cac lecturer co the pick course
            List<Lecturer> lecturersPickCourse = new List<Lecturer>();

            //lay subject cua course
            var subjectOfCourse = from subject in subjects                         
                          where subject.status==1 && subject.ID == course.SubjectID 
                          select new Subject(subject.ID, subject.SubjectName, subject.Description, subject.status, subject.DepartmentID);


            //cac lecturer co the day course trong department, neu isEnable subjecLecturer = 0 thi khong duoc day
            var departmentLecturers = from lecturer in lecturers
                                      join subjectLecturer in subjectOfLecturers on lecturer.ID equals subjectLecturer.LecturerID
                                      where lecturer.status == 1 && subjectLecturer.LecturerID == lecturer.ID && subjectLecturer.SubjectID == subjectOfCourse.ElementAtOrDefault(0).ID
                                          && subjectLecturer.isEnable == 1
                                      select new Lecturer(lecturer.ID, lecturer.LecturerName, lecturer.Email, lecturer.DOB, lecturer.Gender, lecturer.IDCard, lecturer.Address, lecturer.Phone, lecturer.status, lecturer.PriorityLecturer, lecturer.IsFullTime, lecturer.DepartmentID);

            //cac lecturer co the day course trong danh sach uu tien 
            var priorityLecturers = from lecturer in lecturers
                                    join lecturerCourseGroup in lecturerCourseGroups on lecturer.ID equals lecturerCourseGroup.LecturerID
                                    join courseGroupItem in courseGroupItems on lecturerCourseGroup.ID equals courseGroupItem.LecturerCourseGroupID
                                    where courseGroupItem.CourseID == course.ID
                                    select new Lecturer(lecturer.ID, lecturer.LecturerName, lecturer.Email, lecturer.DOB, lecturer.Gender, lecturer.IDCard, lecturer.Address, lecturer.Phone, lecturer.status, lecturer.PriorityLecturer, lecturer.IsFullTime, lecturer.DepartmentID);

            //them lecturer vao danh sach
            foreach (Lecturer departmentLecturer in departmentLecturers)
            {
                Boolean checkMaxCourseSubject = CheckMaxCourseSubject(subjectOfLecturers, courseAssignsTemp, departmentLecturer, subjects, courses, subjectOfCourse.ElementAtOrDefault(0), semesterID);
                Boolean checkMaxCourseSemester = CheckMaxCourseSemester(lecturerCourseGroups, courseAssignsTemp, departmentLecturer, courses, semesterID);
                Boolean checkMaxCourseWithSlot = CheckMaxCourseWithSlot(SlotTypes, LecturerSlotConfigs, courseAssignsTemp, departmentLecturer, courses, semesterID);
                if (checkMaxCourseSubject == true && checkMaxCourseSemester == true && checkMaxCourseWithSlot == true)
                {
                    lecturersPickCourse.Add(departmentLecturer);
                }
            }

            foreach (Lecturer priorityLecturer in priorityLecturers)
            {
                int check = 0;
                foreach (Lecturer lecturer in lecturersPickCourse)
                {
                    if (lecturer.ID == priorityLecturer.ID)
                    {
                        check = 1;
                        break;
                    }
                }
                if (check == 0)
                {
                    Boolean checkMaxCourseSubject = CheckMaxCourseSubject(subjectOfLecturers, courseAssignsTemp, priorityLecturer, subjects, courses, subjectOfCourse.ElementAtOrDefault(0), semesterID);
                    Boolean checkMaxCourseSemester = CheckMaxCourseSemester(lecturerCourseGroups, courseAssignsTemp, priorityLecturer, courses, semesterID);
                    Boolean checkMaxCourseWithSlot = CheckMaxCourseWithSlot(SlotTypes, LecturerSlotConfigs, courseAssignsTemp, priorityLecturer, courses, semesterID);
                    if (checkMaxCourseSubject == true && checkMaxCourseSemester == true && checkMaxCourseWithSlot == true)
                    {
                        lecturersPickCourse.Add(priorityLecturer);
                    }
                }
            }


            return lecturersPickCourse;
        }

        public static List<Lecturer> GetLecturerPickCourse1(List<Course> courses, Course course, List<Subject> subjects, List<LecturerCourseGroup> lecturerCourseGroups, List<CourseGroupItem> courseGroupItems, List<SubjectOfLecturer> subjectOfLecturers, List<Lecturer> lecturers, string semesterID, List<CourseAssign> courseAssignsTemp)// lay  giang vien co the pick course
        {
            //danh sach cac lecturer co the pick course
            List<Lecturer> lecturersPickCourse = new List<Lecturer>();

            //lay subject cua course
            var subjectOfCourse = from subject in subjects
                                  where subject.status == 1 && subject.ID == course.SubjectID
                                  select new Subject(subject.ID, subject.SubjectName, subject.Description, subject.status, subject.DepartmentID);

           
            foreach (Lecturer lecturer in lecturers)
            {
                Boolean checkMaxCourseSubject = CheckMaxCourseSubject(subjectOfLecturers, courseAssignsTemp, lecturer, subjects, courses, subjectOfCourse.ElementAtOrDefault(0), semesterID);
                Boolean checkMaxCourseSemester = CheckMaxCourseSemester(lecturerCourseGroups, courseAssignsTemp, lecturer, courses, semesterID);
                Boolean checkMaxCourseWithSlot = CheckMaxCourseWithSlot(SlotTypes, LecturerSlotConfigs, courseAssignsTemp, lecturer, courses, semesterID);
                if (checkMaxCourseSubject == true && checkMaxCourseSemester == true && checkMaxCourseWithSlot == true)
                {
                    lecturersPickCourse.Add(lecturer);
                }
            }

            return lecturersPickCourse;
        }

        static Boolean CheckMaxCourseSubject(List<SubjectOfLecturer> subjectOfLecturers, List<CourseAssign> CourseAssignsTemp, Lecturer lecturer, List<Subject> subjects, List<Course> courses, Subject subjectOfCourse, string semesterID)//ham nay de check subject da co so luong maxCourse 
        {
            if(CourseAssignsTemp.Count >0)
            {
                //so luong cac course cua subject ma lecturer da duox xep
                var amountCourseSubjectPresent = from courseAssign in CourseAssignsTemp
                            join course in courses on courseAssign.CourseID equals course.ID
                            join subject in subjects on course.SubjectID equals subject.ID
                            where course.SemesterID == semesterID && subject.ID == subjectOfCourse.ID && courseAssign.LecturerID == lecturer.ID
                            select courseAssign.ID;

                //so luong toi da duoc quy dinh cho lecturer
                var maxCourseSubject = from subjectOfLecturer in subjectOfLecturers
                                       where subjectOfLecturer.SemesterID == semesterID && subjectOfLecturer.LecturerID == lecturer.ID && subjectOfLecturer.SubjectID == subjectOfCourse.ID
                                       select subjectOfLecturer.MaxCourseSubject;


                if(amountCourseSubjectPresent.Count() > 0)
                {
                    int count = amountCourseSubjectPresent.Count();
                    int max;
                    if (maxCourseSubject.Count() == 0 )// course cua department khac lecturer
                    {
                        max = MaxCourseSubjectDiff;
                    }
                    else
                    {
                        max = maxCourseSubject.ElementAtOrDefault(0);
                    }

                    if (count >= max)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public static Boolean CheckMaxCourseSemester(List<LecturerCourseGroup> lecturerCourseGroups, List<CourseAssign> CourseAssignsTemp, Lecturer lecturer, List<Course> courses, string semesterID)//ham nay de check subject da co so luong maxCourse 
        {
            if (CourseAssignsTemp.Count > 0)
            {
                //so luong cac coursema lecturer da duox xep
                var amountCoursePresent = from courseAssign in CourseAssignsTemp
                                          join course in courses on courseAssign.CourseID equals course.ID
                                          where course.SemesterID == semesterID && courseAssign.LecturerID == lecturer.ID
                                          select courseAssign.ID;

                //so luong toi da duoc quy dinh cho lecturer
                var maxCourseSemester = from lecturerCourseGroup in lecturerCourseGroups
                                        where lecturerCourseGroup.SemesterID == semesterID && lecturerCourseGroup.LecturerID == lecturer.ID 
                                        select lecturerCourseGroup.MaxCourse;


                if (amountCoursePresent.Count() > 0 )
                {
                    int count = amountCoursePresent.Count();
                    int max= maxCourseSemester.ElementAtOrDefault(0);

                    if (count >= max)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public static Boolean CheckMaxCourseWithSlot(List<SlotType> slotTypes, List<LecturerSlotConfig> LecturerSlotConfigs, List<CourseAssign> CourseAssignsTemp, Lecturer lecturer, List<Course> courses, string semesterID)//ham nay de check subject da co so luong maxCourse 
        {
            if (CourseAssignsTemp.Count > 0)
            {
                //so luong cac coursema lecturer da duox xep
                var amountCoursePresent = from courseAssign in CourseAssignsTemp
                                          join course in courses on courseAssign.CourseID equals course.ID
                                          where course.SemesterID == semesterID && courseAssign.LecturerID == lecturer.ID
                                          select courseAssign.ID;

                //so luong toi da duoc quy dinh cho lecturer theo slot
                var slotIsDisable = from lecturerSlotConfig in LecturerSlotConfigs
                                    where lecturerSlotConfig.SemesterID == semesterID && lecturerSlotConfig.LecturerID == lecturer.ID
                                    && lecturerSlotConfig.IsEnable == 0
                                    select lecturerSlotConfig;


                if (amountCoursePresent.Count() > 0)
                {
                    int count = amountCoursePresent.Count();
                    int maxCourseWithSlot = slotTypes.Count() - slotIsDisable.Count();

                    if (count >= maxCourseWithSlot)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        public static double TinhDiemLecturer_Course(List<LecturerCourseGroup> lecturerCourseGroups, List<CourseGroupItem> courseGroupItems, List<SubjectOfLecturer> subjectOfLecturers, Lecturer lecturer, string semesterID, Course course)// ham dung de tinh diem toi uu cua lecturer voi course
        {
            double priorityCoursePoint = 0;
            double favoritePoint = 0;
            double feedbackPoint = 0;
            //double priorityLecturerPoint = 0;

            var priorityCoursePoint_temp = from courseGroupItem in courseGroupItems
                                join lecturerCourseGroup in lecturerCourseGroups on courseGroupItem.LecturerCourseGroupID equals lecturerCourseGroup.ID
                                where lecturerCourseGroup.SemesterID == semesterID && lecturerCourseGroup.LecturerID == lecturer.ID && courseGroupItem.CourseID== course.ID
                                select courseGroupItem.Priority;
            if(priorityCoursePoint_temp.Count() >0)//course co trong danh sach uu tien
            {
                priorityCoursePoint = Convert.ToDouble(priorityCoursePoint_temp.ElementAtOrDefault(0));             
            }

            //priorityLecturerPoint = lecturer.PriorityLecturer;

            var favoriteFeedbackPoint_temp = from subjectOfLecturerDTO in subjectOfLecturers
                                             where subjectOfLecturerDTO.LecturerID == lecturer.ID
                                             && subjectOfLecturerDTO.SemesterID == semesterID && subjectOfLecturerDTO.SubjectID == course.SubjectID
                                             select (subjectOfLecturerDTO.FavoritePoint, subjectOfLecturerDTO.FeedbackPoint);
            if (favoriteFeedbackPoint_temp.Count() > 0)
            {
                favoritePoint = favoriteFeedbackPoint_temp.ElementAtOrDefault(0).FavoritePoint;
                feedbackPoint = favoriteFeedbackPoint_temp.ElementAtOrDefault(0).FeedbackPoint;
            }
            else 
            {
                favoritePoint = 3;
                feedbackPoint = 3;
            }


            double lecturerCoursePoint = ((priorityCoursePoint/4) * HESO_PRIORITYCOURSE + (feedbackPoint/5) * HESO_FEEDBACKPOINT +(favoritePoint/5) * HESO_FAVORITEPOINTSUBJECT);
            return lecturerCoursePoint;
        }
    }

    public class LecturerCoursePoint
    {
        public Course Course;
        public Lecturer Lecturer { get; set; }
        public double Score { get; set; }
        public LecturerCoursePoint()
        {
            Course = new Course();
            Lecturer = new Lecturer();
            Score = 0;
        }

        public LecturerCoursePoint(Course course, Lecturer lecturer, double score)
        {
            Course = course;
            Lecturer = lecturer;
            Score = score;
        }
    }
}
