namespace MedicalID.Backend.Dtos.RecordHistory
{
    public class RecordHistoryDto
    {
        public int RecordID { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
    }
}
