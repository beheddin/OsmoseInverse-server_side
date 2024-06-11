using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class SuiviQuotidien
    {
        [Required]
        [Key]
        public Guid IdSuiviQuotidien { get; set; }
        public DateTime DateSuiviQuotidien { get; set; }
        public double ValueSuiviQuotidien { get; set; }

        #region StationParametre
        public Guid? FkStationParametre { get; set; }
        public virtual StationParametre StationParametre { get; set; }
        #endregion
    }
}
