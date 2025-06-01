using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }
        public string CityName { get; set; }
        public ICollection<Region> Regions { get; set; }
        
    }
}

