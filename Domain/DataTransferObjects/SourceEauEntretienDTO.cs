﻿using System;

namespace Domain.DataTransferObjects
{
    public class SourceEauEntretienDTO
    {
        public Guid IdSourceEauEntretien { get; set; }
        public string NomSourceEauEntretien { get; set; }
        public string DescriptionSourceEauEntretien { get; set; }
        public double ChargeSourceEauEntretien { get; set; }
        public bool IsExternalSourceEauEntretien { get; set; }
        public string Descriminant { get; set; }    //Puit ou Bassin

        //public string? NomPuit { get; set; }
        //public string? NomBassin { get; set; }
        public string NomSourceEau{ get; set; }    //Puit ou Bassin
        public string NomFournisseur { get; set; }
    }
}
