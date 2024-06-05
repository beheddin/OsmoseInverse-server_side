using System.ComponentModel.DataAnnotations;

namespace Domain.DataTransferObjects
{
    public class LoginDTO
    {
        public string Cin { get; set; }
        public string Password { get; set; }
    }
}