namespace MedicalID.Backend.Dtos
{
    public class RegionGetDto
    {
        public string RegionID { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }

        public List<string> Hospitals { get; set; }
        public int PatientsCount { get; set; }
    }
}
