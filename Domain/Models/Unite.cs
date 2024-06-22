using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Unite
    {
        [Key]
        public  Guid IdUnite { get; set; }
        public string LabelUnite { get; set; }

        public virtual ICollection<ProduitConsommable> ProduitsConsommables { get; set; }
    }
}
