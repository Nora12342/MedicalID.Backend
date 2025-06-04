namespace MedicalID.Backend.Dtos.RecordHistory
{
    public class RecordHistoryDTO
    {
        public string MedicalID { get; set; }
        public string DoctorID { get; set; }
        public int LogID { get; set; }
        public string DiagnosisNotes { get; set; }
        public string TreatmentPlan { get; set; }
    }

}
