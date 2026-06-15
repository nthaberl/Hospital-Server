using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace HospitalClient.Models
{
    [Serializable]
    public class User
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("username"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Username { get; set; }

        [BsonElement("password"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Password { get; set; }

        [BsonElement("email"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Email { get; set; }

        [BsonElement("role"), BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Role { get; set; }
    }
}