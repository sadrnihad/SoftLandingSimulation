using SoftLanding.Models;

namespace SoftLanding.Areas.ViewModels
{
    public class UpdateWorkerVm
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public int DesignationID { get; set; }
        public IFormFile? ImageFile { get; set; }

        public List<Designation> Designations { get; set; }
    }
}
