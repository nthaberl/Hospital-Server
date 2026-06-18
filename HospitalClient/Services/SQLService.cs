using HospitalClient.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HospitalClient.Forms.ChatForm;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;

namespace HospitalClient.Services
{
    //service used for SQL db operations
    //stores patients, staff, appointments, inventory, chat messages
    //used to query to for analytics data also
    //all user registration and authentication handled with MongoDB
    public class SQLService
    {
        //Dylan's SQL connection string
        //string connectionString = "Data Source=DYLAN-LAPTOP\\SQLEXPRESS;Initial Catalog=HospitalManagement;Integrated Security=True;Encrypt=False;";

        //Vitoria's SQL connection string
        //string connectionString = "Data Source=RESENDE\\SQLEXPRESS;Initial Catalog=HospitalManagement;Integrated Security=True;Encrypt=False;";

        //Natascha's SQL connection string 
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

        // inventory methods

        // get list of items
        public List<InventoryItem> GetInventoryItems()
        {
            List<InventoryItem> items = new List<InventoryItem>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT ItemId, Name, Category, QtyInStock, ReorderThreshold " +
                    "FROM Inventory ORDER BY Category, Name", conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new InventoryItem
                        {
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Name = reader["Name"].ToString(),
                            Category = reader["Category"].ToString(),
                            QtyInStock = Convert.ToInt32(reader["QtyInStock"]),
                            ReorderThreshold = Convert.ToInt32(reader["ReorderThreshold"])
                        });
                    }
                }
            }

            return items;
        }

        // get one item 
        // pass in id return item
        public InventoryItem GetInventoryItemById(int itemId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT ItemId, Name, Category, QtyInStock, ReorderThreshold " +
                    "FROM Inventory WHERE ItemId = @ItemId", conn);

                cmd.Parameters.AddWithValue("@ItemId", itemId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new InventoryItem
                        {
                            ItemId = Convert.ToInt32(reader["ItemId"]),
                            Name = reader["Name"].ToString(),
                            Category = reader["Category"].ToString(),
                            QtyInStock = Convert.ToInt32(reader["QtyInStock"]),
                            ReorderThreshold = Convert.ToInt32(reader["ReorderThreshold"])
                        };
                    }
                }
            }

            return null;
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

        public DataTable GetAllPatients()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT PatientId, FirstName, LastName, Phone, Email, UserId FROM Patient", conn);

                DataTable table = new DataTable();
                adapter.Fill(table);

                return table;
            }
        }

        // Updates selected patient information
        public void UpdatePatient(Patient patient)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "UPDATE Patient SET FirstName=@FirstName, LastName=@LastName, Phone=@Phone, Email=@Email WHERE PatientId=@PatientId", conn);

                cmd.Parameters.AddWithValue("@PatientId", patient.PatientId);
                cmd.Parameters.AddWithValue("@FirstName", patient.FirstName);
                cmd.Parameters.AddWithValue("@LastName", patient.LastName);
                cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                cmd.Parameters.AddWithValue("@Email", patient.Email);

                int rows = cmd.ExecuteNonQuery();

                MessageBox.Show("Rows updated: " + rows);
            }
        }
        

        // insert item, returns item inserted
        public InventoryItem InsertInventoryItem(InventoryItem item)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Inventory (Name, Category, QtyInStock, ReorderThreshold) " +
                    "OUTPUT INSERTED.ItemId " +
                    "VALUES (@Name, @Category, @QtyInStock, @ReorderThreshold)", conn);

                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Category", item.Category);
                cmd.Parameters.AddWithValue("@QtyInStock", item.QtyInStock);
                cmd.Parameters.AddWithValue("@ReorderThreshold", item.ReorderThreshold);

                item.ItemId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return item;
        }

        // 
        public void UpdateInventoryItem(InventoryItem item)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "UPDATE Inventory SET Name = @Name, Category = @Category, " +
                    "QtyInStock = @QtyInStock, ReorderThreshold = @ReorderThreshold " +
                    "WHERE ItemId = @ItemId", conn);

                cmd.Parameters.AddWithValue("@ItemId", item.ItemId);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Category", item.Category);
                cmd.Parameters.AddWithValue("@QtyInStock", item.QtyInStock);
                cmd.Parameters.AddWithValue("@ReorderThreshold", item.ReorderThreshold);

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateInventoryQuantity(int itemId, int qtyInStock)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "UPDATE Inventory SET QtyInStock = @QtyInStock WHERE ItemId = @ItemId", conn);

                cmd.Parameters.AddWithValue("@ItemId", itemId);
                cmd.Parameters.AddWithValue("@QtyInStock", qtyInStock);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteInventoryItem(int itemId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Inventory WHERE ItemId = @ItemId", conn);

                cmd.Parameters.AddWithValue("@ItemId", itemId);

                cmd.ExecuteNonQuery();
            }
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

            }

        }

        // Deletes selected patient from SQL table
        public void DeletePatient(int patientId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Patient WHERE PatientId=@PatientId", conn);

                cmd.Parameters.AddWithValue("@PatientId", patientId);

                cmd.ExecuteNonQuery();
            }
        }

        // Counts total patient visits using the Appointment table
        public int GetTotalPatientVisits()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Appointment", conn);

                return (int)cmd.ExecuteScalar();
            }
        }

        // Finds the most common patient concern/ailment
        public string GetMostCommonConcern()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "SELECT TOP 1 Concern FROM Appointment WHERE Concern IS NOT NULL GROUP BY Concern ORDER BY COUNT(*) DESC", conn);

                object result = cmd.ExecuteScalar();

                return result == null ? "N/A" : result.ToString();
            }
        }

        // Shows appointment totals grouped by status
        public DataTable GetAppointmentStatusReport()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT Status, COUNT(*) AS Total FROM Appointment GROUP BY Status", conn);

                DataTable table = new DataTable();
                adapter.Fill(table);

                return table;
            }
        }

        // Shows medication/inventory report
        public DataTable GetMedicationUsageReport()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(
                    "SELECT Name, Category, QuantityInStock FROM InventoryItem ORDER BY QuantityInStock ASC", conn);

                DataTable table = new DataTable();
                adapter.Fill(table);

                return table;
            }
        }
    }
}
