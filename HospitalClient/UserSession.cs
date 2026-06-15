using HospitalClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalClient
{
    namespace HospitalManagementSystem
    {
        //class used to store loggedin user information for application state
        public class UserSession
        {
            public static User CurrentUser { get; set; }
            public static int CurrentPatientId { get; set; }
            public static int CurrentStaffId { get; set; }
        }
    }
}
