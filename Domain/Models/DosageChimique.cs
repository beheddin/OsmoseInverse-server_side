using System;

namespace Domain.Models
{
    public class DosageChimique : ProduitConsommable
    {
        public string ConfigurationPompe { get; set; }
        public double ConcentrationDosageChimique { get; set; }

        #region ProduitChimique
        public Guid? FkProduitChimique { get; set; }
        public virtual ProduitChimique ProduitChimique { get; set; }
        #endregion 
    }
}
