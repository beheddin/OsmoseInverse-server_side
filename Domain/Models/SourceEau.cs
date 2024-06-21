using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    //public enum TypeSourceEau
    //{
    //    Bassin,
    //    Puit
    //}

    public abstract class SourceEau
    {
        [Required]
        [Key]
        public Guid IdSourceEau { get; set; }

        //[Required]
        //[EnumDataType(typeof(TypeSourceEau), ErrorMessage = "SourceEau can only be 'Bassin' or 'Puit'")]
        //public TypeSourceEau TypeSourceEau { get; set; }

        public double VolumeEau { get; set; }

        #region Filiale
        public Guid? FkFiliale { get; set; }
        public virtual Filiale Filiale { get; set; }
        #endregion

        public virtual ICollection<SourceEauEntretien> SourceEauEntretiens { get; set; }
    }
}
