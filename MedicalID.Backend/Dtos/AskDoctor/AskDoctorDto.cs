namespace MedicalID.Backend.Dtos.AskDoctor
{
    public class AskDoctorDto
    {
        public int MessageID { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? RepliedAt { get; set; }

        public string DoctorName { get; set; }  // Flat instead of full object
        public string PatientName { get; set; }
    }
}
