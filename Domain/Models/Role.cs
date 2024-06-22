using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public enum TypeRole
    {
        SuperAdmin,
        Admin,
        User
    }

    public class Role
    {
        [Key]
        public  Guid IdRole { get; set; }

        [Required]
        [EnumDataType(typeof(TypeRole), ErrorMessage = "Role can only be 'User' or 'Admin' or 'SuperAdmin'")]
        public TypeRole NomRole { get; set; }

        public virtual ICollection<Compte> Comptes { get; set; }
    }
}
