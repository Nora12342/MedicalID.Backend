namespace MedicalID.Backend.Dtos.Doctor
{
    public class DoctorUpdateDto
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public int SpecializationID { get; set; }
        public int RegionID { get; set; }
    }
}
