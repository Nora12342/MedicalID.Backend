using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class Medication
    {
        [Key]
        public int MedicationID { get; set; }
        public string PatientID { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }

        public string? Note { get; set; }  // ✅ Make this nullable

        public Patient Patient { get; set; }
    }
}
