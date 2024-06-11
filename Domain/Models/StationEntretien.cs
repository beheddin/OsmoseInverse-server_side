using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class StationEntretien 
    {
        [Required]
        [Key]
        public  Guid IdStationEntretien { get; set; }
        public string EntretienDescription { get; set; }

        public double EntretienCharge { get; set; }
        public bool IsExternalEntretien { get; set; }

        #region Fournisseur
        public Guid? FkFournisseur { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
        #endregion

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion
    }
}
