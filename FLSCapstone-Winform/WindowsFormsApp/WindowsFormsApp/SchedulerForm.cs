using FPTULecturerScheduler;
using FPTULecturerScheduler.Entity;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.DAO;

namespace WindowsFormsApp
{
    public partial class SchedulerForm : Form
    {

        public List<Semester> Semesters = new List<Semester>();
        
        public static List<Course> Courses = new List<Course>();
        public List<CourseAssign> CourseAssigns = new List<CourseAssign>();
        public List<CourseGroupItem> CourseGroupItems = new List<CourseGroupItem>();
        public List<Department> Departments = new List<Department>();
        public List<Lecturer> Lecturers = new List<Lecturer>();
        public List<LecturerCourseGroup> LecturerCourseGroups = new List<LecturerCourseGroup>();
        public List<LecturerSlotConfig> LecturerSlotConfigs = new List<LecturerSlotConfig>();
        public List<SlotType> SlotTypes = new List<SlotType>();
        public List<Subject> Subjects = new List<Subject>();
        public List<SubjectOfLecturer> SubjectOfLecturers = new List<SubjectOfLecturer>();
        public List<RoomSemester> roomSemester = new List<RoomSemester>();

        public List<Schedule> schedule = new List<Schedule>();
        public static List<CourseAssign> SelectedscheduleItem = new List<CourseAssign>();

        public static List<ScheduleShow> scheduleShows = new List<ScheduleShow>();
        public int ScheduleOrder = 1;

        public ProgressDialog progressDialog;
        public WaitingDialog waitingDialog = new WaitingDialog();
        public RunIfnoForm runInfoDialog = new RunIfnoForm();
        public static string scheduleNameRun ;
        public static string scheduleDescriptionRun;
        public static Boolean scheduleTypeRun ;
        public LoginForm loginForm;

        public Thread thr;

        //public LoginForm loginForm1;

        public SchedulerForm(LoginForm loginForm, Lecturer user)
        {
            //Xuat file Excel
            //List<CourseAssign> courseAssignsTemp = LecturerScheduler.Run();
            //courseAssignsTemp = courseAssignsTemp.OrderBy(courseAssign => courseAssign.LecturerID).ThenBy(courseAssign => courseAssign.SlotTypeID).ToList();
            //btnExport_Click("C:/Users/84393/Desktop/Capstone/WindowsFormsApp/WindowsFormsApp/bin/Debug/CourseAssignTemp.xlsx", courseAssignsTemp);

            InitializeComponent();

            this.loginForm = loginForm;

            string semestersJson = new WebClient().DownloadString("http://20.214.249.72/api/Semester?pageIndex=1&pageSize=100");
            Semesters = JsonConvert.DeserializeObject<List<Semester>>(semestersJson);
            semesterCombobox.DataSource = Semesters;
            semesterCombobox.DisplayMember = "Term";

            semesterCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            subjectCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            lecturerCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            outputSubjectCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            outputLecturerCombobox.DropDownStyle = ComboBoxStyle.DropDownList;
            listBox1.Hide();
            label27.Hide();
            panel3.Hide();

            outputLecturerDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //loginForm1 = new LoginForm(this);
            //loginForm1.Show();
            //this.Hide();
            //loginForm.Close();
        }



        private async void loadDataButton_Click(object sender, EventArgs e)
        {
            //reset output
            tabControl1.SelectTab(0);
            tabControl2.SelectTab(0);
            SelectedscheduleItem = new List<CourseAssign>();
            outputDepartmentDataGridView.Rows.Clear();
            outputSubjectDataGridView.Rows.Clear();
            outputLecturerDataGridView.Rows.Clear();
            outputSlotTypeDataGridView.Rows.Clear();

          
            //////////////////
            await loadDataAsync((Semester)semesterCombobox.SelectedItem);

            //load data vao department gridView
            departmentDataGridView.RowCount = Departments.Count();
            for (int i = 0; i < Departments.Count(); i++)
            {

                var subjectAmount = from subject in Subjects
                                    where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID
                                    group subject by subject.DepartmentID into gr
                                    let count = gr.Count()
                                    select count;

                var lecturerAmount = from lecturer in Lecturers
                                     where lecturer.DepartmentID == Departments.ElementAtOrDefault(i).ID
                                     group lecturer by lecturer.DepartmentID into gr
                                     let count = gr.Count()
                                     select count;

                var courseAmount = from course in Courses
                                   join subject in Subjects on course.SubjectID equals subject.ID
                                   where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID
                                   select course.ID;

                departmentDataGridView.Rows[i].Cells[0].Value = Departments.ElementAtOrDefault(i).ID;
                departmentDataGridView.Rows[i].Cells[1].Value = Departments.ElementAtOrDefault(i).DepartmentName;
                departmentDataGridView.Rows[i].Cells[2].Value = subjectAmount.ElementAtOrDefault(0);
                departmentDataGridView.Rows[i].Cells[3].Value = courseAmount.Count();
                departmentDataGridView.Rows[i].Cells[4].Value = lecturerAmount.ElementAtOrDefault(0);
                
            }

            //load data vao department subject comboBox
            subjectCombobox.DataSource = Departments;
            subjectCombobox.DisplayMember = "DepartmentName";

            //load data vao subject gridView
            var SubjectsShowByDepartment = from subject in Subjects
                                           join department in Departments on subject.DepartmentID equals department.ID
                                           where department.ID == ((Department)subjectCombobox.SelectedItem).ID
                                           select subject;

            subjectDataGridView.RowCount = SubjectsShowByDepartment.Count();
            for (int i = 0; i < SubjectsShowByDepartment.Count(); i++)
            {
                var courseAmount = from course in Courses
                                   where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                   group course by course.SubjectID into gr
                                   let count = gr.Count()
                                   select count;

                var assignCourseAmount = from course in Courses
                                         join courseAssign in CourseAssigns on course.ID equals courseAssign.CourseID
                                         where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                         group course by course.SubjectID into gr
                                         let count = gr.Count()
                                         select count;

                subjectDataGridView.Rows[i].Cells[0].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).ID;
                subjectDataGridView.Rows[i].Cells[1].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).SubjectName;
                subjectDataGridView.Rows[i].Cells[2].Value = courseAmount.ElementAtOrDefault(0);
                subjectDataGridView.Rows[i].Cells[3].Value = assignCourseAmount.ElementAtOrDefault(0);
            }

            //load data vao department lecturer comboBox
            lecturerCombobox.DataSource = Departments;
            lecturerCombobox.DisplayMember = "DepartmentName";

            //load data vao lecturer gridView
            var LecturerShowByDepartment = from lecturer in Lecturers
                                           join department in Departments on lecturer.DepartmentID equals department.ID
                                           where department.ID == ((Department)lecturerCombobox.SelectedItem).ID
                                           select lecturer;

