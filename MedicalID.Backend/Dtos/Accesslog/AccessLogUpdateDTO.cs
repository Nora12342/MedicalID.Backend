namespace MedicalID.Backend.Dtos.Accesslog
{
    public class AccessLogUpdateDTO
    {
        public bool AccessGranted { get; set; }
        public string AccessStatus { get; set; } = "Pending";
        public int PatientId { get; set; }
    }
}
