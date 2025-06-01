using System.ComponentModel.DataAnnotations;
using MedicalID.Backend.Models.JoinModels;

namespace MedicalID.Backend.Models
{
    public class Patient
    {
        [Key]
        public int ID { get; set; } // New auto-generated primary key

        [Required]
        [StringLength(14)]
        public string PatientID { get; set; } // National ID (unique)

        [Required]
        [StringLength(7)]
        public string MedicalID { get; set; } // Last 7 digits of National ID (unique)

        public string FName { get; set; }
        public string LName { get; set; }

        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContact { get; set; }
        public bool OrganDonorStatus { get; set; }
        public string Gender { get; set; }

        public int RegionID { get; set; }
        public Region Region { get; set; }
        

        [Required]
        public string Email { get; set; }

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }

        // Navigations
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<RecordHistory> RecordHistories { get; set; }
        public ICollection<AccessLog> AccessLogs { get; set; }
        public ICollection<AskDoctor> AskDoctors { get; set; }

        // Many-to-many navigations
        public ICollection<PatientMedication> PatientMedications { get; set; }
        public ICollection<PatientCondition> PatientConditions { get; set; }
        public ICollection<PatientAllergy> PatientAllergies { get; set; }
    }

}
