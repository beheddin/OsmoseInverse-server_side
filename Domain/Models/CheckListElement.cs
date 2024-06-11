using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CheckListElement
    {
        [Required]
        [Key]
        public Guid IdCheckListElement { get; set; }
        public string LabelCheckList { get; set; }

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion
    }
}
