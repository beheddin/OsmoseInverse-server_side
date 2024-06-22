using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Filiale
    {
        [Key]
        public  Guid IdFiliale{ get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string NomFiliale { get; set; }
        public string AbbreviationNomFiliale { get; set; }
        public virtual ICollection<Compte> Comptes { get; set; }
        public virtual ICollection<Atelier> Ateliers { get; set; }
        public virtual ICollection<SourceEau> SourcesEaux { get; set; }
    }
}
