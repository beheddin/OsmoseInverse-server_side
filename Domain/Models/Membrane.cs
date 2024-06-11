using System;

namespace Domain.Models
{
    public class Membrane : ProduitConsommable
    {
        public string LabelMembrane { get; set; }

        #region TypeMembrane
        public Guid? FkTypeMembrane { get; set; }
        public virtual TypeMembrane TypeMembrane { get; set; }
        #endregion

    }
}