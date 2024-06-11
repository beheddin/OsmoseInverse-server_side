using Domain.Models;
using System;
using System.Collections.Generic;

namespace OsmoseProject.Models
{
    public class TypeCartouche
    {
        public Guid IdTypeCartouche { get; set; }
        public string LabelTypeCartouche { get; set; }

        public virtual ICollection<Cartouche> Cartouche { get; set; }
    }
}
