using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    //public class User : IdentityUser
    public class User
    {
        [Key]  //primary key
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [Required]
        public string Cin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool Access { get; set; } = true;

        #region Role
        //public Guid FkRole { get; set; }
        public Guid? FkRole { get; set; }   // nullable foreign key
        public virtual Role Role { get; set; }
        #endregion

        #region Filiale
        public Guid? FkFiliale { get; set; }
        public virtual Filiale Filiale { get; set; }
        #endregion

        //public User()
        //{
        //    Access = true;
        //}
    }
}
