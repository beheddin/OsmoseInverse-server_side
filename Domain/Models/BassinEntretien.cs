using System;

namespace Domain.Models
{
    public class BassinEntretien : SourceEauEntretien
    {
        //public string NomBassin { get; set; }
        #region Bassin
        public Guid? FkBassin { get; set; }
        public virtual Bassin Bassin { get; set; }
        #endregion
    }
}
