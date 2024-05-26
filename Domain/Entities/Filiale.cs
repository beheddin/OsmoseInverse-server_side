using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Filiale
    {
        [Key]
        public Guid FilialeId{ get; set; }

        [Required]
        public string FilialeLabel { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Atelier> Ateliers { get; set; }
    }
}
