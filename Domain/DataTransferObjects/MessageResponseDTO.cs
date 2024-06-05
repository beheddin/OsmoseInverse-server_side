namespace Domain.DataTransferObjects
{
    public class MessageResponseDTO
    {
        public bool IsSuccessful { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
