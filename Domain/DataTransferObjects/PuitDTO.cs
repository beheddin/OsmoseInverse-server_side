namespace Domain.DataTransferObjects
{
    public class PuitDTO : SourceEauDTO
    {
        public double Profondeur { get; set; }
        public string TypeAmortissement { get; set; }
    }
}
