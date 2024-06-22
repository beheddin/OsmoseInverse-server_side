using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class StationParametre
    {
        [Key]
        public  Guid IdStationParametre { get; set; }

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion

        #region Station
        public Guid? FkSuiviParametre { get; set; }
        public virtual SuiviParametre SuiviParametre { get; set; }
        #endregion

        public virtual ICollection<SuiviQuotidien> SuivisQuotidiens { get; set; }
    }
}
