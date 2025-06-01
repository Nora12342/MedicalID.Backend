namespace MedicalID.Backend.Dtos.Region
{
    public class RegionGetDto
    {
        public int RegionID { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }

        public List<string> Hospitals { get; set; }
        public int PatientsCount { get; set; }
    }
}
