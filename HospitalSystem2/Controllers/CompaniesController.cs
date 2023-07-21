using HospitalSystem2.DAL;
using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CompaniesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public CompaniesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            decimal take = 5;
            ViewBag.PageCount = Math.Ceiling((await _db.Companies.CountAsync() / take));
            List<Company> companies= await _db.Companies.Skip((page - 1) * 5).Take((int)take).ToListAsync();
            return View(companies);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (company.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil boş qala bilməz");
                return View();
            }
            if (!company.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil növünü seçin");
                return View();
            }
            if (company.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "max 1mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "uploads/company");
            company.Image = await company.Photo.SaveFileAsync(folder);
            bool isExist = await _db.Companies.AnyAsync(x => x.Name == company.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu sığorta şirkəti mövcuddur !");
                return View();
            }

            await _db.Companies.AddAsync(company);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Company dbCompany = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCompany == null)
            {
                return BadRequest();
            }
            if (dbCompany.IsDeactive)
            {
                dbCompany.IsDeactive = false;
            }
            else
            {
                dbCompany.IsDeactive = true;
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
            Company dbCompany = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCompany == null)
            {
                return BadRequest();
            }

            return View(dbCompany);
        }
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Company dbCompany = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCompany == null)
            {
                return BadRequest();
            }
            return View(dbCompany);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Company company)
        {

            if (id == null)
            {
                return NotFound();
            }
            Company dbCompany = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCompany == null)
            {
                return BadRequest();
            }
            #region Exist Item
            //bool isExist = await _db.Companies.AnyAsync(x => x.Name == company.Name);
            //if (isExist)
            //{
            //    ModelState.AddModelError("Name", "Bu sığorta şirkəti mövcuddur !");
            //    return View();
            //}
            #endregion
            if (company.Photo != null)
            {
                if (!company.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil növünü seçin");
                    return View();
                }
                if (company.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "max 1mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "uploads/company");
                dbCompany.Image = await company.Photo.SaveFileAsync(folder);


            }


            dbCompany.Contact = company.Contact;
            dbCompany.Description = company.Description;
            dbCompany.Name = company.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Company dbCompany = await _db.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCompany == null)
            {
                return BadRequest();
            }
            return View(dbCompany);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Company company = _db.Companies.FirstOrDefault(x => x.Id == id);
            if (company is null) return NotFound();


            _db.Companies.Remove(company);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
