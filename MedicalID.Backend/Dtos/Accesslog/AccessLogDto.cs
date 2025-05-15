namespace MedicalID.Backend.Dtos.Accesslog
{
    public class AccessLogDto
    {
        public int LogID { get; set; }
        public DateTime AccessTime { get; set; }
        public string Purpose { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
    }
}
