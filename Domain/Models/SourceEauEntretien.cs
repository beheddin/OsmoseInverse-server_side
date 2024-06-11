using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class SourceEauEntretien
    {
        [Required]
        [Key]
        public  Guid IdSourceEauEntretien { get; set; }
        public string SourceEauEntretienDescription { get; set; }
        public double SourceEauEntretienCharge { get; set; }
        public bool IsExternalSourceEauEntretien { get; set; }
        public string Descriminant { get; set; }
        public string NomPuit { get; set; }
        public string NomBassin { get; set; }

        #region Fournisseur
        public  Guid? FkFournisseur { get; set; }
        public Fournisseur Fournisseur { get; set; }
        #endregion

        #region SourceEau
        public  Guid? FkSourceEau { get; set; }
        public SourceEau SourceEau { get; set; }
        #endregion
    }
}
