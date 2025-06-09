using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class Doctor
    {
        [Key]
        [StringLength(14)]
        public string DoctorID { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "ReferenceID must be exactly 6 digits.")]
        public string ReferenceID { get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }

        public int SpecializationID { get; set; }
        [Required]
        public Specialization Specialization { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        public int RegionID { get; set; }
        public Region Region { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<RecordHistory> RecordHistories { get; set; }
        public ICollection<AccessLog> AccessLogs { get; set; }
        public ICollection<AskDoctor> AskDoctors { get; set; }
    }
}

