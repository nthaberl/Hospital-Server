using HospitalClient.HospitalManagementSystem;
using HospitalClient.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalClient.Forms
{
    public partial class ChatForm : Form
    {
        private HubConnection _connection;
        private SQLService _sqlService = new SQLService();

        public ChatForm()
        {
            InitializeComponent();
            LoadRecipients();

        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            InitializeSignalR();
        }

        //helper function to populate dropdown menu with appropriate user list based on
        //the role of the person who is logged in
        private void LoadRecipients()
        {
            //instantiating list of users to store either all patients or all staff
            List<UserList> users;

            //currentstaffid is non-zero only when staff member is logged in
            if (UserSession.CurrentStaffId != 0)
            {
                // logged in as staff — show only patients
                users = _sqlService.GetAllPatientUsers();
            }
            else
            {
                // logged in as patient — show only staff
                users = _sqlService.GetAllStaffUsers();
            }

            //binding list of appropriate recipients to combobox
            comboboxRecipient.DataSource = users;
            comboboxRecipient.DisplayMember = "DisplayName";
            comboboxRecipient.ValueMember = "UserId";
        }

        private async void InitializeSignalR()
        {
            _connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:5059/hospitalhub")
               .Build();

            _connection.On<string, string, string>("ReceiveMessage", (senderUserId, receiverUserId, message) =>
            {
                string currentUserId = UserSession.CurrentUser.UserId;

                Invoke((Action)(() =>
                {
                    string selectedUserId = (string)comboboxRecipient.SelectedValue;

                    //only display messages involving current user and selected recipient
                    //filtering out messages between other users using same hub connection
                    bool isForMe = receiverUserId == currentUserId && senderUserId == selectedUserId;
                    bool isFromMe = senderUserId == currentUserId && receiverUserId == selectedUserId;

                    if (isForMe || isFromMe)
                    {
                        //messages labeled with "You" is send by current user, otherwise displays recipient's name
                        string label = isFromMe ? "You" : comboboxRecipient.Text;
                        listBox1.Items.Add($"{label}: {message}");
                    }
                }));
            });

            try
            {
                await _connection.StartAsync();
                MessageBox.Show("Connected to chat successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //send message via SignalR and save copy of message into SQL database
        private async void buttonSend_Click(object sender, EventArgs e)
        {
            string myUserId = UserSession.CurrentUser.UserId;
            string recipientId = (string)comboboxRecipient.SelectedValue;
            string message = textboxMessage.Text;

            try
            {
                await _connection.InvokeAsync("SendMessage", myUserId, recipientId, message);
                _sqlService.SaveChatMessage(myUserId, recipientId, message);
                textboxMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            DashboardForm dashboardForm = new DashboardForm();
            dashboardForm.Show();
            this.Hide();
        }

        //used to bind users to recipient combobox.
        //userid used internally for routing messages; DisplayName is shown to user
        public class UserList
        {
            public string UserId { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
