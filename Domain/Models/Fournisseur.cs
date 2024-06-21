using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Fournisseur
    {
        [Required]
        [Key]
        public  Guid IdFournisseur { get; set; }
        public string NomFournisseur{ get; set; }
        //public string CodeFournisseur { get; set; }
        public string NumTelFournisseur { get; set; }
        public string NumFaxFournisseur { get; set; }
        public string EmailFournisseur { get; set; }
        public string AddresseFournisseur { get; set; }

        public virtual ICollection<StationEntretien> StationEntretiens { get; set; }
        public virtual ICollection<SourceEauEntretien> SourceEauEntretiens { get; set; }
    }
}
