using System;

namespace Domain.DataTransferObjects
{
    public abstract class SourceEauDTO
    {
        public Guid IdSourceEau { get; set; }
        public double VolumeEau { get; set; }
        public string NomFiliale { get; set; }
    }
}
