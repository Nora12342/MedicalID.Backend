namespace MedicalID.Backend.Dtos.Doctor
{
    public class DoctorDto
    {
        public string DoctorID { get; set; }
        public string ReferenceID { get; set; }

        public string FName { get; set; }
        public string LName { get; set; }
        public string SpecializationName { get; set; }
        public int SpecializationID { get; set; }
        public string Email { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get;  set; }
    }
}
