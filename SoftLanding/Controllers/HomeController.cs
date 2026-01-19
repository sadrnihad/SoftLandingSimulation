using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftLanding.DAL;
using SoftLanding.Models;

namespace SoftLanding.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Worker> workers = await _context.Workers.ToListAsync();
            return View(workers);
        }


    }
}
