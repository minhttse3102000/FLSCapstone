using FPTULecturerScheduler;
using FPTULecturerScheduler.Entity;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class ProgressDialog : Form
    {
        Thread thr;
        List<Course> temp = new List<Course>();
        public ProgressDialog(Thread thr)
        {
            this.thr = thr;

            foreach (var item in SchedulerForm.Courses)
            {
                temp.Add(item);
            }
            

            InitializeComponent();
            var rnd = new Random();
            var result = temp.OrderBy(item => rnd.Next());

            //Thread thr = new Thread(new ThreadStart(XepLich(selectScheduleItem)));

            Thread thread = new Thread(new ThreadStart(() =>
                        {
                            try
                            {
                                int i ;                               
                                foreach(var item in result)
                                {
                                    label2.Text = "Assigned course  "+ item.ID;
                                    Thread.Sleep(400);
                                }


                                //this.Close();
                            }
                            catch (Exception ex)
                            {                               
                            }
                        }));


            thread.IsBackground = true;
            thread.Start();


        }

        private void ProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            thr.Abort();
        }

        //void XepLich(List<CourseAssign> selectScheduleItem)
        //{
        //    for (int i = 0; i <= 7800000000; i++)
        //    {
        //        label2.Text = i + "";
        //    }
        //    selectScheduleItem = LecturerScheduler.Run();
        //    if(selectScheduleItem.Count > 1)
        //    {
        //        label2.Text = selectScheduleItem[selectScheduleItem.Count - 1].ToString();
        //    }

        //}


    }
}
