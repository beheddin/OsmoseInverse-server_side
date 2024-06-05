namespace Domain.DataTransferObjects
{
    public class LoginResponseDTO
    {
        public bool IsSuccessful { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
