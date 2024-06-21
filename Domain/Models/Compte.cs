using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Compte
    {
        [Required]
        [Key]
        public  Guid IdCompte { get; set; }
        public string Nom { get; set; }
        [Required]
        [StringLength(8)]
        public string CIN { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool Access { get; set; }

        #region Role
        public Guid? FkRole { get; set; }   // nullable foreign key
        public virtual Role Role { get; set; }
        #endregion

        #region Filiale
        public Guid? FkFiliale { get; set; }
        public virtual Filiale Filiale { get; set; }
        #endregion
    }
}
