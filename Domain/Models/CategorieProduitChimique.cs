using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CategorieProduitChimique
    {
        [Key]
        public  Guid IdCategorie { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string NomCategorie { get; set; }
        public ICollection<ProduitChimique> produitsChimiques { get; set; }
    }
}
