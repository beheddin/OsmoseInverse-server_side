﻿using System;

namespace Domain.DataTransferObjects
{
    public abstract class SourceEauEntretienDTO
    {
        public Guid IdSourceEauEntretien { get; set; }
        public string NomSourceEauEntretien { get; set; }
        public string DescriptionSourceEauEntretien { get; set; }
        public double ChargeSourceEauEntretien { get; set; }
        public bool IsExternalSourceEauEntretien { get; set; }
        public string Descriminant { get; set; }

        //public string? NomPuit { get; set; }
        //public string? NomBassin { get; set; }
        public string NomFournisseur { get; set; }
    }
}