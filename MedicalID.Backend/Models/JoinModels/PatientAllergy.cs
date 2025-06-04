namespace MedicalID.Backend.Models.JoinModels
{
    public class PatientAllergy
    {
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        public int AllergyID { get; set; }
        public Allergy Allergy { get; set; }
        public string Note { get; set; }
    }

}