            lecturerDataGridView.RowCount = LecturerShowByDepartment.Count();
            for (int i = 0; i < LecturerShowByDepartment.Count(); i++)
            {
                var semesterConfig = from lecturerCourseGroup in LecturerCourseGroups
                                     where lecturerCourseGroup.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                     select (lecturerCourseGroup.MinCourse, lecturerCourseGroup.MaxCourse);

                var priorityCourseAmount = from courseGroupItem in CourseGroupItems
                                           join lecturerCourseGroup in LecturerCourseGroups on courseGroupItem.LecturerCourseGroupID equals lecturerCourseGroup.ID
                                           where lecturerCourseGroup.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                           group courseGroupItem by LecturerShowByDepartment.ElementAtOrDefault(i).ID into gr
                                           let count = gr.Count()
                                           select count;

                var assignCourseAmount = from courseAssign in CourseAssigns                                          
                                         where courseAssign.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                         group courseAssign by courseAssign.LecturerID into gr
                                         let count = gr.Count()
                                         select count;

                lecturerDataGridView.Rows[i].Cells[0].Value = LecturerShowByDepartment.ElementAtOrDefault(i).ID;
                lecturerDataGridView.Rows[i].Cells[1].Value = LecturerShowByDepartment.ElementAtOrDefault(i).LecturerName;
                lecturerDataGridView.Rows[i].Cells[2].Value = LecturerShowByDepartment.ElementAtOrDefault(i).IsFullTime;
                lecturerDataGridView.Rows[i].Cells[3].Value = semesterConfig.ElementAtOrDefault(0).MinCourse;
                lecturerDataGridView.Rows[i].Cells[4].Value = semesterConfig.ElementAtOrDefault(0).MaxCourse;
                lecturerDataGridView.Rows[i].Cells[5].Value = LecturerShowByDepartment.ElementAtOrDefault(i).PriorityLecturer;
                lecturerDataGridView.Rows[i].Cells[6].Value = priorityCourseAmount.ElementAtOrDefault(0);
                lecturerDataGridView.Rows[i].Cells[7].Value = assignCourseAmount.ElementAtOrDefault(0);
            }


            //load data vao slotType gridView
            slotTypeDataGridView.RowCount = SlotTypes.Count();
            for (int i = 0; i < SlotTypes.Count(); i++)
            {
                var assignAmount = from courseAssign in CourseAssigns
                                   where courseAssign.SlotTypeID == SlotTypes.ElementAtOrDefault(i).ID
                                   group courseAssign by courseAssign.SlotTypeID into gr
                                   let count = gr.Count()
                                   select count;

                slotTypeDataGridView.Rows[i].Cells[0].Value = SlotTypes.ElementAtOrDefault(i).SlotTypeCode;
                slotTypeDataGridView.Rows[i].Cells[1].Value = SlotTypes.ElementAtOrDefault(i).SlotNumber;
                slotTypeDataGridView.Rows[i].Cells[2].Value = SlotTypes.ElementAtOrDefault(i).TimeStart;
                slotTypeDataGridView.Rows[i].Cells[3].Value = SlotTypes.ElementAtOrDefault(i).TimeEnd;
                slotTypeDataGridView.Rows[i].Cells[4].Value = SlotTypes.ElementAtOrDefault(i).DateOfWeek;
                slotTypeDataGridView.Rows[i].Cells[5].Value = roomSemester.ElementAtOrDefault(0).Quantity;
                slotTypeDataGridView.Rows[i].Cells[6].Value = assignAmount.ElementAtOrDefault(0);
            }

