using Domain.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.DataTransferObjects
{
    public class CompteDTO
    {
        public Guid IdCompte { get; set; }
        public string Nom { get; set; }
        public string CIN { get; set; }
        //[JsonIgnore]    //hide pwd in JSON response
        public string Password { get; set; }
        public bool? Access { get; set; }

        public string NomRole { get; set; }
        public string NomFiliale { get; set; }
    }
}
