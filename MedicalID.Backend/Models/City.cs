namespace MedicalID.Backend.Models
{
    public class City
    {
        public string CityID { get; set; }
        public string Name { get; set; }

        public ICollection<Region> Regions { get; set; }

    }
}
