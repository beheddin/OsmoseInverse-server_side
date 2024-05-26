using Domain.Entities;
using System;

namespace Domain.DataTransferObjects
{
    public class RoleDTO
    {
        public Guid RoleId { get; set; }
        public string RoleLabel { get; set; }
    }
}
