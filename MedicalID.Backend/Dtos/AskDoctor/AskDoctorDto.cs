namespace MedicalID.Backend.Dtos.AskDoctor
{
    public class AskDoctorDto
    {
        public int MessageID { get; set; }

        public string Subject { get; set; }

        public string Question { get; set; }
        public string Response { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? RepliedAt { get; set; }
        public string DoctorName { get; set; } 
        public string PatientName { get; set; }

        public string? UpiRef { get; set; }
    }
}
