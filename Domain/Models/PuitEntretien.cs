using System;

namespace Domain.Models
{
    public class PuitEntretien : SourceEauEntretien
    {
        //public string NomPuit { get; set; }
        #region Puit
        public Guid? FkPuit { get; set; }
        public virtual Puit Puit { get; set; }
        #endregion
    }
}
