using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class FilialeDTO
    {
        public Guid FilialeId { get; set; }
        public string FilialeLabel { get; set; }
    }
}
