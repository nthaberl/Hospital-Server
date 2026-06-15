using System;

namespace HospitalClient
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int StaffId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }
        public string Concern { get; set; }
        public string Notes { get; set; }

        public Patient Patient { get; set; }
        public Staff Staff { get; set; }
    }
}