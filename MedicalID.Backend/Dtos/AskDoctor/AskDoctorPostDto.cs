namespace MedicalID.Backend.Dtos.AskDoctor
{
    public class AskDoctorPostDto
    {
        public string DoctorID { get; set; }
        public int PatientID { get; set; }

        public int PID { get; set; }
        public string Question { get; set; }
        public bool IsPaid { get; set; }
        public decimal AmountPaid { get; set; }
        public string? UpiRef { get; set; }

        public string Subject { get; set; }
    }
}
