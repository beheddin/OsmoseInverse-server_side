using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class BassinDTO : SourceEauDTO
    {
        //public Guid IdSourceEau { get; set; }
        //public double VolumeEau { get; set; }
        //public string NomFiliale { get; set; }
        public string NomBassin { get; set; }
    }
}
