namespace MedicalID.Backend.Dtos.RecordHistory
{
    public class RecordHistoryDTO
    {
        public int RecordHistoryID { get; set; } // Add this for identifying the record
        public string MedicalID { get; set; }
        public string DoctorID { get; set; }
        public int LogID { get; set; }
        public string DiagnosisNotes { get; set; }
        public string TreatmentPlan { get; set; }
        public DateTime CreateTime { get; set; } // Add this
        public DateTime? UpdateTime { get; set; } // Add this
        public string? Surgery { get; set; } // Add this
        public string? SurgeryNote { get; set; } // Add this
        public string? DoctorName { get; set; }
    }

}
