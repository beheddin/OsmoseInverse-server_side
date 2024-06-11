using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LavageChimique
    {
        [Required]
        [Key]
        public Guid IdLavageChimique { get; set; }
        public DateTime DateLavageChimique { get; set; }

        #region ProduitChimique
        public Guid? FkProduitChimique { get; set; }
        public virtual ProduitChimique ProduitChimique { get; set; }
        #endregion

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion
    }
}
