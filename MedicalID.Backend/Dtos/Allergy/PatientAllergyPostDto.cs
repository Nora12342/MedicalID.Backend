namespace MedicalID.Backend.Dtos.Allergy
{
    public class PatientAllergyPostDto
    {
        public int PatientID { get; set; }
        public int AllergyID { get; set; }
        public string Note { get; set; }
    }
}
