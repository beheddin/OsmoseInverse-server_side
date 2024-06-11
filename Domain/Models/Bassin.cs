using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace OsmoseProject.Models
{
    public class Bassin : SourceEau
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string NomBassin { get; set; }
    }
}
