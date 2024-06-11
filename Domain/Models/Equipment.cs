using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Equipment
    {
        [Required]
        [Key]
        public  Guid IdEquipment { get; set; }
        public string LabelEquipment { get; set; }

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion

        #region NatureEquipment
        public Guid? FkNatureEquipment { get; set; }
        public virtual NatureEquipment NatureEquipment { get; set; }
        #endregion

        #region TypeEquipment
        public Guid? FkTypeEquipment { get; set; }
        public virtual TypeEquipment TypeEquipment { get; set; }
        #endregion
    }
}
