using Microsoft.AspNetCore.SignalR.Client;
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
    public partial class PatientMonitoringForm : Form
    {
        private HubConnection _connection;

        public PatientMonitoringForm()
        {
            InitializeComponent();
        }

        private async void PatientMonitoringForm_Load(object sender, EventArgs e)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5059/hospitalhub")
                .Build();

            // each update displays as a patient name with room number and preformatted, random vitals string
            _connection.On<string, string>("ReceiveVitalsUpdate", (patientName, vitals) =>
            {
                Invoke((Action)(() =>
                {
                    string entry = $"[{DateTime.Now:HH:mm:ss}] {patientName} — {vitals}";
                    listBox1.Items.Insert(0, entry); // newest at top

                    // cap at 50 entries so the list doesn't grow forever
                    if (listBox1.Items.Count > 50)
                        listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                }));
            });

            try
            {
                await _connection.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not connect: {ex.Message}");
            }
        }

        //close connection when form closes to prevent SignalR from continuously emitting updates
        private async void PatientMonitoringForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connection != null)
                await _connection.StopAsync();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
            this.Hide();
        }

    }
}
