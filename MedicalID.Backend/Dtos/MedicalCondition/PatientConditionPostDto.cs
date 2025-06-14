namespace MedicalID.Backend.Dtos.MedicalCondition
{
    public class PatientConditionPostDto
    {
        public string ConditionName { get; set; } 
        public string Description { get; set; } 
        public DateTime? DiagnosedDate { get; set; } 
        public string? Note { get; set; }

    }
}
