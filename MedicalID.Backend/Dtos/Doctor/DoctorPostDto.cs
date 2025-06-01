namespace MedicalID.Backend.Dtos.Doctor
{
    public class DoctorPostDto
    {
        public string DoctorID { get; set; }
        public string ReferenceID { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }
        public int SpecializationID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RegionID { get; set; }
        
    }
}
