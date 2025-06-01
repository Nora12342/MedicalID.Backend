using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class Doctor
    {
        [Key]
        [StringLength(14)]
        public string DoctorID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }

        public int RegionID { get; set; }

        public Region Region { get; set; }

        public ICollection<AccessLog> AccessLogs { get; set; }
        public ICollection<RecordHistory> RecordHistories { get; set; }
        public ICollection<AskDoctor> AskDoctors { get; set; }
        public string UserName { get;  set; }
        public string PasswordHash { get; set; }

        public int Phone { get;  set; }
    }
}
