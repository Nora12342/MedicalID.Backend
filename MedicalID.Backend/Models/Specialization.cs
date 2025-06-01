using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class Specialization
    {
        [Key]
        public int SpecializationID { get; set; }

        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}
