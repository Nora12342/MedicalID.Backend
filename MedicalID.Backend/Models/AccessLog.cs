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

        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        [Required]
        [StringLength(7)] // Make sure this matches the actual MedicalID length in the Patients table
        public string MedicalID { get; set; }

        //[ForeignKey(nameof(MedicalID))]
        //public Patient PatientByMedicalID { get; set; }

        public DateTime AccessTime { get; set; }

        public string Purpose { get; set; }

        public string AccessStatus { get; set; }

        public bool AccessGranted { get; set; }
    }


}
