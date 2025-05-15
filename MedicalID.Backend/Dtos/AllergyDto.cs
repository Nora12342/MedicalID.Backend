namespace MedicalID.Backend.Dtos
{
    public class AllergyDto
    {
        public string Allergen { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
        public string PatientId { get; set; }
    }
}
