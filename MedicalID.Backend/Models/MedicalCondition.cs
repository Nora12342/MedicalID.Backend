namespace MedicalID.Backend.Models
{
    public class MedicalCondition
    {
        public int MedConditionID { get; set; }
        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        public string ConditionName { get; set; }
        public string Description { get; set; }
        public DateTime? DiagnosedDate { get; set; }
    }
}