            //load from, to label
            string[] temp1 = ((Semester)semesterCombobox.SelectedItem).DateStart.Split('-');
            string[] temp2 = ((Semester)semesterCombobox.SelectedItem).DateEnd.Split('-');
            DateTime dateStart = new DateTime(Convert.ToInt32(temp1[0]), Convert.ToInt32(temp1[1]), Convert.ToInt32(temp1[2]));
            DateTime dateEnd = new DateTime(Convert.ToInt32(temp2[0]), Convert.ToInt32(temp2[1]), Convert.ToInt32(temp2[2]));
            //fromDatelabel.Text = "From date: "+((Semester)semesterCombobox.SelectedItem).DateStart;
            fromDatelabel.Text = "From date: " + String.Format("{0:MM/dd/yyyy}", dateStart);
            toDatelabel.Text = "To date: " + String.Format("{0:MM/dd/yyyy}", dateEnd);

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            if (Courses.Count()==0 || Lecturers.Count() == 0 || Departments.Count() == 0 || SlotTypes.Count() == 0 || Subjects.Count() == 0 )
            {
                MessageBox.Show("Please load data of semester to run", "Message");
            }
            else
            {
                runInfoDialog = new RunIfnoForm();
                runInfoDialog.textBox1.Text = "Schedule " + ScheduleOrder;
                scheduleDescriptionRun = "- Coefficient value of priority course: " + (priorityCourseTrackBar.Value + 1) +"\n"+
                    "- Coefficient value of Department's rating: " + (departmentRatingTrackBar.Value + 1) + "\n" +
                    "- Coefficient value of Lecturer's favorite subject: " + (favoriteSubjectTrackBar.Value + 1) + "\n" +
                    "- Coefficient value of Lecturer's favorite slot: " + (favoriteSlotTrackBar.Value + 1);

                              
                           
                // he so low level
                //foreach (RadioButton radio in lowPanel.Controls)
                //{
                //    if (radio != null)
                //    {
                //        if (radio.Checked)
                //        {
                //            runInfoDialog.textBox2.Text = runInfoDialog.textBox2.Text + radio.Text + ": Low";
                //            break;
                //        }
                //    }
                //}
                            
                runInfoDialog.ShowDialog();

                if (scheduleTypeRun == false)
                {

                }
                else
                {

                    thr = new Thread(new ThreadStart(XepLich));
                    thr.IsBackground = true;
                    thr.Start();


                    //show dialog loading chay thuat toan

                    progressDialog = new ProgressDialog(thr);
                    progressDialog.Show();
                    progressDialog.progressBar1.PerformLayout();

                    
                    
                }

            }
            scheduleTypeRun = false;


        }
       
        void XepLich()
        {
            
           
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            LecturerScheduler.HESO_PRIORITYCOURSE = (priorityCourseTrackBar.Value + 1); 
            LecturerScheduler.HESO_FEEDBACKPOINT = (departmentRatingTrackBar.Value + 1);
            LecturerScheduler.HESO_FAVORITEPOINTSUBJECT = (favoriteSubjectTrackBar.Value + 1);
            LecturerScheduler.HESO_FAVORITEPOINTSLOT = (favoriteSlotTrackBar.Value + 1);

            // he so low level
            //foreach (RadioButton radio in lowPanel.Controls)
            //{
            //    if (radio != null)
            //    {
            //        if (radio.Checked)
            //        {
            //            if (radio.Text == "Priority course level")
            //            {
            //                LecturerScheduler.HESO_PRIORITYCOURSE = 1;
            //            }
            //            else if (radio.Text == "Department's rating level")
            //            {
            //                LecturerScheduler.HESO_FEEDBACKPOINT = 1;
            //            }
            //            else if (radio.Text == "Favorite of lecturer level")
            //            {
            //                LecturerScheduler.HESO_FAVORITEPOINT = 1;
            //            }                    
            //            break;
            //        }
            //    }
            //}


            LecturerScheduler.Courses = Courses;

            LecturerScheduler.Lecturers = Lecturers;

            LecturerScheduler.LecturerSlotConfigs = LecturerSlotConfigs;

            semesterCombobox.Invoke(new MethodInvoker(() => {
                LecturerScheduler.Semester = (Semester)semesterCombobox.SelectedItem;
            }));

            LecturerScheduler.SlotTypes = SlotTypes;

            LecturerScheduler.Subjects = Subjects;

            LecturerScheduler.SubjectOfLecturers = SubjectOfLecturers;

            LecturerScheduler.LecturerCourseGroups = LecturerCourseGroups;

            LecturerScheduler.CourseGroupItems = CourseGroupItems;

            LecturerScheduler.CourseAssigns = CourseAssigns;

            LecturerScheduler.MaxCourseSlot = roomSemester.ElementAtOrDefault(0).Quantity;

            SelectedscheduleItem.Clear();
            SelectedscheduleItem = LecturerScheduler.Run();
            SelectedscheduleItem = SelectedscheduleItem.OrderBy(scheduler => scheduler.LecturerID).ThenBy(scheduler => scheduler.SlotTypeID).ToList();
            //btnExport_Click(@"C:\Users\84393\Desktop\Capstone\FLSCapstone\WindowsFormsApp\WindowsFormsApp\" + "CourseAssignTemp.xlsx", SelectedscheduleItem);

            
                    
            tabControl1.Invoke(new MethodInvoker(() => {
                tabControl1.SelectTab(1);
            }));
            tabControl3.Invoke(new MethodInvoker(() => {
                tabControl3.SelectTab(0);
            }));

            //load data vao output department gridView
            outputDepartmentDataGridView.Invoke(new MethodInvoker(() => {
                outputDepartmentDataGridView.RowCount = Departments.Count();
                for (int i = 0; i < Departments.Count(); i++)
                {
                    var Mon_ThuAmount = from scheduler in SelectedscheduleItem
                                        join slotType in SlotTypes on scheduler.SlotTypeID equals slotType.ID
                                        join course in Courses on scheduler.CourseID equals course.ID
                                        join subject in Subjects on course.SubjectID equals subject.ID
                                        where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID && slotType.DateOfWeek.Contains("Monday")
                                        select scheduler.ID;

                    var Tue_FriAmount = from scheduler in SelectedscheduleItem
                                        join slotType in SlotTypes on scheduler.SlotTypeID equals slotType.ID
                                        join course in Courses on scheduler.CourseID equals course.ID
                                        join subject in Subjects on course.SubjectID equals subject.ID
                                        where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID && slotType.DateOfWeek.Contains("Tuesday")
                                        select scheduler.ID;

                    var Web_SatAmount = from scheduler in SelectedscheduleItem
                                        join slotType in SlotTypes on scheduler.SlotTypeID equals slotType.ID
                                        join course in Courses on scheduler.CourseID equals course.ID
                                        join subject in Subjects on course.SubjectID equals subject.ID
                                        where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID && slotType.DateOfWeek.Contains("Wednesday")
                                        select scheduler.ID;

                    var courseAmount = from course in Courses
                                       join subject in Subjects on course.SubjectID equals subject.ID
                                       where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID
                                       select course.ID;

                    var assignAmount = from scheduler in SelectedscheduleItem
                                       join course in Courses on scheduler.CourseID equals course.ID
                                       join subject in Subjects on course.SubjectID equals subject.ID
                                       where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID
                                       select course.ID;

                    outputDepartmentDataGridView.Rows[i].Cells[0].Value = Departments.ElementAtOrDefault(i).ID;
                    outputDepartmentDataGridView.Rows[i].Cells[1].Value = Departments.ElementAtOrDefault(i).DepartmentName;
                    outputDepartmentDataGridView.Rows[i].Cells[2].Value = courseAmount.Count();
                    outputDepartmentDataGridView.Rows[i].Cells[3].Value = assignAmount.Count();

                    outputDepartmentDataGridView.Rows[i].Cells[4].Value = Mon_ThuAmount.Count();
                    outputDepartmentDataGridView.Rows[i].Cells[5].Value = Tue_FriAmount.Count();
                    outputDepartmentDataGridView.Rows[i].Cells[6].Value = Web_SatAmount.Count();
                    
                }
            }));

            //load du lieu vao output subject combobox
            outputSubjectCombobox.Invoke(new MethodInvoker(() => {
                outputSubjectCombobox.DataSource = Departments;
                outputSubjectCombobox.DisplayMember = "DepartmentName";


                //load data vao output subject gridView           
                outputSubjectDataGridView.Invoke(new MethodInvoker(() =>{
                    var SubjectsShowByDepartment = from subject in Subjects
                                                   join department in Departments on subject.DepartmentID equals department.ID
                                                   where department.ID == ((Department)outputSubjectCombobox.SelectedItem).ID
                                                   select subject;
                    outputSubjectDataGridView.RowCount = SubjectsShowByDepartment.Count();
                    for (int i = 0; i < SubjectsShowByDepartment.Count(); i++)
                    {
                        var courseAmount = from course in Courses
                                           where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                           group course by course.SubjectID into gr
                                           let count = gr.Count()
                                           select count;

                        var assignCourseAmount = from course in Courses
                                                 join scheduler in SelectedscheduleItem on course.ID equals scheduler.CourseID
                                                 where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                                 group course by course.SubjectID into gr
                                                 let count = gr.Count()
                                                 select count;

                        outputSubjectDataGridView.Rows[i].Cells[0].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).ID;
                        outputSubjectDataGridView.Rows[i].Cells[1].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).SubjectName;
                        outputSubjectDataGridView.Rows[i].Cells[2].Value = courseAmount.ElementAtOrDefault(0);
                        outputSubjectDataGridView.Rows[i].Cells[3].Value = assignCourseAmount.ElementAtOrDefault(0);
                    }
                }));
            }));

            //load du lieu vao output lecturer combobox
            outputLecturerCombobox.Invoke(new MethodInvoker(() =>{
                outputLecturerCombobox.DataSource = Departments;
                outputLecturerCombobox.DisplayMember = "DepartmentName";

                //load data vao output lecturer gridView 
                outputLecturerDataGridView.Invoke(new MethodInvoker(() => {
                    var LecturerShowByDepartment = from lecturer in Lecturers
                                                   join department in Departments on lecturer.DepartmentID equals department.ID
                                                   where department.ID == ((Department)outputLecturerCombobox.SelectedItem).ID
                                                   select lecturer;

                    outputLecturerDataGridView.RowCount = LecturerShowByDepartment.Count();
                    for (int i = 0; i < LecturerShowByDepartment.Count(); i++)
                    {
                        var semesterConfig = from lecturerCourseGroup in LecturerCourseGroups
                                             where lecturerCourseGroup.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                             select (lecturerCourseGroup.MinCourse, lecturerCourseGroup.MaxCourse);

                        var assignCourseAmount = from scheduler in SelectedscheduleItem
                                                 where scheduler.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                                 group scheduler by scheduler.LecturerID into gr
                                                 let count = gr.Count()
                                                 select count;

                        outputLecturerDataGridView.Rows[i].Cells[0].Value = LecturerShowByDepartment.ElementAtOrDefault(i).ID;
                        outputLecturerDataGridView.Rows[i].Cells[1].Value = LecturerShowByDepartment.ElementAtOrDefault(i).LecturerName;
                        outputLecturerDataGridView.Rows[i].Cells[2].Value = semesterConfig.ElementAtOrDefault(0).MaxCourse;
                        outputLecturerDataGridView.Rows[i].Cells[3].Value = LecturerShowByDepartment.ElementAtOrDefault(i).PriorityLecturer;
                        outputLecturerDataGridView.Rows[i].Cells[4].Value = assignCourseAmount.ElementAtOrDefault(0);
                    }
                }));
            }));

            //load du lieu vao output slotType gridView 
            outputSlotTypeDataGridView.Invoke(new MethodInvoker(() =>
            {
                outputSlotTypeDataGridView.RowCount = SlotTypes.Count();
                for (int i = 0; i < SlotTypes.Count(); i++)
                {
                    var assignAmount = from scheduler in SelectedscheduleItem
                                       where scheduler.SlotTypeID == SlotTypes.ElementAtOrDefault(i).ID
                                       group scheduler by scheduler.SlotTypeID into gr
                                       let count = gr.Count()
                                       select count;

                    outputSlotTypeDataGridView.Rows[i].Cells[0].Value = SlotTypes.ElementAtOrDefault(i).SlotTypeCode;
                    outputSlotTypeDataGridView.Rows[i].Cells[1].Value = SlotTypes.ElementAtOrDefault(i).SlotNumber;
                    outputSlotTypeDataGridView.Rows[i].Cells[2].Value = SlotTypes.ElementAtOrDefault(i).TimeStart;
                    outputSlotTypeDataGridView.Rows[i].Cells[3].Value = SlotTypes.ElementAtOrDefault(i).TimeEnd;
                    outputSlotTypeDataGridView.Rows[i].Cells[4].Value = SlotTypes.ElementAtOrDefault(i).DateOfWeek;
                    outputSlotTypeDataGridView.Rows[i].Cells[5].Value = roomSemester.ElementAtOrDefault(0).Quantity;
                    outputSlotTypeDataGridView.Rows[i].Cells[6].Value = assignAmount.ElementAtOrDefault(0);
                }
            }));

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            //add schedule vao list schedule show
            //scheduleListView.Invoke(new MethodInvoker(() =>
            //{
            //    Schedule temp = new Schedule();

            //    temp.Id = "Schedule " + ScheduleOrder++;
            //    temp.decription = description;

            //    DateTime now = DateTime.Now;
            //    temp.createTime = now;
            //    temp.SemesterId = ((Semester)semesterCombobox.SelectedItem).ID;
            //    temp.Status = 1;
            //    List<CourseAssign> scheduleItem = new List<CourseAssign>();
            //    foreach (var item1 in SelectedscheduleItem)
            //    {
            //        scheduleItem.Add(item1);
            //    }
            //    ScheduleShow newSchedule = new ScheduleShow(temp, scheduleItem, elapsedTime);
            //    scheduleShows.Add(newSchedule);

            //    ListViewItem item = new ListViewItem();
            //    item.Text = temp.Id;
            //    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = temp.createTime.ToString() });


            //    scheduleListView.Items.Add(item);
            //    scheduleListView.Items[scheduleListView.Items.Count-1].Selected = true;

            //    scheduleNOLabel.Invoke(new MethodInvoker(() =>
            //    {
            //        scheduleNOLabel.Text = temp.Id;
            //    }));

            //    createTimeLabel.Invoke(new MethodInvoker(() =>
            //    {
            //        createTimeLabel.Text = temp.createTime.ToString();
            //    }));

            //    descriptionTextBox.Invoke(new MethodInvoker(() =>
            //    {
            //        descriptionTextBox.Text = temp.decription;
            //    }));

            //    totalCourseLabel.Invoke(new MethodInvoker(() =>
            //    {
            //        totalCourseLabel.Text = SelectedscheduleItem.Count() + "";
            //    }));
            //    runTimeLabel.Invoke(new MethodInvoker(() =>
            //    {
            //        runTimeLabel.Text = elapsedTime + "";
            //    }));


            //    scheduleNOLabel1.Invoke(new MethodInvoker(() =>
            //    {
            //        scheduleNOLabel1.Text = "Schedule number: " + temp.Id;
            //    }));

            //    scheduleNOLabel2.Invoke(new MethodInvoker(() =>
            //    {
            //        scheduleNOLabel2.Text = "Schedule number: " + temp.Id;
            //    }));

            //    scheduleNOLabel3.Invoke(new MethodInvoker(() =>
            //    {
            //        scheduleNOLabel3.Text = "Schedule number: " + temp.Id;
            //    }));

            //    scheduleNOLabel4.Invoke(new MethodInvoker(() =>
            //    {
            //        scheduleNOLabel4.Text = "Schedule number: " + temp.Id;
            //    }));



            //}));


            listBox1.Invoke(new MethodInvoker(() =>
            {
                label27.Show();
                listBox1.Show();
                panel3.Show();
                Schedule temp = new Schedule();

                temp.Id = scheduleNameRun;
                ScheduleOrder++;
                temp.decription = scheduleDescriptionRun;

                DateTime now = DateTime.Now;
                temp.createTime = now;
                temp.SemesterId = ((Semester)semesterCombobox.SelectedItem).ID;
                temp.Status = 1;
                List<CourseAssign> scheduleItem = new List<CourseAssign>();

                //foreach (var item1 in SelectedscheduleItem)
                //{
                //    scheduleItem.Add(item1);
                //}

                for (int i = 0; i < SelectedscheduleItem.Count; i++)
                {
                    scheduleItem.Add(SelectedscheduleItem[i]);
                }

                ////////////////////////// them vao list schedule luu tren winform
                ScheduleShow newSchedule = new ScheduleShow(temp, scheduleItem, elapsedTime, ((priorityCourseTrackBar.Value + 1) + (departmentRatingTrackBar.Value + 1) + (favoriteSubjectTrackBar.Value + 1)) + ((favoriteSlotTrackBar.Value + 1) * (4 / 12)), 0);

                double point = 0;
                foreach (var item in scheduleItem)
                {
                    point = point + item.point;
                }
                double pointPerRow = point / scheduleItem.Count();
                newSchedule.totalAveragePoint = Math.Round((pointPerRow * 10) / (newSchedule.pointPerRowMax), 2);

                scheduleShows.Add(newSchedule);

                ///////////////////////////



                listBox1.Items.Add(temp.Id);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;




                scheduleNOLabel.Invoke(new MethodInvoker(() =>
                {
                    scheduleNOLabel.Text = temp.Id;
                }));

                createTimeLabel.Invoke(new MethodInvoker(() =>
                {
                    createTimeLabel.Text = temp.createTime.ToString();
                }));



                configDescriptionLabel.Invoke(new MethodInvoker(() =>
                {
                    configDescriptionLabel.Text = temp.decription;
                }));

                totalCourseLabel.Invoke(new MethodInvoker(() =>
                {
                    totalCourseLabel.Text = scheduleItem.Count() + "";
                }));
                runTimeLabel.Invoke(new MethodInvoker(() =>
                {
                    runTimeLabel.Text = elapsedTime + "";
                }));


                schedulePointLabel.Invoke(new MethodInvoker(() =>
                {

                    schedulePointLabel.Text = newSchedule.totalAveragePoint + " / 10";                  
                }));


                scheduleNOLabel1.Invoke(new MethodInvoker(() =>
                {
                    scheduleNOLabel1.Text = "Schedule: " + temp.Id;
                }));

                scheduleNOLabel2.Invoke(new MethodInvoker(() =>
                {
                    scheduleNOLabel2.Text = "Schedule: " + temp.Id;
                }));

                scheduleNOLabel3.Invoke(new MethodInvoker(() =>
                {
                    scheduleNOLabel3.Text = "Schedule: " + temp.Id;
                }));

                scheduleNOLabel4.Invoke(new MethodInvoker(() =>
                {
                    scheduleNOLabel4.Text = "Schedule: " + temp.Id;
                }));

            }));



            progressDialog.Hide();

        }
        public static void btnExport_Click(string filePath, List<CourseAssign> listCourseAssign)
        {
            // tạo SaveFileDialog để lưu file excel

            // nếu đường dẫn null hoặc rỗng thì báo không hợp lệ và return hàm
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Đường dẫn báo cáo không hợp lệ");
                return;
            }

            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {


                    // đặt tiêu đề cho file
                    p.Workbook.Properties.Title = "CourseAssignTemp";

                    //Tạo một sheet để làm việc trên đó
                    p.Workbook.Worksheets.Add("sheet1");

                    // lấy sheet vừa add ra để thao tác
                    ExcelWorksheet ws = p.Workbook.Worksheets[0];

                    // đặt tên cho sheet
                    ws.Name = "sheet1";

                    // Tạo danh sách các column header
                    string[] arrColumnHeader = { "ID", "LecturerID", "CourseID", "SlotTypeID" };

                    // lấy ra số lượng cột cần dùng dựa vào số lượng header
                    var countColHeader = arrColumnHeader.Count();

                    // merge các column lại từ column 1 đến số column header
                    // gán giá trị cho cell vừa merge là Thống kê thông tni User Kteam

                    int colIndex = 1;
                    int rowIndex = 1;

                    //tạo các header từ column header đã tạo từ bên trên
                    foreach (var item in arrColumnHeader)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];

                        //căn chỉnh các border

                        //gán giá trị
                        cell.Value = item;

                        colIndex++;
                    }

                    // lấy ra danh sách UserInfo từ ItemSource của DataGrid

                    // với mỗi item trong danh sách sẽ ghi trên 1 dòng
                    foreach (var item in listCourseAssign)
                    {
                        // bắt đầu ghi từ cột 1. Excel bắt đầu từ 1 không phải từ 0
                        colIndex = 1;

                        // rowIndex tương ứng từng dòng dữ liệu
                        rowIndex++;

                        //gán giá trị cho từng cell                      
                        ws.Cells[rowIndex, colIndex++].Value = item.ID;
                        ws.Cells[rowIndex, colIndex++].Value = item.LecturerID;
                        ws.Cells[rowIndex, colIndex++].Value = item.CourseID;
                        ws.Cells[rowIndex, colIndex++].Value = item.SlotTypeID;

                        //ws.Cells[rowIndex, colIndex++].Value = item.Birthday.ToShortDateString();

                    }

                    //Lưu file lại
                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
                Console.WriteLine("Xuất excel thành công!");
            }
            catch (Exception EE)
            {
                Console.WriteLine(EE.Message);
            }
        }

        //load du lieu voi semester truyen vao
        async Task loadDataAsync(Semester semester)
        {
            string semesterId = semester.ID;
            //string filePath = @"C:\Users\84393\Desktop\Capstone\FLSCapstone\WindowsFormsApp\WindowsFormsApp\";
            //Courses = CourseDAO.ReadDataJsonCourse(filePath + "Course.json");
            //Lecturers = LecturerDAO.ReadDataJsonLecturer(filePath + "Lecturer.json");
            //Departments = DepartmentDAO.ReadDataJsonDepartment(filePath + "Department.json");
            //SlotTypes = SlotTypeDAO.ReadDataJsonSlotType(filePath + "SlotType.json");
            //Subjects = SubjectDAO.ReadDataJsonSubject(filePath + "Subject.json");
            //LecturerSlotConfigs = LecturerSlotConfigDAO.ReadDataJsonLecturerSlotConfig(filePath + "LecturerSlotConfig.json");
            //SubjectOfLecturers = SubjectOfLecturerDAO.ReadDataJsonSubjectOfLecturer(filePath + "SubjectOfLecturer.json");
            //LecturerCourseGroups = LecturerCourseGroupDAO.ReadDataJsonLecturerCourseGroup(filePath + "LecturerCourseGroup.json");
            //CourseGroupItems = CourseGroupItemDAO.ReadDataJsonCourseGroupItem(filePath + "CourseGroupItem.json");
            //CourseAssigns = CourseAssignDAO.ReadDataJsonCourseAssign(filePath + "CourseAssign.json");

            //string coursesJson = new WebClient().DownloadString("http://20.214.249.72/api/Course?SemesterId="+ semesterId + "&Status=1&pageIndex=1&pageSize=10000");
            //Courses = JsonConvert.DeserializeObject<List<Course>>(coursesJson);
            Courses = await CourseDAO.GetCourseAsync(semesterId);

            //string lecturersJson = new WebClient().DownloadString("http://20.214.249.72/api/User?RoleIDs=LC&Status=1&pageIndex=1&pageSize=10000");
            //Lecturers = JsonConvert.DeserializeObject<List<Lecturer>>(lecturersJson);
            Lecturers = await LecturerDAO.GetLecturerAsync();

            string departmentsJson = new WebClient().DownloadString("http://20.214.249.72/api/Department?Status=1&pageIndex=1&pageSize=10000");
            Departments = JsonConvert.DeserializeObject<List<Department>>(departmentsJson);

            string slotTypesJson = new WebClient().DownloadString("http://20.214.249.72/api/SlotType?SemesterId=" + semesterId + "&Status=1&pageIndex=1&pageSize=1000");
            SlotTypes = JsonConvert.DeserializeObject<List<SlotType>>(slotTypesJson);

            string subjectsJson = new WebClient().DownloadString("http://20.214.249.72/api/Subject?Status=1&pageIndex=1&pageSize=10000");
            Subjects = JsonConvert.DeserializeObject<List<Subject>>(subjectsJson);

            string lecturerSlotConfigsJson = new WebClient().DownloadString("http://20.214.249.72/api/LecturerSlotConfig?SemesterId=" + semesterId + "&pageIndex=1&pageSize=10000");
            LecturerSlotConfigs = JsonConvert.DeserializeObject<List<LecturerSlotConfig>>(lecturerSlotConfigsJson);

            string subjectOfLecturersJson = new WebClient().DownloadString("http://20.214.249.72/api/SubjectOfLecturer?SemesterId=" + semesterId + "&pageIndex=1&pageSize=10000");
            SubjectOfLecturers = JsonConvert.DeserializeObject<List<SubjectOfLecturer>>(subjectOfLecturersJson);

            string lecturerCourseGroupsJson = new WebClient().DownloadString("http://20.214.249.72/api/LecturerCourseGroup?SemesterId=" + semesterId + "&pageIndex=1&pageSize=10000");
            LecturerCourseGroups = JsonConvert.DeserializeObject<List<LecturerCourseGroup>>(lecturerCourseGroupsJson);

            string courseGroupItemsJson = new WebClient().DownloadString("http://20.214.249.72/api/CourseGroupItem?Status=1&pageIndex=1&pageSize=100000");
            CourseGroupItems = JsonConvert.DeserializeObject<List<CourseGroupItem>>(courseGroupItemsJson);

            string roomSemesterItemsJson = new WebClient().DownloadString("http://20.214.249.72/api/RoomSemester?RoomTypeId=R1&pageIndex=1&pageSize=10");
            roomSemester = JsonConvert.DeserializeObject<List<RoomSemester>>(roomSemesterItemsJson);

            string scheduleJson = new WebClient().DownloadString("http://20.214.249.72/api/Schedule?SemesterId="+ semesterId + "&pageIndex=1&pageSize=100");
            schedule = JsonConvert.DeserializeObject<List<Schedule>>(scheduleJson);

            string CourseAssignsJson = new WebClient().DownloadString("http://20.214.249.72/api/CourseAssign?isAssign=1&pageIndex=1&pageSize=100000");
            List<CourseAssign> AllCourseAssigns = JsonConvert.DeserializeObject<List<CourseAssign>>(CourseAssignsJson);
            var temp = from courseAssign in AllCourseAssigns
                       join course in Courses on courseAssign.CourseID equals course.ID
                       where course.SemesterID == semesterId
                       select courseAssign;
            CourseAssigns.Clear();
            foreach (var item in temp)
            {
                CourseAssigns.Add(item);
            }


            Subjects = Subjects.OrderBy(subject => subject.DepartmentID).ThenBy(subject => subject.ID).ToList();
            Lecturers = Lecturers.OrderBy(lecturer => lecturer.DepartmentID).ThenBy(lecturer => lecturer.ID).ToList();
            SlotTypes = SlotTypes.OrderBy(slot => slot.DateOfWeek).ThenBy(slot => slot.SlotNumber).ToList();
        }

        



        private async void button1_Click_1(object sender, EventArgs e)
        {
            if (SelectedscheduleItem.Count() < 1)
            {
                MessageBox.Show("No infomation schedule to send !", "Message");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("You want to send this schedule to sever ?", "Message", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string Json = new WebClient().DownloadString("http://20.214.249.72/api/CourseAssign?ScheduleId=" + schedule.ElementAtOrDefault(0).Id + "&isAssign=0&pageIndex=1&pageSize=1000");
                    List<CourseAssign> test = JsonConvert.DeserializeObject<List<CourseAssign>>(Json);

                    if (test.Count() > 1)
                    {
                        MessageBox.Show("You can not send more schedule !", "Message");
                    }
                    else
                    {
                        waitingDialog.Show();
                        waitingDialog.progressBar1.PerformLayout();
                        //show dialog loading send data to sever

                        Thread thr = new Thread(new ThreadStart(SendDataToSeverAsync));
                        thr.IsBackground = true;
                        thr.Start();
                    }

                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        async void SendDataToSeverAsync()
        {
            List<CourseAssign> temp = new List<CourseAssign>();
            foreach (var item in SelectedscheduleItem)
            {
                if (item.isAssign == 0)
                {
                    item.ScheduleId = schedule.ElementAtOrDefault(0).Id;
                    temp.Add(item);
                }
            }
            await CourseAssignDAO.CreateListCourseAssignAsync(schedule.ElementAtOrDefault(0).Id, temp);
            waitingDialog.Invoke(new MethodInvoker(() => {
                waitingDialog.Hide();
            }));
            MessageBox.Show("Send schedule successful !", "Message");
        }

        private void departmentCbxSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load lai data vao subject gridView khi click combobox
            var SubjectsShowByDepartment = from subject in Subjects
                                           join department in Departments on subject.DepartmentID equals department.ID
                                           where department.ID == ((Department)subjectCombobox.SelectedItem).ID
                                           select subject;

            subjectDataGridView.RowCount = SubjectsShowByDepartment.Count();
            for (int i = 0; i < SubjectsShowByDepartment.Count(); i++)
            {
                var courseAmount = from course in Courses
                                   where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                   group course by course.SubjectID into gr
                                   let count = gr.Count()
                                   select count;

                var assignCourseAmount = from course in Courses
                                         join courseAssign in CourseAssigns on course.ID equals courseAssign.CourseID
                                         where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                         group course by course.SubjectID into gr
                                         let count = gr.Count()
                                         select count;

                subjectDataGridView.Rows[i].Cells[0].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).ID;
                subjectDataGridView.Rows[i].Cells[1].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).SubjectName;
                subjectDataGridView.Rows[i].Cells[2].Value = courseAmount.ElementAtOrDefault(0);
                subjectDataGridView.Rows[i].Cells[3].Value = assignCourseAmount.ElementAtOrDefault(0);
            }
        }

        private void departmentCbxLecturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load lai data vao lecturer gridView khi click combobox
            var LecturerShowByDepartment = from lecturer in Lecturers
                                           join department in Departments on lecturer.DepartmentID equals department.ID
                                           where department.ID == ((Department)lecturerCombobox.SelectedItem).ID
                                           select lecturer;

            lecturerDataGridView.RowCount = LecturerShowByDepartment.Count();
            for (int i = 0; i < LecturerShowByDepartment.Count(); i++)
            {
                var semesterConfig = from lecturerCourseGroup in LecturerCourseGroups
                                     where lecturerCourseGroup.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                     select (lecturerCourseGroup.MinCourse, lecturerCourseGroup.MaxCourse);

                var priorityCourseAmount = from courseGroupItem in CourseGroupItems
                                           join lecturerCourseGroup in LecturerCourseGroups on courseGroupItem.LecturerCourseGroupID equals lecturerCourseGroup.ID
                                           where lecturerCourseGroup.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                           group courseGroupItem by LecturerShowByDepartment.ElementAtOrDefault(i).ID into gr
                                           let count = gr.Count()
                                           select count;

                var assignCourseAmount = from courseAssign in CourseAssigns
                                         where courseAssign.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                         group courseAssign by courseAssign.LecturerID into gr
                                         let count = gr.Count()
                                         select count;

                lecturerDataGridView.Rows[i].Cells[0].Value = LecturerShowByDepartment.ElementAtOrDefault(i).ID;
                lecturerDataGridView.Rows[i].Cells[1].Value = LecturerShowByDepartment.ElementAtOrDefault(i).LecturerName;
                lecturerDataGridView.Rows[i].Cells[2].Value = LecturerShowByDepartment.ElementAtOrDefault(i).IsFullTime;
                lecturerDataGridView.Rows[i].Cells[3].Value = semesterConfig.ElementAtOrDefault(0).MinCourse;
                lecturerDataGridView.Rows[i].Cells[4].Value = semesterConfig.ElementAtOrDefault(0).MaxCourse;
                lecturerDataGridView.Rows[i].Cells[5].Value = LecturerShowByDepartment.ElementAtOrDefault(i).PriorityLecturer;
                lecturerDataGridView.Rows[i].Cells[6].Value = priorityCourseAmount.ElementAtOrDefault(0);
                lecturerDataGridView.Rows[i].Cells[7].Value = assignCourseAmount.ElementAtOrDefault(0);
            }
        }

        private void outputSubjectCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load lai data vo output subject data gridview khi click combobox
            var SubjectsShowByDepartment = from subject in Subjects
                                           join department in Departments on subject.DepartmentID equals department.ID
                                           where department.ID == ((Department)outputSubjectCombobox.SelectedItem).ID
                                           select subject;
            outputSubjectDataGridView.RowCount = SubjectsShowByDepartment.Count();
            for (int i = 0; i < SubjectsShowByDepartment.Count(); i++)
            {
                var courseAmount = from course in Courses
                                   where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                   group course by course.SubjectID into gr
                                   let count = gr.Count()
                                   select count;

                var assignCourseAmount = from course in Courses
                                         join scheduler in SelectedscheduleItem on course.ID equals scheduler.CourseID
                                         where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                         group course by course.SubjectID into gr
                                         let count = gr.Count()
                                         select count;

                outputSubjectDataGridView.Rows[i].Cells[0].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).ID;
                outputSubjectDataGridView.Rows[i].Cells[1].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).SubjectName;
                outputSubjectDataGridView.Rows[i].Cells[2].Value = courseAmount.ElementAtOrDefault(0);
                outputSubjectDataGridView.Rows[i].Cells[3].Value = assignCourseAmount.ElementAtOrDefault(0);
            }
        }

        private void outputLecturerCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load lai data vo output lecturer data gridview khi click combobox
            var LecturerShowByDepartment = from lecturer in Lecturers
                                           join department in Departments on lecturer.DepartmentID equals department.ID
                                           where department.ID == ((Department)outputLecturerCombobox.SelectedItem).ID
                                           select lecturer;

            outputLecturerDataGridView.RowCount = LecturerShowByDepartment.Count();
            for (int i = 0; i < LecturerShowByDepartment.Count(); i++)
            {
                var semesterConfig = from lecturerCourseGroup in LecturerCourseGroups
                                     where lecturerCourseGroup.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                     select (lecturerCourseGroup.MinCourse, lecturerCourseGroup.MaxCourse);

                var assignCourseAmount = from scheduler in SelectedscheduleItem
                                         where scheduler.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                         group scheduler by scheduler.LecturerID into gr
                                         let count = gr.Count()
                                         select count;

                outputLecturerDataGridView.Rows[i].Cells[0].Value = LecturerShowByDepartment.ElementAtOrDefault(i).ID;
                outputLecturerDataGridView.Rows[i].Cells[1].Value = LecturerShowByDepartment.ElementAtOrDefault(i).LecturerName;
                outputLecturerDataGridView.Rows[i].Cells[2].Value = semesterConfig.ElementAtOrDefault(0).MaxCourse;
                outputLecturerDataGridView.Rows[i].Cells[3].Value = LecturerShowByDepartment.ElementAtOrDefault(i).PriorityLecturer;
                outputLecturerDataGridView.Rows[i].Cells[4].Value = assignCourseAmount.ElementAtOrDefault(0);
            }
        }

        

        

       

        private void outputLecturerDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (outputLecturerDataGridView.RowCount > 1)
            {
                Lecturer lecturerShow = Lecturers.Find(lec => lec.ID == outputLecturerDataGridView.Rows[e.RowIndex].Cells[0].Value);

                List<CourseAssign> scheduleItemShow = SelectedscheduleItem;
                List<SlotType> slotTypesShow = SlotTypes;
                ScheduleDetailDialog scheduleDetailDialog = new ScheduleDetailDialog((Semester)semesterCombobox.SelectedItem, lecturerShow, scheduleItemShow, slotTypesShow, scheduleShows[index]);

                scheduleDetailDialog.ShowDialog();
                //MessageBox.Show(outputLecturerDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "", "Message");
            }

        }



        private void scheduleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void priorityCourseTrackBar_Scroll(object sender, EventArgs e)
        {
            double total = (priorityCourseTrackBar.Value + 1) + (departmentRatingTrackBar.Value + 1) + (favoriteSubjectTrackBar.Value + 1) + (favoriteSlotTrackBar.Value + 1);
            priorityCourseValueLabel.Text = "Coefficient value: " + (priorityCourseTrackBar.Value + 1) + "   (" + Math.Round((priorityCourseTrackBar.Value + 1) / total * 100, 2) + "%)";
            departmentRatingValueLabel.Text = "Coefficient value: " + (departmentRatingTrackBar.Value + 1) + "   (" + Math.Round((departmentRatingTrackBar.Value + 1) / total * 100, 2) + "%)";
            favoriteSubjectValueLabel.Text = "Coefficient value: " + (favoriteSubjectTrackBar.Value + 1) + "   (" + Math.Round((favoriteSubjectTrackBar.Value + 1) / total * 100, 2) + "%)";
            favoriteSlotValueLabel.Text = "Coefficient value: " + (favoriteSlotTrackBar.Value + 1) + "   (" + Math.Round((favoriteSlotTrackBar.Value + 1) / total * 100, 2) + "%)";
        }

        private void departmentRatingTrackBar_Scroll(object sender, EventArgs e)
        {
            double total = (priorityCourseTrackBar.Value + 1) + (departmentRatingTrackBar.Value + 1) + (favoriteSubjectTrackBar.Value + 1) + (favoriteSlotTrackBar.Value + 1);
            priorityCourseValueLabel.Text = "Coefficient value: " + (priorityCourseTrackBar.Value + 1) + "   (" + Math.Round((priorityCourseTrackBar.Value + 1) / total * 100, 2) + "%)";
            departmentRatingValueLabel.Text = "Coefficient value: " + (departmentRatingTrackBar.Value + 1) + "   (" + Math.Round((departmentRatingTrackBar.Value + 1) / total * 100, 2) + "%)";
            favoriteSubjectValueLabel.Text = "Coefficient value: " + (favoriteSubjectTrackBar.Value + 1) + "   (" + Math.Round((favoriteSubjectTrackBar.Value + 1) / total * 100, 2) + "%)";
            favoriteSlotValueLabel.Text = "Coefficient value: " + (favoriteSlotTrackBar.Value + 1) + "   (" + Math.Round((favoriteSlotTrackBar.Value + 1) / total * 100, 2) + "%)";
        }

        private void favoriteTrackBar_Scroll(object sender, EventArgs e)
        {
            double total = (priorityCourseTrackBar.Value + 1) + (departmentRatingTrackBar.Value + 1) + (favoriteSubjectTrackBar.Value + 1) + (favoriteSlotTrackBar.Value + 1);
            priorityCourseValueLabel.Text = "Coefficient value: " + (priorityCourseTrackBar.Value + 1) + "   (" + Math.Round((priorityCourseTrackBar.Value + 1) / total * 100, 2) + "%)";
            departmentRatingValueLabel.Text = "Coefficient value: " + (departmentRatingTrackBar.Value + 1) + "   (" + Math.Round((departmentRatingTrackBar.Value + 1) / total * 100, 2) + "%)";
            favoriteSubjectValueLabel.Text = "Coefficient value: " + (favoriteSubjectTrackBar.Value + 1) + "   (" + Math.Round((favoriteSubjectTrackBar.Value + 1) / total * 100, 2) + "%)";
            favoriteSlotValueLabel.Text = "Coefficient value: " + (favoriteSlotTrackBar.Value + 1) + "   (" + Math.Round((favoriteSlotTrackBar.Value + 1) / total * 100, 2) + "%)";
        }

        private void favoriteSlotTrackBar_Scroll(object sender, EventArgs e)
        {
            double total = (priorityCourseTrackBar.Value + 1) + (departmentRatingTrackBar.Value + 1) + (favoriteSubjectTrackBar.Value + 1) + (favoriteSlotTrackBar.Value + 1);
            priorityCourseValueLabel.Text = "Coefficient value: " + (priorityCourseTrackBar.Value + 1) + "   (" + Math.Round((priorityCourseTrackBar.Value + 1) / total * 100, 2) + "%)";
            departmentRatingValueLabel.Text = "Coefficient value: " + (departmentRatingTrackBar.Value + 1) + "   (" + Math.Round((departmentRatingTrackBar.Value + 1) / total * 100, 2) + "%)";
            favoriteSubjectValueLabel.Text = "Coefficient value: " + (favoriteSubjectTrackBar.Value + 1) + "   (" + Math.Round((favoriteSubjectTrackBar.Value + 1) / total * 100, 2) + "%)";
            favoriteSlotValueLabel.Text = "Coefficient value: " + (favoriteSlotTrackBar.Value + 1) + "   (" + Math.Round((favoriteSlotTrackBar.Value + 1) / total * 100, 2) + "%)";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // load du lieu vao info panel
            int index = listBox1.SelectedIndex;
            scheduleNOLabel.Text = scheduleShows[index].Schedule.Id;
            createTimeLabel.Text = scheduleShows[index].Schedule.createTime.ToString();
            configDescriptionLabel.Text = scheduleShows[index].Schedule.decription;
            totalCourseLabel.Text = scheduleShows[index].scheduleItem.Count() + "";
            runTimeLabel.Text = scheduleShows[index].runTime;

            schedulePointLabel.Text = scheduleShows[index].totalAveragePoint + " / 10";

            // load du lieu vao cac gridview
            scheduleNOLabel1.Text = "Schedule: " + scheduleShows[index].Schedule.Id;
            scheduleNOLabel2.Text = "Schedule: " + scheduleShows[index].Schedule.Id;
            scheduleNOLabel3.Text = "Schedule: " + scheduleShows[index].Schedule.Id;
            scheduleNOLabel4.Text = "Schedule: " + scheduleShows[index].Schedule.Id;

            //load data vao output department gridView
            SelectedscheduleItem.Clear();

            foreach (var item in scheduleShows[index].scheduleItem)
            {
                SelectedscheduleItem.Add(item);
            }



            outputDepartmentDataGridView.RowCount = Departments.Count();
            for (int i = 0; i < Departments.Count(); i++)
            {
                var Mon_ThuAmount = from scheduler in SelectedscheduleItem
                                    join slotType in SlotTypes on scheduler.SlotTypeID equals slotType.ID
                                    join course in Courses on scheduler.CourseID equals course.ID
                                    join subject in Subjects on course.SubjectID equals subject.ID
                                    where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID && slotType.DateOfWeek.Contains("Monday")
                                    select scheduler.ID;

                var Tue_FriAmount = from scheduler in SelectedscheduleItem
                                    join slotType in SlotTypes on scheduler.SlotTypeID equals slotType.ID
                                    join course in Courses on scheduler.CourseID equals course.ID
                                    join subject in Subjects on course.SubjectID equals subject.ID
                                    where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID && slotType.DateOfWeek.Contains("Tuesday")
                                    select scheduler.ID;

                var Web_SatAmount = from scheduler in SelectedscheduleItem
                                    join slotType in SlotTypes on scheduler.SlotTypeID equals slotType.ID
                                    join course in Courses on scheduler.CourseID equals course.ID
                                    join subject in Subjects on course.SubjectID equals subject.ID
                                    where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID && slotType.DateOfWeek.Contains("Wednesday")
                                    select scheduler.ID;

                var courseAmount = from course in Courses
                                   join subject in Subjects on course.SubjectID equals subject.ID
                                   where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID
                                   select course.ID;

                var assignAmount = from scheduler in SelectedscheduleItem
                                   join course in Courses on scheduler.CourseID equals course.ID
                                   join subject in Subjects on course.SubjectID equals subject.ID
                                   where subject.DepartmentID == Departments.ElementAtOrDefault(i).ID
                                   select course.ID;

                outputDepartmentDataGridView.Rows[i].Cells[0].Value = Departments.ElementAtOrDefault(i).ID;
                outputDepartmentDataGridView.Rows[i].Cells[1].Value = Departments.ElementAtOrDefault(i).DepartmentName;
                outputDepartmentDataGridView.Rows[i].Cells[2].Value = courseAmount.Count();
                outputDepartmentDataGridView.Rows[i].Cells[3].Value = assignAmount.Count();

                outputDepartmentDataGridView.Rows[i].Cells[4].Value = Mon_ThuAmount.Count();
                outputDepartmentDataGridView.Rows[i].Cells[5].Value = Tue_FriAmount.Count();
                outputDepartmentDataGridView.Rows[i].Cells[6].Value = Web_SatAmount.Count();

            }





            //load data vao output subject gridView           

            var SubjectsShowByDepartment = from subject in Subjects
                                           join department in Departments on subject.DepartmentID equals department.ID
                                           where department.ID == ((Department)outputSubjectCombobox.SelectedItem).ID
                                           select subject;
            outputSubjectDataGridView.RowCount = SubjectsShowByDepartment.Count();
            for (int i = 0; i < SubjectsShowByDepartment.Count(); i++)
            {
                var courseAmount = from course in Courses
                                   where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                   group course by course.SubjectID into gr
                                   let count = gr.Count()
                                   select count;

                var assignCourseAmount = from course in Courses
                                         join scheduler in SelectedscheduleItem on course.ID equals scheduler.CourseID
                                         where course.SubjectID == SubjectsShowByDepartment.ElementAtOrDefault(i).ID
                                         group course by course.SubjectID into gr
                                         let count = gr.Count()
                                         select count;

                outputSubjectDataGridView.Rows[i].Cells[0].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).ID;
                outputSubjectDataGridView.Rows[i].Cells[1].Value = SubjectsShowByDepartment.ElementAtOrDefault(i).SubjectName;
                outputSubjectDataGridView.Rows[i].Cells[2].Value = courseAmount.ElementAtOrDefault(0);
                outputSubjectDataGridView.Rows[i].Cells[3].Value = assignCourseAmount.ElementAtOrDefault(0);
            }





            //load data vao output lecturer gridView 

            var LecturerShowByDepartment = from lecturer in Lecturers
                                           join department in Departments on lecturer.DepartmentID equals department.ID
                                           where department.ID == ((Department)outputLecturerCombobox.SelectedItem).ID
                                           select lecturer;

            outputLecturerDataGridView.RowCount = LecturerShowByDepartment.Count();
            for (int i = 0; i < LecturerShowByDepartment.Count(); i++)
            {
                var semesterConfig = from lecturerCourseGroup in LecturerCourseGroups
                                     where lecturerCourseGroup.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                     select (lecturerCourseGroup.MinCourse, lecturerCourseGroup.MaxCourse);

                var assignCourseAmount = from scheduler in SelectedscheduleItem
                                         where scheduler.LecturerID == LecturerShowByDepartment.ElementAtOrDefault(i).ID
                                         group scheduler by scheduler.LecturerID into gr
                                         let count = gr.Count()
                                         select count;

                outputLecturerDataGridView.Rows[i].Cells[0].Value = LecturerShowByDepartment.ElementAtOrDefault(i).ID;
                outputLecturerDataGridView.Rows[i].Cells[1].Value = LecturerShowByDepartment.ElementAtOrDefault(i).LecturerName;
                outputLecturerDataGridView.Rows[i].Cells[2].Value = semesterConfig.ElementAtOrDefault(0).MaxCourse;
                outputLecturerDataGridView.Rows[i].Cells[3].Value = LecturerShowByDepartment.ElementAtOrDefault(i).PriorityLecturer;
                outputLecturerDataGridView.Rows[i].Cells[4].Value = assignCourseAmount.ElementAtOrDefault(0);
            }


            //load du lieu vao output slotType gridView 

            outputSlotTypeDataGridView.RowCount = SlotTypes.Count();
            for (int i = 0; i < SlotTypes.Count(); i++)
            {
                var assignAmount = from scheduler in SelectedscheduleItem
                                   where scheduler.SlotTypeID == SlotTypes.ElementAtOrDefault(i).ID
                                   group scheduler by scheduler.SlotTypeID into gr
                                   let count = gr.Count()
                                   select count;

                outputSlotTypeDataGridView.Rows[i].Cells[0].Value = SlotTypes.ElementAtOrDefault(i).SlotTypeCode;
                outputSlotTypeDataGridView.Rows[i].Cells[1].Value = SlotTypes.ElementAtOrDefault(i).SlotNumber;
                outputSlotTypeDataGridView.Rows[i].Cells[2].Value = SlotTypes.ElementAtOrDefault(i).TimeStart;
                outputSlotTypeDataGridView.Rows[i].Cells[3].Value = SlotTypes.ElementAtOrDefault(i).TimeEnd;
                outputSlotTypeDataGridView.Rows[i].Cells[4].Value = SlotTypes.ElementAtOrDefault(i).DateOfWeek;
                outputSlotTypeDataGridView.Rows[i].Cells[5].Value = roomSemester.ElementAtOrDefault(0).Quantity;
                outputSlotTypeDataGridView.Rows[i].Cells[6].Value = assignAmount.ElementAtOrDefault(0);
            }
        }

        private void SchedulerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            loginForm.Close();
        }
    }

    public class ScheduleShow
    {
        public Schedule Schedule;
        public List<CourseAssign> scheduleItem;
        public string runTime;
        public double pointPerRowMax;
        public double totalAveragePoint;
        public ScheduleShow()
        {
        }

        public ScheduleShow(Schedule schedule, List<CourseAssign> scheduleItem, string runTime, double pointPerRowMax, double totalAveragePoint)
        {
            Schedule = schedule;
            this.scheduleItem = scheduleItem;
            this.runTime = runTime;
            this.pointPerRowMax = pointPerRowMax;
            this.totalAveragePoint = totalAveragePoint;
        }

    }

}
