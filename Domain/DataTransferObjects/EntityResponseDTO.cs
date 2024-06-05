namespace Domain.DataTransferObjects
{
    public class EntityResponseDTO<T>
    {
        public bool IsSuccessful { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public T Entity { get; set; } = default;
    }
}
