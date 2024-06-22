using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public enum TypeSourceEauEntretien
    {
        BassinEntretien,
        PuitEntretien
    }

    public class SourceEauEntretien
    {
        [Key]
        public Guid IdSourceEauEntretien { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string NomSourceEauEntretien { get; set; }
        public string DescriptionSourceEauEntretien { get; set; }
        public double ChargeSourceEauEntretien { get; set; }
        public bool IsExternalSourceEauEntretien { get; set; }
        [Required]
        [EnumDataType(typeof(TypeSourceEau), ErrorMessage = "SourceEauEntretien can only be 'BassinEntretien' or 'PuitEntretien'")]
        //public string Descriminant { get; set; }
        public TypeSourceEauEntretien Descriminant { get; set; }    //PuitEntretien ou BassinEntretien

        #region SourceEau
        public Guid? FkSourceEau { get; set; }
        public virtual SourceEau SourceEau { get; set; }
        #endregion

        //#region Puit
        //public Guid? FkPuit { get; set; }
        ////public Puit Puit { get; set; }
        //public virtual SourceEau Puit { get; set; }
        ////public string NomPuit { get; set; }
        //#endregion

        //#region Bassin
        //public Guid? FkBassin { get; set; }
        ////public Bassin Bassin { get; set; }
        //public virtual SourceEau Bassin { get; set; }
        ////public string NomBassin { get; set; }
        //#endregion

        #region Fournisseur
        public Guid? FkFournisseur { get; set; }
        public virtual Fournisseur Fournisseur { get; set; }
        #endregion
    }
}
