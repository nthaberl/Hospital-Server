namespace HospitalClient.Forms
{
    partial class AnalyticsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotalVisits = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCommonConcern = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblMedicationUsage = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvAppointmentStatus = new System.Windows.Forms.DataGridView();
            this.dgvMedicationUsage = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.btnRefreshReports = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointmentStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicationUsage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(220, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Analytics Dashboard ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(111, 122);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Total Patient Visits:";
            // 
            // lblTotalVisits
            // 
            this.lblTotalVisits.AutoSize = true;
            this.lblTotalVisits.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalVisits.Location = new System.Drawing.Point(238, 122);
            this.lblTotalVisits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTotalVisits.Name = "lblTotalVisits";
            this.lblTotalVisits.Size = new System.Drawing.Size(16, 17);
            this.lblTotalVisits.TabIndex = 2;
            this.lblTotalVisits.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(112, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Most Common Concern:";
            // 
            // lblCommonConcern
            // 
            this.lblCommonConcern.AutoSize = true;
            this.lblCommonConcern.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommonConcern.Location = new System.Drawing.Point(257, 166);
            this.lblCommonConcern.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCommonConcern.Name = "lblCommonConcern";
            this.lblCommonConcern.Size = new System.Drawing.Size(31, 17);
            this.lblCommonConcern.TabIndex = 4;
            this.lblCommonConcern.Text = "N/A";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(111, 205);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Most Used Medication:";
            // 
            // lblMedicationUsage
            // 
            this.lblMedicationUsage.AutoSize = true;
            this.lblMedicationUsage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicationUsage.Location = new System.Drawing.Point(257, 205);
            this.lblMedicationUsage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMedicationUsage.Name = "lblMedicationUsage";
            this.lblMedicationUsage.Size = new System.Drawing.Size(31, 17);
            this.lblMedicationUsage.TabIndex = 6;
            this.lblMedicationUsage.Text = "N/A";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(112, 256);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(204, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Appointment Status Report";
            // 
            // dgvAppointmentStatus
            // 
            this.dgvAppointmentStatus.AllowUserToAddRows = false;
            this.dgvAppointmentStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointmentStatus.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dgvAppointmentStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointmentStatus.Location = new System.Drawing.Point(114, 302);
            this.dgvAppointmentStatus.Margin = new System.Windows.Forms.Padding(2);
            this.dgvAppointmentStatus.Name = "dgvAppointmentStatus";
            this.dgvAppointmentStatus.ReadOnly = true;
            this.dgvAppointmentStatus.RowHeadersVisible = false;
            this.dgvAppointmentStatus.RowHeadersWidth = 51;
            this.dgvAppointmentStatus.RowTemplate.Height = 24;
            this.dgvAppointmentStatus.Size = new System.Drawing.Size(360, 122);
            this.dgvAppointmentStatus.TabIndex = 8;
            // 
            // dgvMedicationUsage
            // 
            this.dgvMedicationUsage.AllowUserToAddRows = false;
            this.dgvMedicationUsage.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMedicationUsage.BackgroundColor = System.Drawing.SystemColors.InactiveCaption;
            this.dgvMedicationUsage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMedicationUsage.Location = new System.Drawing.Point(114, 513);
            this.dgvMedicationUsage.Margin = new System.Windows.Forms.Padding(2);
            this.dgvMedicationUsage.Name = "dgvMedicationUsage";
            this.dgvMedicationUsage.ReadOnly = true;
            this.dgvMedicationUsage.RowHeadersVisible = false;
            this.dgvMedicationUsage.RowHeadersWidth = 51;
            this.dgvMedicationUsage.RowTemplate.Height = 24;
            this.dgvMedicationUsage.Size = new System.Drawing.Size(360, 122);
            this.dgvMedicationUsage.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(112, 470);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(190, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Medication Usage Report";
            // 
            // btnRefreshReports
            // 
            this.btnRefreshReports.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshReports.Location = new System.Drawing.Point(224, 661);
            this.btnRefreshReports.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefreshReports.Name = "btnRefreshReports";
            this.btnRefreshReports.Size = new System.Drawing.Size(140, 37);
            this.btnRefreshReports.TabIndex = 11;
            this.btnRefreshReports.Text = "Refresh Reports";
            this.btnRefreshReports.UseVisualStyleBackColor = true;
            this.btnRefreshReports.Click += new System.EventHandler(this.btnRefreshReports_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(224, 755);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(121, 23);
            this.buttonExit.TabIndex = 12;
            this.buttonExit.Text = "Return to Dashboard";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click_1);
            // 
            // AnalyticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 801);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.btnRefreshReports);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dgvMedicationUsage);
            this.Controls.Add(this.dgvAppointmentStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblMedicationUsage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblCommonConcern);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTotalVisits);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AnalyticsForm";
            this.Text = "Analytics Dashboard";
            this.Load += new System.EventHandler(this.AnalyticsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointmentStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMedicationUsage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalVisits;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCommonConcern;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblMedicationUsage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvAppointmentStatus;
        private System.Windows.Forms.DataGridView dgvMedicationUsage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRefreshReports;
        private System.Windows.Forms.Button buttonExit;
    }
}