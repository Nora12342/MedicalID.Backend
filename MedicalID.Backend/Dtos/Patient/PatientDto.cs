using MedicalID.Backend.Dtos.MedicalCondition;

namespace MedicalID.Backend.Dtos.Patient
{
    public class PatientDto
    {
        public int ID { get; set; } // New primary key

        public string PatientID { get; set; } // National ID (14 digits)
        public string MedicalID { get; set; } // Last 7 digits

        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContact { get; set; }
        public bool OrganDonorStatus { get; set; }

        public int RegionID { get; set; }
        public string RegionName { get; set; } // Optional if you include region details

        // Optional summaries
        public List<MedicalConditionDto> MedicalConditions { get; set; }
        public List<AllergyDto> Allergies { get; set; }
        public List<MedicationDto> Medications { get; set; }
    }

}
