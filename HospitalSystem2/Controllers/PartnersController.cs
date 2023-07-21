using HospitalSystem2.DAL;
using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class PartnersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public PartnersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            decimal take = 5;
            ViewBag.PageCount = Math.Ceiling((await _db.Partners.CountAsync() / take));
            List<Partner> partners = await _db.Partners.Skip((page - 1) * 5).Take((int)take).ToListAsync();
            return View(partners);
            
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partner partner)
        {
            if (partner.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil boş qala bilməz");
                return View();
            }
            if (!partner.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil növünü seçin");
                return View();
            }
            if (partner.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "max 1mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "uploads/partner");
            partner.Image = await partner.Photo.SaveFileAsync(folder);
            bool isExist = await _db.Partners.AnyAsync(x => x.Name == partner.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu partnyor mövcuddur !");
                return View();
            }

            await _db.Partners.AddAsync(partner);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Partner dbPartner = await _db.Partners.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPartner == null)
            {
                return BadRequest();
            }
            if (dbPartner.IsDeactive)
            {
                dbPartner.IsDeactive = false;
            }
            else
            {
                dbPartner.IsDeactive = true;
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
            Partner dbPartner = await _db.Partners.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPartner == null)
            {
                return BadRequest();
            }

            return View(dbPartner);
        }
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Partner dbPartner = await _db.Partners.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPartner == null)
            {
                return BadRequest();
            }
            return View(dbPartner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Partner partner)
        {

            if (id == null)
            {
                return NotFound();
            }
            Partner dbPartner = await _db.Partners.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPartner == null)
            {
                return BadRequest();
            }
            #region Exist Item
            //bool isExist = await _db.Partners.AnyAsync(x => x.Name == partner.Name);
            //if (isExist)
            //{
            //    ModelState.AddModelError("Name", "Bu partnyor mövcuddur !");
            //    return View();
            //}
            #endregion
            if (partner.Photo != null)
            {
                if (!partner.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil növünü seçin");
                    return View();
                }
                if (partner.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "max 1mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "uploads/partner");
                dbPartner.Image = await partner.Photo.SaveFileAsync(folder);


            }


            dbPartner.Contact = partner.Contact;
            dbPartner.Description = partner.Description;
            dbPartner.Name = partner.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Partner dbPartner = await _db.Partners.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPartner == null)
            {
                return BadRequest();
            }
            return View(dbPartner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Partner partner = _db.Partners.FirstOrDefault(x => x.Id == id);
            if (partner is null) return NotFound();


            _db.Partners.Remove(partner);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

    

