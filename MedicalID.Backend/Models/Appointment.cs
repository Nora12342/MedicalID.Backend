using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public string DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }
        public string? Notes { get; set; }
        public string AppointmentType { get; set; } // e.g., Clinic, Emergency
        public string Status { get; set; } // Scheduled, Completed, Cancelled

    }

}
