using System.ComponentModel.DataAnnotations;
using MedicalID.Backend.Models;

public class Hospital
{
    [Key]
    public int HospitalID { get; set; }
    public string HospitalName { get; set; }
    public string Type { get; set; }
    public string ContactInformation { get; set; }

    public int RegionID { get; set; } // ✅ must be int
    public Region Region { get; set; } // ✅ navigation
}

