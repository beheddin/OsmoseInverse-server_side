using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class NatureEquipment
    {
        [Required]
        [Key]
        public  Guid IdNatureEquipment { get; set; }
        public string LabelNatureEquipment { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}
