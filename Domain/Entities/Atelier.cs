using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Atelier
    {
        [Key]
        public Guid AtelierId { get; set; }
        
        [Required]
        public string AtelierLabel { get; set; }

        #region Filiale
        public Guid? FkFiliale { get; set; }

        public virtual Filiale Filiale { get; set; }
        #endregion

        public virtual ICollection<Station> Stations { get; set; }
    }
}
