namespace MedicalID.Backend.Dtos.Allergy
{
    public class PatientAllergyDto
    {
        public int AllergyID { get; set; }
        public string Allergen { get; set; }
        public string Severity { get; set; }
        public string Reaction { get; set; }
        public string Note { get; set; }
    }
}
