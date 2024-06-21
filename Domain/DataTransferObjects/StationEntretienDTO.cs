using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class StationEntretienDTO
    {
        public Guid IdStationEntretien { get; set; }
        public string NomStationEntretien { get; set; }
        public string DescriptionStationEntretien { get; set; }
        public double ChargeStationEntretien { get; set; }
        public bool IsExternalStationEntretien { get; set; }

        public string NomStation { get; set; }        
        //public string CodeStation { get; set; }        
        public string NomFournisseur { get; set; }
        //public string CodeFournisseur { get; set; }
    }
}
