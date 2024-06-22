using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class SuiviParametre
    {
        [Key]
        public  Guid IdSuiviParametre { get; set; }
        public string LabelSuiviParametre { get; set; }

        #region TypeSuivi
        public Guid? FkSuiviType { get; set; }
        public virtual SuiviType SuiviType { get; set; }
        #endregion

        public virtual ICollection<StationParametre> StationParametres { get; set; }
    }
}
