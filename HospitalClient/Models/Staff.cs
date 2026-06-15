using System.Collections;
using System.Collections.Generic;

namespace HospitalClient
{
    public class Staff
    {
        public int StaffId {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }


        public ICollection<Appointment> Appointments { get; set; }
    }
}