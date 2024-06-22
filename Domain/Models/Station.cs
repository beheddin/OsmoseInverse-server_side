using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Station
    {
        [Key]
        public  Guid IdStation { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string NomStation { get; set; }
        //public string CodeStation { get; set; }
        public int CapaciteStation { get; set; }
        public string TypeAmmortissement { get; set; }
        public bool IsActif { get; set; }


        #region Atelier
        public Guid? FkAtelier { get; set; }
        public virtual Atelier Atelier { get; set; }
        #endregion

        public ICollection<Objectif> Objectifs { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
        public ICollection<StationParametre> StationParametres { get; set; }
        public ICollection<ProduitConsommable> ProduitsConsommables { get; set; }
        public ICollection<StationEntretien> StationEntretiens { get; set; }
        public ICollection<LavageChimique> LavagesChimiques { get; set; }
    }
}
