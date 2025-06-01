using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalID.Backend.Models
{
    public class RecordHistoryFile
    {
        [Key]
        public int FileID { get; set; }
        public int RecordHistoryID { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        [ForeignKey("RecordHistoryID")]
        public RecordHistory? RecordHistory { get; set; }
    }
}
