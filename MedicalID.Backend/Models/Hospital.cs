namespace MedicalID.Backend.Models
{
    public class Hospital
    {
        public string HospitalID { get; set; }
        public string Name { get; set; }

        public string RegionID { get; set; }
        public Region Region { get; set; }
    }
}
