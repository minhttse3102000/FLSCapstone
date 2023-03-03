namespace WindowsFormsApp
{
    partial class ScheduleDetailDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lecturerIDLabel = new System.Windows.Forms.Label();
            this.lecturerNameLabel = new System.Windows.Forms.Label();
            this.roleLabel = new System.Windows.Forms.Label();
            this.semesterLabel = new System.Windows.Forms.Label();
            this.outputFromLabel = new System.Windows.Forms.Label();
            this.outputToLabel = new System.Windows.Forms.Label();
            this.teachableTimeLabel = new System.Windows.Forms.Label();
            this.schoolTimeLabel = new System.Windows.Forms.Label();
            this.lecturerPointLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column8,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.dataGridView1.Location = new System.Drawing.Point(76, 290);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1374, 306);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.Width = 125;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Monday";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Tuesday";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Wednesday";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 125;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Thursday";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 125;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Friday";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 125;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Saturday";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 125;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Sunday";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.Width = 80;
            // 
            // lecturerIDLabel
            // 
            this.lecturerIDLabel.AutoSize = true;
            this.lecturerIDLabel.Location = new System.Drawing.Point(73, 150);
            this.lecturerIDLabel.Name = "lecturerIDLabel";
            this.lecturerIDLabel.Size = new System.Drawing.Size(74, 16);
            this.lecturerIDLabel.TabIndex = 1;
            this.lecturerIDLabel.Text = "Lecturer ID:";
            // 
            // lecturerNameLabel
            // 
            this.lecturerNameLabel.AutoSize = true;
            this.lecturerNameLabel.Location = new System.Drawing.Point(73, 184);
            this.lecturerNameLabel.Name = "lecturerNameLabel";
            this.lecturerNameLabel.Size = new System.Drawing.Size(95, 16);
            this.lecturerNameLabel.TabIndex = 2;
            this.lecturerNameLabel.Text = "Lecturer name:";
            // 
            // roleLabel
            // 
            this.roleLabel.AutoSize = true;
            this.roleLabel.Location = new System.Drawing.Point(73, 222);
            this.roleLabel.Name = "roleLabel";
            this.roleLabel.Size = new System.Drawing.Size(39, 16);
            this.roleLabel.TabIndex = 3;
            this.roleLabel.Text = "Role:";
            // 
            // semesterLabel
            // 
            this.semesterLabel.AutoSize = true;
            this.semesterLabel.Location = new System.Drawing.Point(50, 32);
            this.semesterLabel.Name = "semesterLabel";
            this.semesterLabel.Size = new System.Drawing.Size(68, 16);
            this.semesterLabel.TabIndex = 4;
            this.semesterLabel.Text = "Semester:";
            // 
            // outputFromLabel
            // 
            this.outputFromLabel.AutoSize = true;
            this.outputFromLabel.Location = new System.Drawing.Point(50, 71);
            this.outputFromLabel.Name = "outputFromLabel";
            this.outputFromLabel.Size = new System.Drawing.Size(71, 16);
            this.outputFromLabel.TabIndex = 5;
            this.outputFromLabel.Text = "From date:";
            // 
            // outputToLabel
            // 
            this.outputToLabel.AutoSize = true;
            this.outputToLabel.Location = new System.Drawing.Point(249, 71);
            this.outputToLabel.Name = "outputToLabel";
            this.outputToLabel.Size = new System.Drawing.Size(60, 16);
            this.outputToLabel.TabIndex = 6;
            this.outputToLabel.Text = "To date: ";
            // 
            // teachableTimeLabel
            // 
            this.teachableTimeLabel.AutoSize = true;
            this.teachableTimeLabel.Location = new System.Drawing.Point(485, 183);
            this.teachableTimeLabel.Name = "teachableTimeLabel";
            this.teachableTimeLabel.Size = new System.Drawing.Size(104, 16);
            this.teachableTimeLabel.TabIndex = 7;
            this.teachableTimeLabel.Text = "Teachable time:";
            // 
            // schoolTimeLabel
            // 
            this.schoolTimeLabel.AutoSize = true;
            this.schoolTimeLabel.Location = new System.Drawing.Point(485, 222);
            this.schoolTimeLabel.Name = "schoolTimeLabel";
            this.schoolTimeLabel.Size = new System.Drawing.Size(91, 16);
            this.schoolTimeLabel.TabIndex = 8;
            this.schoolTimeLabel.Text = "In school time:";
            // 
            // lecturerPointLabel
            // 
            this.lecturerPointLabel.AutoSize = true;
            this.lecturerPointLabel.Location = new System.Drawing.Point(816, 183);
            this.lecturerPointLabel.Name = "lecturerPointLabel";
            this.lecturerPointLabel.Size = new System.Drawing.Size(40, 16);
            this.lecturerPointLabel.TabIndex = 9;
            this.lecturerPointLabel.Text = "Point:";
            // 
            // ScheduleDetailDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1499, 683);
            this.Controls.Add(this.lecturerPointLabel);
            this.Controls.Add(this.schoolTimeLabel);
            this.Controls.Add(this.teachableTimeLabel);
            this.Controls.Add(this.outputToLabel);
            this.Controls.Add(this.outputFromLabel);
            this.Controls.Add(this.semesterLabel);
            this.Controls.Add(this.roleLabel);
            this.Controls.Add(this.lecturerNameLabel);
            this.Controls.Add(this.lecturerIDLabel);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ScheduleDetailDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lecturer schedule";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lecturerIDLabel;
        private System.Windows.Forms.Label lecturerNameLabel;
        private System.Windows.Forms.Label roleLabel;
        private System.Windows.Forms.Label semesterLabel;
        private System.Windows.Forms.Label outputFromLabel;
        private System.Windows.Forms.Label outputToLabel;
        private System.Windows.Forms.Label teachableTimeLabel;
        private System.Windows.Forms.Label schoolTimeLabel;
        private System.Windows.Forms.Label lecturerPointLabel;
    }
}