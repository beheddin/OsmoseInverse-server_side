using Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DataTransferObjects
{
    public class SourceEauDTO
    {
        public Guid IdSourceEau { get; set; }
        public string NomSourceEau { get; set; }
        public double VolumeEau { get; set; }
        [Required]
        [EnumDataType(typeof(TypeSourceEau), ErrorMessage = "SourceEau can only be 'Bassin' or 'Puit'")]
        //public TypeSourceEau Descriminant { get; set; }    // Puit ou Bassin
        public string Descriminant { get; set; }    //Puit ou Bassin

        public string NomFiliale { get; set; }
    }
}
