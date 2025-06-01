namespace MedicalID.Backend.Dtos
{
    public class DoctorRegisterDto
    {
        public string DoctorID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string ReferenceID { get; set; }
        public string Specialization { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RegionID { get; set; }
        public string Email { get; set; }
        public string Phone { get;  set; }
    }
}
