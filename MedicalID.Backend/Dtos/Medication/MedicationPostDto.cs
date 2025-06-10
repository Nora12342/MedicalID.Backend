namespace MedicalID.Backend.Dtos.Medication
{
    public class MedicationPostDto
    {
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }
        public string? Note { get; set; }
    }
}
