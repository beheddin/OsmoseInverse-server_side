using Domain.Models;
using System;

namespace Domain.DataTransferObjects
{
    public class RoleDTO
    {
        public  Guid IdRole { get; set; }
        public string NomRole { get; set; }
    }
}
