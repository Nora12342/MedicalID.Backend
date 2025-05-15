namespace MedicalID.Backend.Models
{
    public class Medication
    {
        public int MedicationID { get; set; }

        public string PatientID { get; set; }
        public Patient Patient { get; set; }

        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }
    }
}
