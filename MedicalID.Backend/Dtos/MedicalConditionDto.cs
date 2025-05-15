namespace MedicalID.Backend.Dtos
{
    public class MedicalConditionDto
    {
        public int MedConditionID { get; set; }
        public string ConditionName { get; set; }
        public string Description { get; set; }
        public DateTime DiagnosedDate { get; set; }
        public string PatientID { get;  set; }
    }
}
