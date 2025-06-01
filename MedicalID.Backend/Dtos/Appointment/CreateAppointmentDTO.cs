namespace MedicalID.Backend.Dtos.Appointment
{
    public class CreateAppointmentDTO
    {
        public int PatientID { get; set; }
        public string DoctorID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentType { get; set; }
        public string? Notes { get; set; }
    }
}
