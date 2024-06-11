using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class SourceEau
    {
        [Required]
        [Key]
        public  Guid IdSourceEau { get; set; }
        public Double VolumeEau { get; set; }

        #region Filiale
        public  Guid? FkFiliale { get; set; }
        public virtual Filiale Filiale { get; set; }
        #endregion

        public virtual ICollection<SourceEauEntretien> SourceEauEntretiens { get; set; }
    }
}
