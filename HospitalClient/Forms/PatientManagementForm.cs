using HospitalClient.Services;
using System;
using System.Data;
using System.Windows.Forms;

namespace HospitalClient.Forms
{
    public partial class PatientManagementForm : Form
    {
        SQLService sqlService = new SQLService();

        public PatientManagementForm()
        {
            InitializeComponent();
        }

        private void PatientManagementForm_Load(object sender, EventArgs e)
        {
            LoadPatients();
        }

        private void LoadPatients()
        {
            dgvPatients.DataSource = sqlService.GetAllPatients();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPatients();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required.");
                return;
            }

            Patient patient = new Patient
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
                UserId = ""
            };

            sqlService.InsertPatient(patient);
            LoadPatients();

            MessageBox.Show("Patient added.");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Patient patient = new Patient
            {
                PatientId = int.Parse(txtPatientId.Text),
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text
            };

            sqlService.UpdatePatient(patient);
            LoadPatients();

            MessageBox.Show("Patient updated.");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int patientId = int.Parse(txtPatientId.Text);

            sqlService.DeletePatient(patientId);
            LoadPatients();

            MessageBox.Show("Patient deleted.");
        }

        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPatients.CurrentRow == null) return;

            txtPatientId.Text = dgvPatients.CurrentRow.Cells["PatientId"].Value.ToString();
            txtFirstName.Text = dgvPatients.CurrentRow.Cells["FirstName"].Value.ToString();
            txtLastName.Text = dgvPatients.CurrentRow.Cells["LastName"].Value.ToString();
            txtPhone.Text = dgvPatients.CurrentRow.Cells["Phone"].Value.ToString();
            txtEmail.Text = dgvPatients.CurrentRow.Cells["Email"].Value.ToString();
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            btnAdd_Click(sender, e);
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            btnUpdate_Click(sender, e);
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            btnDelete_Click(sender, e);
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
            this.Hide();
        }
    }
}
