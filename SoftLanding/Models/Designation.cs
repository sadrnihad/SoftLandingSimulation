using System.ComponentModel.DataAnnotations;
using SoftLanding.Models.Base;

namespace SoftLanding.Models
{
    public class Designation : BaseEntity
    {
        [MaxLength(64)]
        public string Name { get; set; }
        public List<Worker> Worker { get; set; } = new();
    }
}
