using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class PuitDTO
    {
        public Guid IdSourceEau { get; set; }
        public double VolumeEau { get; set; }
        public string NomFiliale { get; set; }
        public string NomPuit { get; set; }
        public double Profondeur { get; set; }
        public string TypeAmortissement { get; set; }
    }
}
