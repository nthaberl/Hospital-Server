using HospitalClient.HospitalManagementSystem;
using HospitalClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalClient.Forms
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {


            InitializeComponent();
            LoadDashboard();
        }

        //method to only display buttons available to user based on user's role
        private void LoadDashboard()
        {
            //display logged-in user's name and role at top for clarity
            //when running multiple instances
            // fetch first name from SQL using the ID already resolved at login
            SQLService sqlService = new SQLService();
            string firstName = UserSession.CurrentStaffId != 0
    ? sqlService.GetStaffFirstName(UserSession.CurrentStaffId)
    : sqlService.GetPatientFirstName(UserSession.CurrentPatientId);
            MessageBox.Show($"PatientId: {UserSession.CurrentPatientId}, FirstName: {firstName}");
            labelWelcome.Text = $"Welcome, {firstName} ({UserSession.CurrentUser.Role})";

            // visible to all roles
            buttonAppt.Visible = true;
            buttonChat.Visible = true;

            string role = UserSession.CurrentUser.Role.ToLower();

            // admin, doctor, nurse only
            bool isStaff = role == "admin" || role == "doctor" || role == "nurse";

            buttonPatientMgmt.Visible = isStaff;
            buttonInventory.Visible = isStaff;
            buttonVitals.Visible = isStaff;


            // admin only
            buttonAnalytics.Visible = UserSession.CurrentUser.Role == "admin";
        }

        //clears session and returns user to login form
        //session is cleared to prevent leftover data when new user logs in
        private void buttonLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "You'll be logged out, would you like to continue?",
                "Logout", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                UserSession.CurrentUser = null;
                UserSession.CurrentPatientId = 0;
                UserSession.CurrentStaffId = 0;

                LoginRegForm loginRegForm = new LoginRegForm();
                loginRegForm.Show();
                this.Hide();
            }
        }

        //------ Form Navigation ------//

        private void buttonAppt_Click(object sender, EventArgs e)
        {
            AppointmentForm appointmentForm = new AppointmentForm();
            appointmentForm.Show();
            this.Hide();
        }

        private void buttonInventory_Click(object sender, EventArgs e)
        {
            InventoryForm inventoryForm = new InventoryForm();
            inventoryForm.Show();
            this.Hide();
        }

        private void buttonChat_Click(object sender, EventArgs e)
        {
            ChatForm chatForm = new ChatForm();
            chatForm.Show();
            this.Hide();
        }

        private void buttonAnalytics_Click(object sender, EventArgs e)
        {
            AnalyticsForm analyticsForm = new AnalyticsForm();
            analyticsForm.Show();
            this.Hide();
        }

        private void buttonVitals_Click(object sender, EventArgs e)
        {
            PatientMonitoringForm patientMonitoringForm = new PatientMonitoringForm();
            patientMonitoringForm.Show();
            this.Hide();
        }

        private void buttonPatientMgmt_Click(object sender, EventArgs e)
        {
            PatientManagementForm patientMgmtForm = new PatientManagementForm();
            patientMgmtForm.Show();
            this.Hide();
        }


    }
}
