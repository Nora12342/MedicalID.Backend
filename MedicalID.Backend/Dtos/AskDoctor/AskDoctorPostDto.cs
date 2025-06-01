namespace MedicalID.Backend.Dtos.AskDoctor
{
    public class AskDoctorPostDto
    {
        public string DoctorID { get; set; }
        public string PatientID { get; set; }
        public string Question { get; set; }
      
        public DateTime SentAt { get; set; }
        public string Answer { get; set; }
    }
}
