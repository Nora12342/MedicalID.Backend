namespace MedicalID.Backend.Dtos.Medication
{
    public class MedicationPostDto
    {
        public int PatientID { get; set; } // Required to create the M:N link

        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public DateTime? PrescribedDate { get; set; }
    }
}
