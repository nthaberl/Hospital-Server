using HospitalClient.Services;
using System;
using System.Windows.Forms;

namespace HospitalClient.Forms
{
    public partial class AnalyticsForm : Form
    {
        SQLService sqlService = new SQLService();

        public AnalyticsForm()
        {
            InitializeComponent();
        }

        private void AnalyticsForm_Load(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void LoadReports()
        {
            lblTotalVisits.Text = sqlService.GetTotalPatientVisits().ToString();
            lblCommonConcern.Text = sqlService.GetMostCommonConcern();
            lblMedicationUsage.Text = "Inventory Report";

            dgvAppointmentStatus.DataSource = sqlService.GetAppointmentStatusReport();
            dgvMedicationUsage.DataSource = sqlService.GetMedicationUsageReport();
        }

        private void btnRefreshReports_Click(object sender, EventArgs e)
        {
            LoadReports();
        }

        private void buttonExit_Click_1(object sender, EventArgs e)
        {
            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
            this.Hide();
        }
    }
}
