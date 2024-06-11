using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class ProduitConsommable
    {
        [Required]
        [Key]
        public  Guid IdProduitConsommable { get; set; }
        public string LabelProduitConsommable { get; set; }
        public double QuantiteProduitConsommable { get; set; }
        public DateTime DateInstallationProduitConsommable { get; set; }

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion

        #region Filiale
        public Guid? FkUnite { get; set; }
        public virtual Unite Unite { get; set; }
        #endregion
    }
}
