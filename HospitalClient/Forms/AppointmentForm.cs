using HospitalClient.HospitalManagementSystem;
using HospitalClient.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace HospitalClient.Forms
{
    public partial class AppointmentForm : Form
    {
        private SQLService _sqlService;
        private HubConnection _connection;
        private DataTable _appointments;

        private string CurrentRole
        {
            get
            {
                if (UserSession.CurrentUser == null || UserSession.CurrentUser.Role == null)
                {
                    return string.Empty;
                }

                return UserSession.CurrentUser.Role.ToLower();
            }
        }

        public AppointmentForm()
        {
            InitializeComponent();

            _sqlService = new SQLService();

            ConfigureRoleAccess();
            LoadDropdowns();
            LoadAppointments();
            StartSignalRConnection();
        }

        private void ConfigureRoleAccess()
        {
            labelRole.Text = "Role: " + CurrentRole;

            if (CurrentRole == "patient")
            {
                comboBoxPatient.Enabled = false;
                comboBoxStaff.Enabled = true;
            }
            else if (CurrentRole == "doctor" || CurrentRole == "nurse")
            {
                comboBoxPatient.Enabled = true;
                comboBoxStaff.Enabled = false;
            }
            else if (CurrentRole == "admin")
            {
                comboBoxPatient.Enabled = true;
                comboBoxStaff.Enabled = true;
            }
        }

        private void LoadDropdowns()
        {
            DataTable patients = _sqlService.GetPatientOptions();
            comboBoxPatient.DataSource = patients;
            comboBoxPatient.DisplayMember = "DisplayName";
            comboBoxPatient.ValueMember = "PatientId";

            DataTable staff = _sqlService.GetStaffOptionsForAppointments();
            comboBoxStaff.DataSource = staff;
            comboBoxStaff.DisplayMember = "DisplayName";
            comboBoxStaff.ValueMember = "StaffId";

            if (CurrentRole == "patient")
            {
                comboBoxPatient.SelectedValue = UserSession.CurrentPatientId;
            }

            if (CurrentRole == "doctor" || CurrentRole == "nurse")
            {
                comboBoxStaff.SelectedValue = UserSession.CurrentStaffId;
            }
        }

        private void LoadAppointments()
        {
            try
            {
                _appointments = _sqlService.GetAppointmentsForUser(
                    CurrentRole,
                    UserSession.CurrentPatientId,
                    UserSession.CurrentStaffId);

                dataGridViewAppointments.DataSource = null;
                dataGridViewAppointments.DataSource = _appointments;

                FormatAppointmentGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load appointments.\n\n" + ex.Message,
                    "Appointment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FormatAppointmentGrid()
        {
            dataGridViewAppointments.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dataGridViewAppointments.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridViewAppointments.MultiSelect = false;
            dataGridViewAppointments.ReadOnly = true;
            dataGridViewAppointments.AllowUserToAddRows = false;

            if (dataGridViewAppointments.Columns.Contains("AppointmentId"))
            {
                dataGridViewAppointments.Columns["AppointmentId"].HeaderText = "ID";
            }

            if (dataGridViewAppointments.Columns.Contains("PatientId"))
            {
                dataGridViewAppointments.Columns["PatientId"].Visible = false;
            }

            if (dataGridViewAppointments.Columns.Contains("StaffId"))
            {
                dataGridViewAppointments.Columns["StaffId"].Visible = false;
            }

            if (dataGridViewAppointments.Columns.Contains("PatientName"))
            {
                dataGridViewAppointments.Columns["PatientName"].HeaderText = "Patient";
            }

            if (dataGridViewAppointments.Columns.Contains("StaffName"))
            {
                dataGridViewAppointments.Columns["StaffName"].HeaderText = "Doctor/Nurse";
            }

            if (dataGridViewAppointments.Columns.Contains("ScheduledDate"))
            {
                dataGridViewAppointments.Columns["ScheduledDate"].HeaderText = "Date / Time";
            }
        }

        private async void StartSignalRConnection()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5059/hospitalhub")
                .WithAutomaticReconnect()
                .Build();

            _connection.On<string, int, int, int, string>(
                "AppointmentUpdated",
                (action, appointmentId, patientId, staffId, changedByRole) =>
                {
                    RunOnUiThread(() =>
                    {
                        bool relevantToCurrentUser =
                            CurrentRole == "admin" ||
                            UserSession.CurrentPatientId == patientId ||
                            UserSession.CurrentStaffId == staffId;

                        if (!relevantToCurrentUser)
                        {
                            return;
                        }

                        LoadAppointments();

                        MessageBox.Show(
                            "Appointment " + action + " by " + changedByRole + ".",
                            "Appointment Update",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    });
                });

            try
            {
                await _connection.StartAsync();
                labelStatus.Text = "SignalR: connected";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "SignalR: offline";

                MessageBox.Show(
                    "SignalR connection failed.\n\n" + ex.ToString(),
                    "SignalR Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            int patientId;
            int staffId;

            if (CurrentRole == "patient")
            {
                patientId = UserSession.CurrentPatientId;
            }
            else
            {
                patientId = Convert.ToInt32(comboBoxPatient.SelectedValue);
            }

            if (CurrentRole == "doctor" || CurrentRole == "nurse")
            {
                staffId = UserSession.CurrentStaffId;
            }
            else
            {
                staffId = Convert.ToInt32(comboBoxStaff.SelectedValue);
            }

            DateTime scheduledDate = dateTimePickerAppointment.Value;
            string concern = textBoxConcern.Text.Trim();
            string notes = textBoxNotes.Text.Trim();

            if (concern.Length == 0)
            {
                MessageBox.Show("Please enter a concern for the appointment.");
                return;
            }

            try
            {
                int appointmentId = _sqlService.InsertAppointment(
                    patientId,
                    staffId,
                    scheduledDate,
                    concern,
                    notes);

                LoadAppointments();

                await NotifyAppointmentChanged(
                    "scheduled",
                    appointmentId,
                    patientId,
                    staffId);

                MessageBox.Show("Appointment scheduled successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not schedule appointment.\n\n" + ex.Message,
                    "Appointment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void buttonCancel_Click(object sender, EventArgs e)
        {
            if (dataGridViewAppointments.CurrentRow == null)
            {
                MessageBox.Show("Select an appointment first.");
                return;
            }

            DataRowView row =
                dataGridViewAppointments.CurrentRow.DataBoundItem as DataRowView;

            if (row == null)
            {
                return;
            }

            int appointmentId = Convert.ToInt32(row["AppointmentId"]);
            int patientId = Convert.ToInt32(row["PatientId"]);
            int staffId = Convert.ToInt32(row["StaffId"]);
            string status = row["Status"].ToString();

            if (status.ToLower() == "cancelled")
            {
                MessageBox.Show("This appointment is already cancelled.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Cancel this appointment?",
                "Confirm Cancel",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);

            if (result != DialogResult.OK)
            {
                return;
            }

            try
            {
                _sqlService.CancelAppointment(appointmentId);
                LoadAppointments();

                await NotifyAppointmentChanged(
                    "cancelled",
                    appointmentId,
                    patientId,
                    staffId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not cancel appointment.\n\n" + ex.Message,
                    "Appointment Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            Hide();
        }

        private async Task NotifyAppointmentChanged(
            string action,
            int appointmentId,
            int patientId,
            int staffId)
        {
            if (_connection == null || _connection.State != HubConnectionState.Connected)
            {
                return;
            }

            await _connection.InvokeAsync(
                "NotifyAppointmentChanged",
                action,
                appointmentId,
                patientId,
                staffId,
                CurrentRole);
        }

        private void RunOnUiThread(Action action)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }

            base.OnFormClosing(e);
        }
    }
}