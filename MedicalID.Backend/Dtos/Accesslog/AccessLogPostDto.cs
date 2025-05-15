namespace MedicalID.Backend.Dtos.Accesslog
{
    public class AccessLogPostDto
    {
        public string DoctorID { get; set; }
        public string PatientID { get; set; }
        public DateTime AccessTime { get; set; }
        public string Purpose { get; set; }
    }
}
