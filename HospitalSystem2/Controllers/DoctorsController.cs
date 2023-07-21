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
    public class DoctorsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public DoctorsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            decimal take = 6;
            ViewBag.PageCount = Math.Ceiling((await _db.Doctors.CountAsync() / take));
            List<Doctor> doctors = await _db.Doctors.Include(x => x.Room).Include(m => m.Receptions).Skip((page - 1) * 6).Take((int)take).ToListAsync();
            return View(doctors);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Rooms = await _db.Rooms.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor, int roomId)
        {
            ViewBag.Rooms = await _db.Rooms.ToListAsync();
            #region Exist Item
            bool isExist = await _db.Doctors.AnyAsync(x => x.Name == doctor.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu həkim mövcuddur !");
                return View();
            }
            #endregion
            #region Save Image
            if (doctor.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil boş qala bilməz");
                return View();
            }
            if (!doctor.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Zəhmət olmasa şəkil növünü seçin");
                return View();
            }
            if (doctor.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "max 1mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "uploads/doctor");
            doctor.Image = await doctor.Photo.SaveFileAsync(folder);
            #endregion
            doctor.RoomId = roomId;
            foreach (var item in doctor.ReceptionDayId)
            {
                ReceptionEnumModel receptionEnumModel = new ReceptionEnumModel
                {
                    Doctor = doctor,
                    ReceptionDayId = item
                };
                _db.ReceptionEnumModels.Add(receptionEnumModel);
            }
            await _db.Doctors.AddAsync(doctor);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Doctor dbDoctor = await _db.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            if (dbDoctor == null)
            {
                return BadRequest();
            }


            return View(dbDoctor);
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Rooms = await _db.Rooms.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Doctor dbDoctor = await _db.Doctors.Include(x => x.Room).Include(x => x.Receptions).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDoctor == null)
            {
                return BadRequest();
            }
            return View(dbDoctor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Doctor doctor, int roomId)
        {
            ViewBag.Rooms = await _db.Rooms.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Doctor dbDoctor = await _db.Doctors.Include(x => x.Room).Include(x => x.Receptions).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDoctor == null)
            {
                return BadRequest();
            }
            //#region Exist Item
            //bool isExist = await _db.Doctors.Include(x=>x.Receptions).Include(x=>x.Room).AnyAsync(x => x.Name == doctor.Name);
            //if (isExist)
            //{
            //    ModelState.AddModelError("Name", "This doctor is already exist !");
            //    return View();
            //}
            //#endregion
            if (doctor.Photo != null)
            {
                if (!doctor.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image type");
                    return View();
                }
                if (doctor.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "max 1mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "uploads/doctor");
                dbDoctor.Image = await doctor.Photo.SaveFileAsync(folder);

            }
            if (doctor.ReceptionDayId == null)
            {
                dbDoctor.ReceptionDayId = doctor.ReceptionDayId;
            }
            else
            {
                foreach (var item in dbDoctor.Receptions)
                {
                    _db.ReceptionEnumModels.Remove(item);
                }
                foreach (var receptionDay in doctor.ReceptionDayId)
                {
                    ReceptionEnumModel receptionEnumModel = new ReceptionEnumModel
                    {
                        DoctorId = doctor.Id,
                        ReceptionDayId = receptionDay,
                    };
                    _db.ReceptionEnumModels.Add(receptionEnumModel);
                }
            }
            dbDoctor.Contact = doctor.Contact;
            dbDoctor.Description = doctor.Description;
            dbDoctor.Salary = doctor.Salary;
            dbDoctor.Name = doctor.Name;
            dbDoctor.Surname = doctor.Surname;
            dbDoctor.IsDeactive = doctor.IsDeactive;
            dbDoctor.Email = doctor.Email;
            dbDoctor.RoomId = doctor.RoomId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Doctor dbDoctor = await _db.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            if (dbDoctor == null)
            {
                return BadRequest();
            }
            if (dbDoctor.IsDeactive)
            {
                dbDoctor.IsDeactive = false;
            }
            else
            {
                dbDoctor.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Doctor dbDoctor = await _db.Doctors.Include(x=>x.Receptions).FirstOrDefaultAsync(x => x.Id == id);
            if (dbDoctor == null)
            {
                return BadRequest();
            }
            return View(dbDoctor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeletePost(int id) 
        {
            Doctor doctor = _db.Doctors.Include(x=>x.Receptions).FirstOrDefault(x=>x.Id== id);  
            if(doctor is null) return NotFound();
            string path = Path.Combine(_env.WebRootPath, "uploads/doctor", doctor.Image);
            System.IO.File.Delete(path);
            foreach(var item in _db.ReceptionEnumModels.Where(x=>x.DoctorId== id))
            {
                _db.ReceptionEnumModels.Remove(item);
            }
            _db.Doctors.Remove(doctor);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
    

