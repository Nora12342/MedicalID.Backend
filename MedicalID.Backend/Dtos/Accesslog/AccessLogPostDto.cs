namespace MedicalID.Backend.Dtos.Accesslog
{
    public class AccessLogPostDto
    {
        public string DoctorID { get; set; }
        public int PatientID { get; set; }
        public string MedicalID { get; set; } // ✅ REQUIRED

        public DateTime AccessTime { get; set; }
        public string Purpose { get; set; }
        public string AccessStatus { get; set; }
        public bool AccessGranted { get; set; }
    }
}
