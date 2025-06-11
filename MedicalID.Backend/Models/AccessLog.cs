using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalID.Backend.Models
{
    public class AccessLog
    {
        [Key]
        public int LogID { get; set; }

        public string DoctorID { get; set; }
        public Doctor Doctor { get; set; }


        [Required]
        [StringLength(7)]
        public string MedicalID { get; set; }

        [ForeignKey(nameof(MedicalID))]
        
        public Patient PatientByMedicalID { get; set; }

        public DateTime AccessTime { get; set; }

        public string Purpose { get; set; }

        public string AccessStatus { get; set; }

        public bool AccessGranted { get; set; }
    }



}
