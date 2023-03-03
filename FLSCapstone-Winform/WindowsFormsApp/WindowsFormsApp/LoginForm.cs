using FPTULecturerScheduler.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.DAO;

namespace WindowsFormsApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            invalidTextbox.Hide();
        }

        private async void loginButton_ClickAsync(object sender, EventArgs e)
        {
            await loginAsync();
            //SchedulerForm schedulerForm = new SchedulerForm(this, user);

            //schedulerForm.Show();
            //this.Hide();

        }

         async Task loginAsync()
        {
            if (tokenKeyTextBox.Text.Count() == 0)
            {
                invalidTextbox.Show();
                invalidTextbox.Text = "nhap key vao ";
            }
            else
            {
                Lecturer user = await LecturerDAO.GetUserByKeyAsync(tokenKeyTextBox.Text.ToString());
                if (user == null)
                {
                    invalidTextbox.Show();
                    invalidTextbox.Text = "Invalid key";
                }
                else
                {
                    SchedulerForm schedulerForm = new SchedulerForm(this, user);
                    schedulerForm.Show();
                    this.Hide();
                }
            }                                           
        }

    }

}
