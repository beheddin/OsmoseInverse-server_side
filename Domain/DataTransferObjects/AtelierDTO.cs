//#nullable enable

using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DataTransferObjects
{
    public class AtelierDTO
    {
        public Guid AtelierId { get; set; }
        public string AtelierLabel { get; set; }
        public string FilialeLabel { get; set; }
    }
}
