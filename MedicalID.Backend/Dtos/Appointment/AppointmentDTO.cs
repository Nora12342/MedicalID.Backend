namespace MedicalID.Backend.Dtos.Appointment
{
    public class AppointmentDTO
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string DoctorID { get; set; }
        public string? PatientName { get; set; } // Add this
        public string? DoctorName { get; set; } // Add this
        public DateTime AppointmentDate { get; set; }
        public string AppointmentType { get; set; }
        public string Status { get; set; }
        public string? Notes { get; set; }
    }
}
