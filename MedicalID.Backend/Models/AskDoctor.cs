namespace MedicalID.Backend.Models
{
    public class AskDoctor
    {
        public int MessageID { get; set; }

        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        public string DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public string Question { get; set; }
        public string Response { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? RepliedAt { get; set; }
        public string Answer { get;  set; }
    }
}
