namespace MedicalID.Backend.Dtos.RecordHistory
{
    public class RecordHistoryPostDto
    {
        public int PatientID { get; set; }
        public string DoctorID { get; set; }
        public int LogID { get; set; }
        public string DiagnosisNotes { get; set; }
        public string TreatmentPlan { get; set; }
        public string? Surgery { get; set; }
        public string? SurgeryNote { get; set; }
    }
}
