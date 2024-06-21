using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class FournisseurDTO
    {
        public Guid IdFournisseur { get; set; }
        public string NomFournisseur { get; set; }
        //public string CodeFournisseur { get; set; }
        public string NumTelFournisseur { get; set; }
        public string NumFaxFournisseur { get; set; }
        public string EmailFournisseur { get; set; }
        public string AddresseFournisseur { get; set; }
    }
}
