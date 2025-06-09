using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedicalID.Backend.Models;

namespace MedicalID.Backend.Models
{
    public class RecordHistory
    {
        [Key]
        public int RecordHistoryID { get; set; }

        [Required]
        [StringLength(7)]
        public string MedicalID { get; set; }

        [Required]
        [StringLength(14)]
        public string DoctorID { get; set; } = string.Empty; // ✅ still string

        [Required]
        public int LogID { get; set; }

        [Required]
        public string DiagnosisNotes { get; set; } = string.Empty;

        [Required]
        public string TreatmentPlan { get; set; } = string.Empty;

        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }

        public string? Surgery { get; set; }
        public string? SurgeryNote { get; set; }

        // Navigation properties
        [ForeignKey(nameof(MedicalID))]
        public Patient? Patient { get; set; }

        [ForeignKey(nameof(DoctorID))]
        public Doctor? Doctor { get; set; }

        [ForeignKey(nameof(LogID))]
        public AccessLog? AccessLog { get; set; }

        public List<RecordHistoryFile>? Files { get; set; }
    }



}

