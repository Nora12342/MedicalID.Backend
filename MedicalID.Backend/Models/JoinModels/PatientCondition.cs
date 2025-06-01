namespace MedicalID.Backend.Models.JoinModels
{
    public class PatientCondition
    {
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        public int ConditionID { get; set; }
        public MedicalCondition Condition { get; set; }
        public string Note { get; set; }
    }

}
