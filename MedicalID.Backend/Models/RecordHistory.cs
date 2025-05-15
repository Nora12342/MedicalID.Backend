namespace MedicalID.Backend.Models
{
    public class RecordHistory
    {
        public int RecordID { get; set; }

        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        public string DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public int? AccessLogID { get; set; }
        public AccessLog AccessLog { get; set; }

        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
