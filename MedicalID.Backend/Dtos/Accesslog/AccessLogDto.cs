namespace MedicalID.Backend.Dtos.Accesslog
{
    public class AccessLogDto
    {
        public int LogID { get; set; }
        public string DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime AccessTime { get; set; }
        public string Purpose { get; set; }
        public string AccessStatus { get; set; }
        public bool AccessGranted { get; set; }
    }
}
