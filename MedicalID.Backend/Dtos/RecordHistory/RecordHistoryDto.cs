namespace MedicalID.Backend.Dtos.RecordHistory
{
    public class RecordHistoryDTO
    {
        public int RecordHistoryID { get; set; } 
        public string MedicalID { get; set; }
        public string DoctorID { get; set; }
        public int LogID { get; set; }
        public string DiagnosisNotes { get; set; }
        public string TreatmentPlan { get; set; }
        public DateTime CreateTime { get; set; } 
        public DateTime? UpdateTime { get; set; } 
        public string? Surgery { get; set; } 
        public string? SurgeryNote { get; set; } 
        public string? DoctorName { get; set; }
    }

}
