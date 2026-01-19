using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftLanding.DAL;
using SoftLanding.Models;

namespace SoftLanding.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    [Area("Admin")]
    public class DesignationController : Controller
    {
        private readonly AppDbContext _context;

        public DesignationController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Designation> designations = await _context.Designations.ToListAsync();
            return View(designations);
        }

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(Designation designation)
        {
            if (!ModelState.IsValid)
            {
                return View(designation);
            }

            await _context.Designations.AddAsync(designation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }













        public async Task<IActionResult> Edit(int id)
        {
            Designation? designation = await _context.Designations.FirstOrDefaultAsync(d => d.Id == id);
            if (designation == null)
            {
                return NotFound();
            }
            return View(designation);
        }







        [HttpPost]
        public async Task<IActionResult> Edit(Designation designation)
        {
            if (!ModelState.IsValid)
            {
                return View(designation);
            }

            Designation? dbDesignation = await _context.Designations.FirstOrDefaultAsync(d => d.Id == designation.Id);
            if (dbDesignation == null)
            {
                return NotFound();
            }

            if (await _context.Designations.AnyAsync(d => d.Name == designation.Name && d.Id != dbDesignation.Id))
            {
                ModelState.AddModelError("Name", "This designation name is already taken.");
                return View(designation);
            }

            dbDesignation.Name = designation.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            Designation? designation = await _context.Designations.FirstOrDefaultAsync(d => d.Id == id);
            if (designation == null)
            {
                return NotFound();
            }
            _context.Designations.Remove(designation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






    }
}
