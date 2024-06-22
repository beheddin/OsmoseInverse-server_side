using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CheckListElement
    {
        [Key]
        public Guid IdCheckListElement { get; set; }
        [Required]
        public string LabelCheckList { get; set; }

        #region Station
        public Guid? FkStation { get; set; }
        public virtual Station Station { get; set; }
        #endregion
    }
}
