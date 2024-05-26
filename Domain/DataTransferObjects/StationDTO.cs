using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class StationDTO
    {
        public Guid StationId { get; set; }
        public string StationLabel { get; set; }
        public string StationCode { get; set; }
        public double StationCapacity { get; set; }
        public bool IsActif { get; set; }
        public string TypeAmmortissement { get; set; }
        public string AtelierLabel { get; set; }
    }
}
