using Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Puit : SourceEau
    {
        //[Required]
        //[StringLength(30, MinimumLength = 3)]
        //public string NomPuit { get; set; }
        public double Profondeur { get; set; }
        public string TypeAmortissement { get; set; }

        //public virtual ICollection<PuitEntretien> PuitEntretiens { get; set; }
    }
}
