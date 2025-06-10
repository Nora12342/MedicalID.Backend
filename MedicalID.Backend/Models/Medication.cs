using System.ComponentModel.DataAnnotations;
using MedicalID.Backend.Models.JoinModels;

namespace MedicalID.Backend.Models
{
    public class Medication
    {
        [Key]
        public int MedicationID { get; set; }
        [Required]
        public string MedicationName { get; set; }
        [Required]
        public string Dosage { get; set; }
        [Required]
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }
        public string? Note { get; set; }
        public ICollection<PatientMedication>? PatientMedications { get; set; }
    }
}
