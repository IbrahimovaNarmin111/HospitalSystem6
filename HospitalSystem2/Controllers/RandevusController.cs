using HospitalSystem2.DAL;
using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class RandevusController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public RandevusController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            decimal take = 6;
            ViewBag.PageCount = Math.Ceiling((await _db.Randevus.CountAsync() / take));
            List<Randevu> randevus = await _db.Randevus.Include(x => x.Doctor).Skip((page - 1) * 6).Take((int)take).ToListAsync();
            return View(randevus);
        }
        
        public async Task<IActionResult> Create()
        {
            ViewBag.Doctors = await _db.Doctors.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Randevu randevu, int randevuId)
        {
            ViewBag.Doctors = await _db.Doctors.Include(x=>x.Receptions).ToListAsync();
            #region Exist Item
            bool isExist = await _db.Randevus.AnyAsync(x => x.Name == randevu.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu randevu mövcuddur !");
                return View();
            }
            #endregion
            Doctor doctor=await _db.Doctors.FindAsync(randevu.DoctorId);
            DateTime dateTime = new DateTime(1900, 01, 01);
            if(randevu.BirthDate<dateTime)
            {
                ModelState.AddModelError("BirthDate", "1900cü ildən sonranı seçin!");
                return View();
            }
           if(randevu.RandevuDate<DateTime.Now) 
            {
                ModelState.AddModelError("RandevuDate", "Düzgün tarix seçin!");
                return View();
            }
            foreach (var reception in doctor.Receptions)
            {
                if (reception.ReceptionDayId == 7)
                    reception.ReceptionDayId = 0;
                if (reception.ReceptionDayId != (int)randevu.RandevuDate.DayOfWeek)
                    continue;
                else
                {
                    await _db.Randevus.AddAsync(randevu);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("RandevuDate", "Qəbul günü həkimə uyğun deyil!");
            return View();

        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Randevu dbRandevu = await _db.Randevus.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRandevu == null)
            {
                return BadRequest();
            }
            if (dbRandevu.IsDeactive)
            {
                dbRandevu.IsDeactive = false;
            }
            else
            {
                dbRandevu.IsDeactive = true;
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
            Randevu dbRandevu = await _db.Randevus.Include(x=>x.Doctor).FirstOrDefaultAsync(x => x.Id == id);
            if (dbRandevu == null)
            {
                return BadRequest();
            }


            return View(dbRandevu);
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Randevu dbRandevu = await _db.Randevus.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRandevu == null)
            {
                return BadRequest();
            }
            return View(dbRandevu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            Randevu randevu = _db.Randevus.FirstOrDefault(x => x.Id == id);
            if (randevu is null) return NotFound();
            
           
            _db.Randevus.Remove(randevu);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
