using System.ComponentModel.DataAnnotations;

namespace MedicalID.Backend.Models
{
    public class Patient
    {
        [Key]
        [StringLength(14)]
        public string PatientID { get; set; }
        public string MedicalID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodType { get; set; }
        public string EmergencyContact { get; set; }
        public bool OrganDonorStatus { get; set; }

        // Optional navigation properties
        public ICollection<MedicalCondition>? MedicalConditions { get; set; }
        public ICollection<Allergy>? Allergies { get; set; }
        public ICollection<Medication>? Medications { get; set; }
        public ICollection<AccessLog>? AccessLogs { get; set; }
        public ICollection<RecordHistory>? RecordHistories { get; set; }
        public ICollection<AskDoctor>? AskDoctors { get; set; }
        

        public int RegionID { get; set; }

        public Region Region { get; set; }
        public string UserName { get;  set; }
        public object PasswordHash { get;  set; }

        public Patient()
        {
            MedicalConditions = new List<MedicalCondition>();
            Allergies = new List<Allergy>();
            Medications = new List<Medication>();
            AccessLogs = new List<AccessLog>();
            RecordHistories = new List<RecordHistory>();
            AskDoctors = new List<AskDoctor>();
        }
    }

}
