﻿using Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DataTransferObjects
{
    public class SourceEauEntretienDTO
    {
        public Guid IdSourceEauEntretien { get; set; }
        public string NomSourceEauEntretien { get; set; }
        public string DescriptionSourceEauEntretien { get; set; }
        public double ChargeSourceEauEntretien { get; set; }
        public bool IsExternalSourceEauEntretien { get; set; }
        //[EnumDataType(typeof(TypeSourceEau), ErrorMessage = "SourceEauEntretien can only be 'BassinEntretien' or 'PuitEntretien'")]
        public string Descriminant { get; set; }    //PuitEntretien ou BassinEntretien

        //public string? NomPuit { get; set; }
        //public string? NomBassin { get; set; }
        public string NomSourceEau { get; set; }
        public string NomFournisseur { get; set; }
    }
}
