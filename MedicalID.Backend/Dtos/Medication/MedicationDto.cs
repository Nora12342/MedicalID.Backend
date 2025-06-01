namespace MedicalID.Backend.Dtos
{
    public class MedicationDto
    {
        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }
    }
}
