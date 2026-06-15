using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalClient
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Prescriptions { get; set; }
        public string Notes { get; set; }

        public ICollection<Appointment> Appointments { get; set;  }

    }
}
