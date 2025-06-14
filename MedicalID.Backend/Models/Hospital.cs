using System.ComponentModel.DataAnnotations;
using MedicalID.Backend.Models;

public class Hospital
{
    [Key]
    public int HospitalID { get; set; }
    [Required]
    public string HospitalName { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    public string ContactInformation { get; set; }

    public int RegionID { get; set; } 
    public Region Region { get; set; } 
}

