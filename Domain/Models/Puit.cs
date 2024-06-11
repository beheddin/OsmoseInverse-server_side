using Domain.Models;

namespace OsmoseProject.Models
{
    public class Puit : SourceEau
    {
        public double Profondeur { get; set; }
        public string NomPuit { get; set; }
        public string TypeAmortissement { get; set; }
    }
}
