namespace MedicalID.Backend.Models
{
    public class Allergy
    {
        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        public string Allergen { get; set; }
        public string Reaction { get; set; }
        public string Severity { get; set; }
    }
}
