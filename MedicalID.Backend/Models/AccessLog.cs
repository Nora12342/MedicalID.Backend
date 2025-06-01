using System.ComponentModel.DataAnnotations;

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

        public DateTime AccessTime { get; set; }
        public string Purpose { get; set; }

        public string AccessStatus { get; set; }

        public bool AccessGranted { get; set; }
    }

}
