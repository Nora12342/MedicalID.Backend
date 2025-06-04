using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalID.Backend.Models
{
    public class Region
    {
        [Key]
        public int RegionID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        public int CityID { get; set; }
        public City City { get; set; }


        public ICollection<Doctor> Doctors { get; set; }
        public ICollection<Patient> Patients { get; set; }

        public ICollection<Hospital> Hospitals { get; set; } // ✅ Required for relationship
        
    }


}

