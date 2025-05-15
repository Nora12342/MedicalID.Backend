namespace MedicalID.Backend.Models
{
    public class AccessLog
    {
        public int LogID { get; set; } // This acts as the Primary Key
        public string DoctorID { get; set; }
        public string PatientID { get; set; }
        public DateTime AccessTime { get; set; }
        public string Purpose { get; set; }

        // Navigation properties (optional)
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
    }

}
