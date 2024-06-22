using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public enum TypeSourceEau
    {
        Bassin,
        Puit
    }

    public abstract class SourceEau
    {
        [Key]
        public Guid IdSourceEau { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string NomSourceEau { get; set; }
        public double VolumeEau { get; set; }
        [Required]
        [EnumDataType(typeof(TypeSourceEau), ErrorMessage = "SourceEau can only be 'Bassin' or 'Puit'")]
        //public string Descriminant { get; set; }
        public TypeSourceEau Descriminant { get; set; }    //Puit or Bassin

        #region Filiale
        public Guid? FkFiliale { get; set; }
        public virtual Filiale Filiale { get; set; }
        #endregion

        public virtual ICollection<SourceEauEntretien> SourceEauEntretiens { get; set; }
    }
}
