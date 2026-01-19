using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftLanding.Areas.ViewModels;
using SoftLanding.DAL;
using SoftLanding.Models;
using SoftLanding.Utilities.Enums;
using SoftLanding.Utilities.Extensions;

namespace SoftLanding.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    [Area("Admin")]
    public class WorkerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public WorkerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Worker> workers = await _context.Workers.Include(w => w.Designation).ToListAsync();
            return View(workers);
        }

        public async Task<IActionResult> Create()
        {
            List<Designation> designations = await _context.Designations.ToListAsync();

            CreateWorkerVm createWorkerVm = new()
            {
                Designations = designations,
            };

            return View(createWorkerVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWorkerVm createWorkerVm)
        {
            if (!ModelState.IsValid)
            {
                createWorkerVm.Designations = await _context.Designations.ToListAsync();
                return View(createWorkerVm);
            }


            if (!createWorkerVm.ImageFile.ValidateType("image"))
            {
                ModelState.AddModelError("ImageFile", "Please select a valid image file.");
                createWorkerVm.Designations = await _context.Designations.ToListAsync();
                return View(createWorkerVm);
            }

            if (!createWorkerVm.ImageFile.ValidateSize(FileSize.MB, 2))
            {
                ModelState.AddModelError("ImageFile", "Image size must be less than 2 MB.");
                createWorkerVm.Designations = await _context.Designations.ToListAsync();
                return View(createWorkerVm);
            }


            Worker worker = new()
            {
                Fullname = createWorkerVm.Fullname,
                DesignationId = createWorkerVm.DesignationID,
                Image = await createWorkerVm.ImageFile.CreateFileAsync(_env.WebRootPath, "assets", "img")
            };

            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            Worker? worker = await _context.Workers.FirstOrDefaultAsync(w => w.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            UpdateWorkerVm updateWorkerVm = new()
            {
                Id = worker.Id,
                Fullname = worker.Fullname,
                DesignationID = worker.DesignationId,
                Designations = await _context.Designations.ToListAsync()
            };

            return View(updateWorkerVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateWorkerVm updateWorkerVm)
        {
            if (!ModelState.IsValid)
            {
                updateWorkerVm.Designations = await _context.Designations.ToListAsync();
                return View(updateWorkerVm);
            }


            Worker? worker = await _context.Workers.FirstOrDefaultAsync(w => w.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            if (await _context.Workers.AnyAsync(w => w.Fullname == updateWorkerVm.Fullname && w.Id != id))
            {
                ModelState.AddModelError("Fullname", "This worker fullname is already taken by another worker.");
                updateWorkerVm.Designations = await _context.Designations.ToListAsync();
                return View(updateWorkerVm);
            }

            worker.Fullname = updateWorkerVm.Fullname;
            worker.DesignationId = updateWorkerVm.DesignationID;

            if (updateWorkerVm.ImageFile != null)
            {
                if (!updateWorkerVm.ImageFile.ValidateType("image"))
                {
                    ModelState.AddModelError("ImageFile", "Please select a valid image file.");
                    updateWorkerVm.Designations = await _context.Designations.ToListAsync();
                    return View(updateWorkerVm);
                }

                if (!updateWorkerVm.ImageFile.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError("ImageFile", "Image size must be less than 2 MB.");
                    updateWorkerVm.Designations = await _context.Designations.ToListAsync();
                    return View(updateWorkerVm);
                }

                worker.Image = await updateWorkerVm.ImageFile.CreateFileAsync(_env.WebRootPath, "assets", "img");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            Worker? worker = await _context.Workers.FirstOrDefaultAsync(w => w.Id == id);
            if (worker == null)
            {
                return NotFound();
            }
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
