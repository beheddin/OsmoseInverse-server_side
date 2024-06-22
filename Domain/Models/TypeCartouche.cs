using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OsmoseProject.Models
{
    public class TypeCartouche
    {
        [Key]
        public Guid IdTypeCartouche { get; set; }
        public string LabelTypeCartouche { get; set; }

        public virtual ICollection<Cartouche> Cartouche { get; set; }
    }
}
