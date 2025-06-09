using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class AskDoctor
    {
        [Key]
        [Required]
        public int MessageID { get; set; }
        [Required]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        public string DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        public string Subject { get; set; }
        [Required]

        public string MessageContent { get; set; }

        public bool IsRead { get; set; }
        public string? ResponseContent { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? RepliedAt { get; set; }

        public bool IsPaid { get; set; }
        public decimal AmountPaid { get; set; } = 50;
        public string? UpiRef { get; set; }
    }
}
