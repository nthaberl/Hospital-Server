using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalClient.Services
{
    // bridge between MongoDB and SQL databases
    // used to look up a patient's MongoDB UserId from their SQL PatientId
    // used for targeting SignalR notifications to the correct user
    public class DataIntegrator
    {
        SQLService _sqlService = new SQLService();
        MongoDBService _mongoService = new MongoDBService();

    }
}
