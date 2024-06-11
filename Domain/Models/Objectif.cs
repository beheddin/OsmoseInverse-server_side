using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Objectif
    {
        [Required]
        [Key]
        public  Guid IdObjectif { get; set; }
        public int Annee { get; set; }
        public double TDSeauBrute { get; set; }
        public double TDSeauOsmosee { get; set; }
        public double RendementMembranes { get; set; }
        public double Rendement { get; set; }
        public double PermanentFlow { get; set; }
        public double TauxExploitation { get; set; }
        public double DeltapMicrofilter { get; set; }
        public double DeltapMembrane { get; set; }
        public double DeltapSable { get; set; }
        public double Cost { get; set; }
        public double TH { get; set; }
        public double PH { get; set; }
        public double TauxChlorure { get; set; }

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion
    }
}
