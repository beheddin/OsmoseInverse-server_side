using OsmoseProject.Models;
using System;

namespace Domain.Models
{
    public class Cartouche : ProduitConsommable
    {
        public string LabelCartouche { get; set; }

        #region TypeCartouche 
        public Guid? FkTypeCartouche { get; set; }
        public virtual TypeCartouche TypeCartouche { get; set; }
        #endregion
    }
}
