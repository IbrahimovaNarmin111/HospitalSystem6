using HospitalSystem2.DAL;
using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CheckupsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public CheckupsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Checkup> checkups = await _db.Checkups.ToListAsync();
            return View(checkups);
            
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Checkup checkup)
        {
            if (checkup.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil boş qala bilməz");
                return View();
            }
            if (!checkup.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil növünü seçin");
                return View();
            }
            if (checkup.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "max 1mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "uploads/checkup");
            checkup.Image = await checkup.Photo.SaveFileAsync(folder);
            bool isExist = await _db.Checkups.AnyAsync(x => x.Name == checkup.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu checkup mövcuddur !");
                return View();
            }

            await _db.Checkups.AddAsync(checkup);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Checkup dbCheckup = await _db.Checkups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCheckup == null)
            {
                return BadRequest();
            }
            if (dbCheckup.IsDeactive)
            {
                dbCheckup.IsDeactive = false;
            }
            else
            {
                dbCheckup.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Checkup dbCheckup = await _db.Checkups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCheckup == null)
            {
                return BadRequest();
            }

            return View(dbCheckup);
        }
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Checkup dbCheckup = await _db.Checkups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCheckup == null)
            {
                return BadRequest();
            }
            return View(dbCheckup);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Checkup checkup)
        {

            if (id == null)
            {
                return NotFound();
            }
            Checkup dbCheckup = await _db.Checkups.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCheckup == null)
            {
                return BadRequest();
            }
            #region Exist Item
            //bool isExist = await _db.Checkups.AnyAsync(x => x.Name == checkup.Name);
            //if (isExist)
            //{
            //    ModelState.AddModelError("Name", "Bu checkup mövcuddur !");
            //    return View();
            //}
            #endregion
            if (checkup.Photo != null)
            {
                if (!checkup.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil növünü seçin");
                    return View();
                }
                if (checkup.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "max 1mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "uploads/checkup");
                dbCheckup.Image = await checkup.Photo.SaveFileAsync(folder);


            }


            dbCheckup.Price = checkup.Price;
            dbCheckup.Structure = checkup.Structure;
            dbCheckup.Name = checkup.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
