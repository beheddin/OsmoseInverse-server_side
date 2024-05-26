using System.ComponentModel.DataAnnotations;

namespace Domain.DataTransferObjects
{
    public class LoginDTO
    {
        public string Cin { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}