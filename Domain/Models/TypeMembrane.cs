using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class TypeMembrane
    {
        public Guid IdTypeMembrane { get; set; }
        public string LabelTypeMembrane { get; set; }
        public double TailleTypeMembrane { get; set; }
        //public string Path { get; set; }

        public virtual ICollection<Membrane> Membrane { get; set; }

    }
}