using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class StationDTO
    {
        public  Guid IdStation { get; set; }
        public string NomStation { get; set; }
        public string CodeStation { get; set; }
        public double CapaciteStation { get; set; }
        public string TypeAmmortissement { get; set; }
        public bool IsActif { get; set; }
        public string NomAtelier { get; set; }
    }
}
