using HospitalClient.HospitalManagementSystem;
using HospitalClient.Models;
using HospitalClient.Services;
using MongoDB.Driver;
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
    public partial class LoginRegForm : Form
    {
        MongoDBService _mongoService;
        SQLService _sqlService;

        public LoginRegForm()
        {
            InitializeComponent();

            // Clearing session on form load ensures the previous user is logged out.
            UserSession.CurrentUser = null;
            UserSession.CurrentPatientId = 0;
            UserSession.CurrentStaffId = 0;

            _mongoService = new MongoDBService();
            _sqlService = new SQLService();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string email = textBoxEmail.Text;
            string role = comboBoxRole.Text.ToLower();

            if (username.Length == 0 ||
                password.Length == 0 ||
                email.Length == 0 ||
                role.Length == 0)
            {
                MessageBox.Show("Please fill out all fields!");
                return;
            }

            if (_mongoService.UserExists(username))
            {
                MessageBox.Show(
                    "Username already exists!\nRegister with a different username");
                return;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                Email = email,
                Role = role
            };

            _mongoService.InsertUser(newUser);

            if (role == "patient")
            {
                var patient = new Patient
                {
                    FirstName = textBoxFirstName.Text,
                    LastName = textBoxLastName.Text,
                    Phone = textBoxPhone.Text,
                    Email = textBoxEmail.Text,
                    UserId = newUser.UserId.ToString()
                };

                _sqlService.InsertPatient(patient);
            }
            else
            {
                var staff = new Staff
                {
                    FirstName = textBoxFirstName.Text,
                    LastName = textBoxLastName.Text,
                    Department = textBoxDepartment.Text,
                    Role = role,
                    UserId = newUser.UserId.ToString()
                };

                _sqlService.InsertStaff(staff);
            }

            MessageBox.Show(
                "Registered successfully!",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void combobox_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            string role = comboBoxRole.Text.ToLower();

            textBoxFirstName.Visible = true;
            textBoxLastName.Visible = true;

            textBoxPhone.Visible = role == "patient";
            labelPhone.Visible = role == "patient";

            textBoxDepartment.Visible =
                role == "doctor" ||
                role == "nurse" ||
                role == "admin";

            labelDepartment.Visible =
                role == "doctor" ||
                role == "nurse" ||
                role == "admin";
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show(
                    "Please fill out username and password fields!",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var user = _mongoService.GetUser(username, password);

            if (user == null)
            {
                MessageBox.Show(
                    "Invalid credentials.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            UserSession.CurrentUser = user;

            string role = (user.Role ?? string.Empty).ToLower();

            if (role == "patient")
            {
                UserSession.CurrentPatientId =
                    _sqlService.GetPatientIdByUserId(user.UserId);
            }
            else
            {
                UserSession.CurrentStaffId =
                    _sqlService.GetStaffIdByUserId(user.UserId);
            }

            bool receivesInventoryAlerts =
                role == "admin" ||
                role == "doctor" ||
                role == "nurse";

            if (receivesInventoryAlerts)
            {
                ShowLowStockAlertsAfterLogin();
            }

            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            Hide();
        }

        private void ShowLowStockAlertsAfterLogin()
        {
            try
            {
                List<InventoryItem> lowStockItems =
                    _sqlService
                        .GetInventoryItems()
                        .Where(item => item.IsLowStock)
                        .ToList();

                if (lowStockItems.Count == 0)
                {
                    return;
                }

                string itemLines = string.Join(
                    Environment.NewLine,
                    lowStockItems.Select(item =>
                        item.Name +
                        " - Current: " + item.QtyInStock +
                        ", Threshold: " + item.ReorderThreshold));

                MessageBox.Show(
                    "The following inventory items need to be reordered:\n\n" +
                    itemLines,
                    "Low Stock Alert",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Login succeeded, but inventory alerts could not be loaded.\n\n" +
                    ex.Message,
                    "Inventory Alert Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}
