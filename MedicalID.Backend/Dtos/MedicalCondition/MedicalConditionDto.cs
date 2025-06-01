namespace MedicalID.Backend.Dtos.MedicalCondition
{
    public class MedicalConditionDto
    {
        public int ConditionID { get; set; }
        public string ConditionName { get; set; }
        public string Description { get; set; }
        public DateTime? DiagnosedDate { get; set; }
        public string Note { get; set; }
    }
}
