using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Atelier
    {
        [Required]
        [Key]
        public  Guid IdAtelier { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string NomAtelier { get; set; }

        #region Filiale
        public Guid? FkFiliale { get; set; }
        public virtual Filiale Filiale { get; set; }
        #endregion

        public virtual ICollection<Station> Stations { get; set; }
    }
}
