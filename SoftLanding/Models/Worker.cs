using System.ComponentModel.DataAnnotations;
using SoftLanding.Models.Base;

namespace SoftLanding.Models
{
    public class Worker : BaseEntity
    {
        [MaxLength(50)]
        public string Fullname { get; set; }
        [Required]
        public string Image { get; set; }

        public string Description { get; set; }

        public int DesignationId { get; set; }
        public Designation Designation { get; set; }

    }
}
