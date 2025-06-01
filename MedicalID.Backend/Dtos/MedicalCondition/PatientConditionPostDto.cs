namespace MedicalID.Backend.Dtos.MedicalCondition
{
    public class PatientConditionPostDto
    {
        public int PatientID { get; set; }
        public int ConditionID { get; set; }
        public string Note { get; set; }
    }
}
