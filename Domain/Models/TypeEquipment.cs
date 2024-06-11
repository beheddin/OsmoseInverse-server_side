using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class TypeEquipment
    {
        [Required]
        [Key]
        public  Guid IdTypeEquipment { get; set; }
        public string LabelTypeEquipment { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
    }
}
