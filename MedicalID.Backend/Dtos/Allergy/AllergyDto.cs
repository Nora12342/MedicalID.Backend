namespace MedicalID.Backend.Dtos
{
    public class AllergyDto
    {
        public int AllergyID { get; set; }
        public string Allergen { get; set; }
        public string Severity { get; set; }
        public string Reaction { get; set; }
    }
}
