using Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DataTransferObjects
{
    public class RoleDTO
    {
        public  Guid IdRole { get; set; }
        [Required(ErrorMessage = "Role name is required")]
        [EnumDataType(typeof(TypeRole), ErrorMessage = "Role can only be 'User', 'Admin', or 'SuperAdmin'")]
        public string NomRole { get; set; }
    }
}
