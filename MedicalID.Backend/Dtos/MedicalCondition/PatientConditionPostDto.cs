namespace MedicalID.Backend.Dtos.MedicalCondition
{
    public class PatientConditionPostDto
    {
        public string ConditionName { get; set; } // Change from ConditionID to ConditionName
        public string Description { get; set; } // You'll likely need Description to create a new condition
        public DateTime? DiagnosedDate { get; set; } // And DiagnosedDate if it's part of the new condition
        public string? Note { get; set; }

    }
}
