using FPTULecturerScheduler.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class ScheduleDetailDialog : Form
    {

        public ScheduleDetailDialog(Semester semester, Lecturer lecturer, List<CourseAssign> scheduleItem, List<SlotType> slotTypes, ScheduleShow schedule)
        {
            InitializeComponent();

            //load semester
            semesterLabel.Text = "Semester:  " + semester.Term;
            string[] temp1 = semester.DateStart.Split('-');
            string[] temp2 = semester.DateEnd.Split('-');
            DateTime dateStart = new DateTime(Convert.ToInt32(temp1[0]), Convert.ToInt32(temp1[1]), Convert.ToInt32(temp1[2]));
            DateTime dateEnd = new DateTime(Convert.ToInt32(temp2[0]), Convert.ToInt32(temp2[1]), Convert.ToInt32(temp2[2]));
            outputFromLabel.Text = "From date: " + String.Format("{0:MM/dd/yyyy}", dateStart);
            outputToLabel.Text = "To date: " + String.Format("{0:MM/dd/yyyy}", dateEnd);


            //load lecturer
            lecturerIDLabel.Text = "Lecturer ID:  " + lecturer.ID;
            lecturerNameLabel.Text =  "Lecturer name:  " + lecturer.LecturerName;
            if (lecturer.IsFullTime == 1)
            {
                roleLabel.Text =  "Role: Full time lecturer";
            }
            else
            {
                roleLabel.Text = "Role: Part time lecturer";
            }


            //load teachable time, in school time
            var teachableTime = from scheduler in scheduleItem
                                where scheduler.LecturerID == lecturer.ID 
                                select scheduler;
            teachableTimeLabel.Text = "Teachable time: " + ((teachableTime.Count() * 2.25)*2) +" (hour/week)";


            //in school time
            double inSchoolTime = 0;
            var courseMonday = from scheduler in scheduleItem
                               join slotType in slotTypes on scheduler.SlotTypeID equals slotType.ID
                               where scheduler.LecturerID == lecturer.ID && slotType.DateOfWeek.Contains("Monday")
                               select (scheduler, slotType.SlotNumber);

            var courseTuesday = from scheduler in scheduleItem
                                join slotType in slotTypes on scheduler.SlotTypeID equals slotType.ID
                                where scheduler.LecturerID == lecturer.ID && slotType.DateOfWeek.Contains("Tuesday")
                                select (scheduler, slotType.SlotNumber);
            var courseWednesday = from scheduler in scheduleItem
                                  join slotType in slotTypes on scheduler.SlotTypeID equals slotType.ID
                                  where scheduler.LecturerID == lecturer.ID && slotType.DateOfWeek.Contains("Wednesday")
                                  select (scheduler, slotType.SlotNumber);

            if (courseMonday.Count() > 0)
            {
                courseMonday = courseMonday.OrderBy(course => course.SlotNumber);
                int slotInSchool = courseMonday.Last().SlotNumber - courseMonday.First().SlotNumber +1;
                if(slotInSchool <= 2)
                {
                    inSchoolTime = inSchoolTime + (2.25 * slotInSchool) + (0.25 * (slotInSchool-1)) ;
                }
                else
                {
                    inSchoolTime = inSchoolTime + (2.25 * slotInSchool) + (0.25 * (slotInSchool - 1)) + 0.5;
                }
                
            }
            if (courseTuesday.Count() > 0)
            {
                courseTuesday = courseTuesday.OrderBy(course => course.SlotNumber);
                int slotInSchool = courseTuesday.Last().SlotNumber - courseTuesday.First().SlotNumber + 1;
                if (slotInSchool <= 2)
                {
                    inSchoolTime = inSchoolTime + (2.25 * slotInSchool) + (0.25 * (slotInSchool - 1));
                }
                else
                {
                    inSchoolTime = inSchoolTime + (2.25 * slotInSchool) + (0.25 * (slotInSchool - 1)) + 0.5;
                }
            }
            if (courseWednesday.Count() > 0)
            {
                courseWednesday = courseWednesday.OrderBy(course => course.SlotNumber);
                int slotInSchool = courseWednesday.Last().SlotNumber - courseWednesday.First().SlotNumber + 1;
                if (slotInSchool <= 2)
                {
                    inSchoolTime = inSchoolTime + (2.25 * slotInSchool) + (0.25 * (slotInSchool - 1));
                }
                else
                {
                    inSchoolTime = inSchoolTime + (2.25 * slotInSchool) + (0.25 * (slotInSchool - 1)) + 0.5;
                }
            }
            schoolTimeLabel.Text= "In school time: "+ (inSchoolTime*2) +" (hour/week)";


            //load lecturer point

            double point = 0;
            foreach (var item in teachableTime)
            {
                point = point + item.point;
            }
            double pointPerRow = point / teachableTime.Count();
            double lecturerPoint = Math.Round((pointPerRow * 10) / (schedule.pointPerRowMax), 2);
            for (int i = 0; i < 50; i++)
            {
                if (lecturerPoint < 10)
                {
                    break;
                }
                else
                {
                    lecturerPoint = lecturerPoint - 1;
                }
            }
            lecturerPointLabel.Text = "Point: " + lecturerPoint + " / 10";


            //load schedule
            dataGridView1.RowCount = 4;

            for (int i = 0; i < 4; i++)
            {
                var slotTime = from slotType in slotTypes                         
                                where slotType.SlotNumber == i + 1 
                                select slotType;

                dataGridView1.Rows[i].Cells[0].Value = "Slot " + (i + 1) ;

                for (int j = 1; j < 8; j++)
                {
                    string dateOfWeek = dataGridView1.Rows[i].Cells[j].OwningColumn.HeaderText; ;
                    
                    var view = from scheduler in scheduleItem
                               join slotType in slotTypes on scheduler.SlotTypeID equals slotType.ID
                               where slotType.SlotNumber == i + 1 && scheduler.LecturerID == lecturer.ID && slotType.DateOfWeek.Contains(dateOfWeek)
                               select scheduler;
                    if (view.Count() > 0)
                    {
                        dataGridView1.Rows[i].Cells[j].Value = view.ElementAtOrDefault(0).CourseID;                   
                    }
                }

            }


        }


    }
}
