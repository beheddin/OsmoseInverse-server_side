using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Station
    {
        [Key]
        public Guid StationId { get; set; }

        [Required]
        public string StationLabel { get; set; }

        public string StationCode { get; set; }

        public double StationCapacity { get; set; }

        public bool IsActif { get; set; } = false;

        public string TypeAmmortissement { get; set; }

        #region Atelier
        public Guid? FkAtelier { get; set; }
        public virtual Atelier Atelier { get; set; }
        #endregion
    }
}
