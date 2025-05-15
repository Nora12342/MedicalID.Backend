namespace MedicalID.Backend.Dtos
{
    public class LoginResponseDto
    {
        public string UserId { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
