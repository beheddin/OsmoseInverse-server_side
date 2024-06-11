using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public enum RoleType
    {
        SuperAdmin,
        Admin,
        Utilisateur
    }

    public class Role
    {
        [Required]
        [Key]
        public  Guid IdRole { get; set; }

        [Required]
        [EnumDataType(typeof(RoleType), ErrorMessage = "Role  can only be 'Utilisateur' or 'Admin' or 'SuperAdmin'")]
        public RoleType NomRole { get; set; }

        public virtual ICollection<Compte> Comptes { get; set; }
    }
}
