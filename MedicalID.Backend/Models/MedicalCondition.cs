using System.ComponentModel.DataAnnotations;
using MedicalID.Backend.Models;
using MedicalID.Backend.Models.JoinModels;

public class MedicalCondition
{
    [Key]
    public int ConditionID { get; set; }
    public string ConditionName { get; set; }
    public string Description { get; set; }
    public DateTime? DiagnosedDate { get; set; }
    public string Note { get; set; }

    public ICollection<PatientCondition> PatientConditions { get; set; }
}


