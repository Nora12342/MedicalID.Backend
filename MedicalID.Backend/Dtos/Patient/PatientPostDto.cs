namespace MedicalID.Backend.Dtos.Patient
{
    public class PatientPostDto
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

        public int RegionID { get; set; }
    }

}
