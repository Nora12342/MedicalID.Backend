namespace MedicalID.Backend.Dtos.Doctor
{
    public class DoctorPostDto
    {
        public string DoctorID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }
        public int RegionID { get; set; }
        public int Phone { get; set; }
    }
}
