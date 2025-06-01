namespace MedicalID.Backend.Dtos.Patient
{
    public class PatientUpdateDto
    {
        public string FName { get; set; }
        public string LName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string BloodType { get; set; }

        public string EmergencyContact { get; set; }
        public bool OrganDonorStatus { get; set; }

        public int RegionID { get; set; }
    }

}
