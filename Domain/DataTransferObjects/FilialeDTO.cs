using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class FilialeDTO
    {
        public Guid IdFiliale { get; set; }
        public string NomFiliale { get; set; }
        public string AbbreviationNomFiliale { get; set; }
    }
}
