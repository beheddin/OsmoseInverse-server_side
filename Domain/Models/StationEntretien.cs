using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class StationEntretien 
    {
        [Required]
        [Key]
        public  Guid IdStationEntretien { get; set; }
        public string NomStationEntretien { get; set; }
        public string DescriptionStationEntretien { get; set; }
        public double ChargeStationEntretien { get; set; }
        public bool IsExternalStationEntretien { get; set; }

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion

        #region Fournisseur
        public Guid? FkFournisseur { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
        #endregion
    }
}
