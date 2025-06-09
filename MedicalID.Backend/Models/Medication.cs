using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class Medication
    {
        [Key]
        public int MedicationID { get; set; }
        public int PatientID { get; set; }
        [Required]
        public string MedicationName { get; set; }
        [Required]
        public string Dosage { get; set; }
        [Required]
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }

        public string? Note { get; set; }  // ✅ Make this nullable

        
    }
}
