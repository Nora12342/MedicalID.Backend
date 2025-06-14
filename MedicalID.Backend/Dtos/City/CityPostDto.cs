using MedicalID.Backend.Dtos.Region;

namespace MedicalID.Backend.Dtos.City
{
    public class CityPostDto
    {
        public string CityName { get; set; } = string.Empty;

        public List<string>? RegionNames { get; set; }

    }
}
