using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HospitalClient.Forms.ChatForm;

namespace HospitalClient.Services
{
    //service used for SQL db operations
    //stores patients, staff, appointments, inventory, chat messages
    //used to query to for analytics data also
    //all user registration and authentication handled with MongoDB
    public class SQLService
    {
        string connectionString = "Data Source=DESKTOP-GO1R8A8\\SQLEXPRESS;Initial Catalog=HospitalManagement;Integrated Security=True;Encrypt=False;";

        //Create new patient in SQL table
        //registration only captures these fields
        //additional fields are filled in by stagg through patient management form
        public void InsertPatient(Patient patient)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Patient (FirstName, LastName, Phone, Email, UserId) " +
                    "VALUES (@FirstName, @LastName, @Phone, @Email, @UserId)", conn);
                cmd.Parameters.AddWithValue("@FirstName", patient.FirstName);
                cmd.Parameters.AddWithValue("@LastName", patient.LastName);
                cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                // UserId is the MongoDB ObjectId stored as a string
                // links this SQL record back to the MongoDB user for session mapping
                cmd.Parameters.AddWithValue("@UserId", patient.UserId);
                cmd.ExecuteNonQuery();
            }
        }

        //Create new Staff in SQL table
        //role is duplicated from mongodb into SQL so patients will be able to filter between
        //available doctors when scheduling appointments
        public void InsertStaff(Staff staff)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Staff (FirstName, LastName, Department, Role, UserId) " +
                    "VALUES (@FirstName, @LastName, @Department, @Role, @UserId)", conn);
                cmd.Parameters.AddWithValue("@FirstName", staff.FirstName);
                cmd.Parameters.AddWithValue("@LastName", staff.LastName);
                cmd.Parameters.AddWithValue("@Department", staff.Department);
                cmd.Parameters.AddWithValue("@Role", staff.Role);
                // UserId is the MongoDB ObjectId stored as a string
                // links this SQL record back to the MongoDB user for session mapping
                cmd.Parameters.AddWithValue("@UserId", staff.UserId);
                cmd.ExecuteNonQuery();
            }
        }


        //following two methods used once at login to capture Id, allowing forms to populate with
        //patient/staff information without the need to query mongodb again

        //mapping MongoDB objectID to SQL patientID
        public int GetPatientIdByUserId(string userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT PatientId FROM Patient WHERE UserId = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                //ExecuteScalar used for only a single digit being returned
                //closes connection immediately after returning the value
                //smaller network payload in comparison to ExecuteReader
                return (int)cmd.ExecuteScalar();
            }
        }

        //mapping MongoDB objectID to SQL StaffID
        public int GetStaffIdByUserId(string userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT StaffId FROM Staff WHERE UserId = @UserId", conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                return (int)cmd.ExecuteScalar();
            }
        }

        //SQL helper method for chat functionality
        //returns all patients that staff can message
        public List<UserList> GetAllPatientUsers()
        {
            var users = new List<UserList>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT UserId, FirstName + ' ' + LastName AS DisplayName FROM Patient", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    users.Add(new UserList { UserId = reader["UserId"].ToString(), DisplayName = reader["DisplayName"].ToString() });
            }
            return users;
        }

        //SQL helper method for chat functionality
        //returns all available doctors and nurses
        public List<UserList> GetAllStaffUsers()
        {
            var users = new List<UserList>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT UserId, FirstName + ' ' + LastName AS DisplayName FROM Staff WHERE Role IN ('Doctor', 'Nurse')", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    users.Add(new UserList { UserId = reader["UserId"].ToString(), DisplayName = reader["DisplayName"].ToString() });
            }
            return users;
        }

        public void SaveChatMessage(string senderUserId, string receiverUserId, string message)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO ChatMessage (SenderUserId, ReceiverUserId, Message, SentAt)
            VALUES (@Sender, @Receiver, @Message, @SentAt)", conn);
                cmd.Parameters.AddWithValue("@Sender", senderUserId);
                cmd.Parameters.AddWithValue("@Receiver", receiverUserId);
                cmd.Parameters.AddWithValue("@Message", message);
                cmd.Parameters.AddWithValue("@SentAt", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
