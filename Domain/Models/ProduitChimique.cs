using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ProduitChimique
    {
        [Key]
        public  Guid IdProduitChimique { get; set; }
        public string LabelProduitChimique { get; set; }
        public string Path { get; set; }

        #region CategorieProduitChimique
        public  Guid? FkCategorieProduitChimique { get; set; }
        public virtual CategorieProduitChimique CategorieProduitChimique { get; set; }
        #endregion

        public ICollection<DosageChimique> DosageChimiques { get; set; }
        public ICollection<LavageChimique> LavageChimiques { get; set; }
    }
}
