using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class PuitDTO : SourceEauDTO
    {
        public double Profondeur { get; set; }
        public string TypeAmortissement { get; set; }
    }
}
