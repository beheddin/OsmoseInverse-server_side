using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public enum RoleType
    {
        User,
        Admin,
        SuperAdmin
    }

    public class Role
    {
        [Key]  //primary key
        public Guid RoleId { get; set; }

        [Required]
        //[RegularExpression("^(User|Admin|SuperAdmin)$", ErrorMessage = "Role can only be 'User' or 'Admin' or 'SuperAdmin'")]
        //public string RoleLabel { get; set; }
        [EnumDataType(typeof(RoleType), ErrorMessage = "Role  can only be 'User' or 'Admin' or 'SuperAdmin'")]
        public RoleType RoleLabel { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
