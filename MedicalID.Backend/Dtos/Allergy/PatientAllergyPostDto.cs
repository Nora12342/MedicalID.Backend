namespace MedicalID.Backend.Dtos.Allergy
{
    public class PatientAllergyPostDto
    {
        public int PatientID { get; set; }
        public string Allergen { get; set; }
        public string Note { get; set; }
    }
}
