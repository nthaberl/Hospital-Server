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

            //clearing session on form load to ensure previous user is logged out
            UserSession.CurrentUser = null;
            UserSession.CurrentPatientId = 0;
            UserSession.CurrentStaffId = 0;

            _mongoService = new MongoDBService();
            _sqlService = new SQLService();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            // get info from user
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            string email = textBoxEmail.Text;
            string role = comboBoxRole.Text.ToLower();

            //validate all fields are filled for registration
            if (username.Length == 0 || password.Length == 0 || email.Length == 0 || role.Length == 0)
            {
                MessageBox.Show("Please fill out all fields!");
                return;
            }

            //using the userexists function from MongoDB service
            if (_mongoService.UserExists(username))
            {
                MessageBox.Show("Username already exists!\nRegister with a different username");
                return;
            }

            //First creating user account for MongoDB table
            //used for user authentication and role based access
            var newUser = new User
            {
                Username = username,
                Password = password,
                Email = email,
                Role = role
            };

            _mongoService.InsertUser(newUser);

            //create corresponding SQL record based on role
            //MongoDB UserId is stored in SQL to link the profile between both databases
            //helps map logged in mongoDB user to SQL record

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

            } else
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

                //Creating record in SQL table dependent on role
                //using SQL service methods
                MessageBox.Show("Registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);



        }

        //dynamically displays fields based on selected role
        //phone number is patient specific, department is staff specific
        //other fields in form are shared across all roles
        private void combobox_role_SelectedIndexChanged(object sender, EventArgs e)
        {
            string role = comboBoxRole.Text.ToLower();

            // shared fields always visible
            textBoxFirstName.Visible = true;
            textBoxLastName.Visible = true;

            // patient only
            textBoxPhone.Visible = role == "patient";
            labelPhone.Visible = role == "patient";

            // staff only
            textBoxDepartment.Visible = role == "doctor" || role == "nurse" || role == "admin";
            labelDepartment.Visible = role == "doctor" || role == "nurse" || role == "admin";
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {

            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Please fill out username and password fields!",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = _mongoService.GetUser(username, password);

            //map mongoDB UserId to SQL PatientId/StaffId
            //stored in session so forms can query SQL records without repeated lookups
            if (user != null)
            {
                UserSession.CurrentUser = user;

                SQLService sqlService = new SQLService();

                if (user.Role == "patient")
                {
                    UserSession.CurrentPatientId = sqlService.GetPatientIdByUserId(user.UserId);
                }
                else
                {
                    UserSession.CurrentStaffId = sqlService.GetStaffIdByUserId(user.UserId);
                }

                DashboardForm dashboard = new DashboardForm();
                dashboard.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid credentials.",
                    "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
