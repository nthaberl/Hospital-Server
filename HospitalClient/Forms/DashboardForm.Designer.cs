namespace HospitalClient.Forms
{
    partial class DashboardForm
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
            this.labelWelcome = new System.Windows.Forms.Label();
            this.buttonAppt = new System.Windows.Forms.Button();
            this.buttonInventory = new System.Windows.Forms.Button();
            this.buttonChat = new System.Windows.Forms.Button();
            this.buttonPatientMgmt = new System.Windows.Forms.Button();
            this.buttonVitals = new System.Windows.Forms.Button();
            this.buttonAnalytics = new System.Windows.Forms.Button();
            this.buttonLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelWelcome
            // 
            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWelcome.Location = new System.Drawing.Point(50, 33);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new System.Drawing.Size(198, 24);
            this.labelWelcome.TabIndex = 0;
            this.labelWelcome.Text = "Welcome, name (role)";
            // 
            // buttonAppt
            // 
            this.buttonAppt.Location = new System.Drawing.Point(181, 83);
            this.buttonAppt.Name = "buttonAppt";
            this.buttonAppt.Size = new System.Drawing.Size(86, 23);
            this.buttonAppt.TabIndex = 1;
            this.buttonAppt.Text = "Appointments";
            this.buttonAppt.UseVisualStyleBackColor = true;
            this.buttonAppt.Click += new System.EventHandler(this.buttonAppt_Click);
            // 
            // buttonInventory
            // 
            this.buttonInventory.Location = new System.Drawing.Point(54, 112);
            this.buttonInventory.Name = "buttonInventory";
            this.buttonInventory.Size = new System.Drawing.Size(86, 23);
            this.buttonInventory.TabIndex = 2;
            this.buttonInventory.Text = "Inventory";
            this.buttonInventory.UseVisualStyleBackColor = true;
            this.buttonInventory.Visible = false;
            this.buttonInventory.Click += new System.EventHandler(this.buttonInventory_Click);
            // 
            // buttonChat
            // 
            this.buttonChat.Location = new System.Drawing.Point(181, 112);
            this.buttonChat.Name = "buttonChat";
            this.buttonChat.Size = new System.Drawing.Size(86, 23);
            this.buttonChat.TabIndex = 3;
            this.buttonChat.Text = "Chat";
            this.buttonChat.UseVisualStyleBackColor = true;
            this.buttonChat.Click += new System.EventHandler(this.buttonChat_Click);
            // 
            // buttonPatientMgmt
            // 
            this.buttonPatientMgmt.Location = new System.Drawing.Point(54, 170);
            this.buttonPatientMgmt.Name = "buttonPatientMgmt";
            this.buttonPatientMgmt.Size = new System.Drawing.Size(86, 40);
            this.buttonPatientMgmt.TabIndex = 4;
            this.buttonPatientMgmt.Text = "Patient Records";
            this.buttonPatientMgmt.UseVisualStyleBackColor = true;
            this.buttonPatientMgmt.Visible = false;
            this.buttonPatientMgmt.Click += new System.EventHandler(this.buttonPatientMgmt_Click);
            // 
            // buttonVitals
            // 
            this.buttonVitals.Location = new System.Drawing.Point(54, 141);
            this.buttonVitals.Name = "buttonVitals";
            this.buttonVitals.Size = new System.Drawing.Size(86, 23);
            this.buttonVitals.TabIndex = 5;
            this.buttonVitals.Text = "ER Updates";
            this.buttonVitals.UseVisualStyleBackColor = true;
            this.buttonVitals.Visible = false;
            this.buttonVitals.Click += new System.EventHandler(this.buttonVitals_Click);
            // 
            // buttonAnalytics
            // 
            this.buttonAnalytics.Location = new System.Drawing.Point(54, 83);
            this.buttonAnalytics.Name = "buttonAnalytics";
            this.buttonAnalytics.Size = new System.Drawing.Size(86, 23);
            this.buttonAnalytics.TabIndex = 6;
            this.buttonAnalytics.Text = "Analytics";
            this.buttonAnalytics.UseVisualStyleBackColor = true;
            this.buttonAnalytics.Visible = false;
            this.buttonAnalytics.Click += new System.EventHandler(this.buttonAnalytics_Click);
            // 
            // buttonLogout
            // 
            this.buttonLogout.Location = new System.Drawing.Point(119, 297);
            this.buttonLogout.Name = "buttonLogout";
            this.buttonLogout.Size = new System.Drawing.Size(75, 23);
            this.buttonLogout.TabIndex = 7;
            this.buttonLogout.Text = "Logout";
            this.buttonLogout.UseVisualStyleBackColor = true;
            this.buttonLogout.Click += new System.EventHandler(this.buttonLogout_Click);
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 349);
            this.Controls.Add(this.buttonLogout);
            this.Controls.Add(this.buttonAnalytics);
            this.Controls.Add(this.buttonVitals);
            this.Controls.Add(this.buttonPatientMgmt);
            this.Controls.Add(this.buttonChat);
            this.Controls.Add(this.buttonInventory);
            this.Controls.Add(this.buttonAppt);
            this.Controls.Add(this.labelWelcome);
            this.Name = "DashboardForm";
            this.Text = "DashboardForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWelcome;
        private System.Windows.Forms.Button buttonAppt;
        private System.Windows.Forms.Button buttonInventory;
        private System.Windows.Forms.Button buttonChat;
        private System.Windows.Forms.Button buttonPatientMgmt;
        private System.Windows.Forms.Button buttonVitals;
        private System.Windows.Forms.Button buttonAnalytics;
        private System.Windows.Forms.Button buttonLogout;
    }
}