using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class SuiviType
    {
        [Required]
        [Key]
        public  Guid IdSuiviType { get; set; }
        public string LabelSuiviType { get; set; }

        public virtual ICollection<SuiviParametre> SuiviParametres { get; set; }
    }
}
