using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class AtelierDTO
    {
        public  Guid IdAtelier { get; set; }
        public string NomAtelier { get; set; }
        public string NomFiliale { get; set; }
    }
}
