namespace MedicalID.Backend.Dtos.RecordHistory
{
    public class RecordHistoryPostDto
    {
        public string PatientID { get; set; }
        public string DoctorID { get; set; }
        public int AccessLogID { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
