namespace MedicalID.Backend.Dtos.Medication
{
    public class MedicationUpdateDTO
    {
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public DateTime? PrescribedDate { get; set; }
    }
}
