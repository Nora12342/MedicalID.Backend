namespace MedicalID.Backend.Dtos.Patient
{
    public class PatientDto
    {
        public string PatientID { get; set; }
        public string MedicalID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContact { get; set; }
        public bool OrganDonorStatus { get; set; }
       

        // Optional summaries (avoid full navigation property loops)
        public List<MedicalConditionDto> MedicalConditions { get; set; }
        public List<AllergyDto> Allergies { get; set; }
        public List<MedicationDto> Medications { get; set; }

        public string RegionName { get; set; }
    }
}
