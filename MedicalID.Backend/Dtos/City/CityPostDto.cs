using MedicalID.Backend.Dtos.Region;

namespace MedicalID.Backend.Dtos.City
{
    public class CityPostDto
    {
        public string CityName { get; set; } = string.Empty;

        // Simple list of region names to create with the city
        public List<string>? RegionNames { get; set; }

    }
}
