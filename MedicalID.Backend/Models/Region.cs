namespace MedicalID.Backend.Models
{
    public class Region
    {
        public string RegionID { get; set; }
        public string Name { get; set; }

        public string CityID { get; set; }
        public City City { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Patient> Patients { get; set; }
        public ICollection<Hospital> Hospitals { get; set; }
    }
}
