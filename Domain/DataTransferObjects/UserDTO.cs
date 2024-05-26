using Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DataTransferObjects
{
    public class UserDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Cin { get; set; }

        //[JsonIgnore]    //hide pwd in JSON response
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //public RoleType RoleLabel { get; set; }
        public string RoleLabel { get; set; }
        public string FilialeLabel { get; set; }
    }
}
