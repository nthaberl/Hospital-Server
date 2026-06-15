using HospitalClient.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalClient.Services
{
    public class MongoDBService
    {
        //service that handles all MongoDB operations
        //MongoDB used exclusively for user authentication and role based access
        //patient and staff records stored in SQL server
        IMongoCollection<User> userCollection;

        public MongoDBService()
        {
            //connecting to MongoDB instance
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("hospital");
            userCollection = database.GetCollection<User>("users");
        }

        //used to retrieve user by username && password
        public User GetUser(string username, string password)
        {
            return userCollection.Find(u => u.Username == username
                && u.Password == password).FirstOrDefault();
        }

        //inserting new user into users collection
        public void InsertUser(User user)
        {
            userCollection.InsertOne(user);
        }

        //used during registration to ensure duplicate users dont exist
        public bool UserExists(string username)
        {
            return userCollection.Find(u => u.Username == username).FirstOrDefault() != null;
        }
    }
}
