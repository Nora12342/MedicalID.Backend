namespace MedicalID.Backend.Dtos
{
    public class PatientRegisterDto
    {
        public string PatientID { get; set; }
        public string MedicalID { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string BloodType { get; set; }

        public string EmergencyContact { get; set; }
        public bool OrganDonorStatus { get; set; }

        public int RegionID { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
    }

}
