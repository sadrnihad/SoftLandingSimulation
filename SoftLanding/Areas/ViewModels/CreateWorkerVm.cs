using Microsoft.CodeAnalysis;
using SoftLanding.Models;

namespace SoftLanding.Areas.ViewModels
{
    public class CreateWorkerVm
    {
        public string Fullname { get; set; }
        public int DesignationID { get; set; }
        public IFormFile ImageFile { get; set; }
        public List<Designation> Designations { get; set; } = new List<Designation>();
    }
}
