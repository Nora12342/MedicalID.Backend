using System.ComponentModel.DataAnnotations;
using MedicalID.Backend.Models;
using MedicalID.Backend.Models.JoinModels;

public class Allergy
{
    [Key]
    public int AllergyID { get; set; } 

    public string Allergen { get; set; }
    public string Severity { get; set; }
    public string Reaction { get; set; }
    public string? Note { get; set; }

    public ICollection<PatientAllergy> PatientAllergies { get; set; }

}

