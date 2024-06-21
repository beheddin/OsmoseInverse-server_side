using Domain.Models;
using System;

namespace Domain.DataTransferObjects
{
    public class SourceEauDTO
    {
        public Guid IdSourceEau { get; set; }
        public string NomSourceEau { get; set; }
        public double VolumeEau { get; set; }
        public string Descriminant { get; set; }    //Puit ou Bassin
        public string NomFiliale { get; set; }
    }
}
