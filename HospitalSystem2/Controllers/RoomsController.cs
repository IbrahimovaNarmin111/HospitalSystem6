using HospitalSystem2.DAL;
using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using HospitalSystem2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class RoomsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public RoomsController(AppDbContext db,IWebHostEnvironment env) 
        {
            _db = db;
            _env = env;
        }
       
        public async Task<IActionResult> Index(int page=1)
        {
            decimal take = 4;
            ViewBag.PageCount=Math.Ceiling((await _db.Rooms.CountAsync()/take));
            List<Room> rooms = await _db.Rooms.Skip((page-1)*4).Take((int)take).ToListAsync();

            return View(rooms);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(Room room) 
        {
            if (room.Photo == null)
            {
                ModelState.AddModelError("Photo", "Image can not be null");
                return View();
            }
            if (!room.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image type");
                return View();
            }
            if (room.Photo.IsOlder1Mb())
            {
                ModelState.AddModelError("Photo", "max 1mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "uploads/room");
            room.Image = await room.Photo.SaveFileAsync(folder);
            bool isExist = await _db.Rooms.AnyAsync(x => x.Name == room.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This department is already exist !");
                return View();
            }

            await _db.Rooms.AddAsync(room);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Room dbRoom = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }
            if (dbRoom.IsDeactive)
            {
                dbRoom.IsDeactive = false;
            }
            else
            {
                dbRoom.IsDeactive = true;
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
            Room dbRoom = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }

            return View(dbRoom);
        }
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Room dbRoom = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }
            return View(dbRoom);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Room room)
        {

            if (id == null)
            {
                return NotFound();
            }
            Room dbRoom = await _db.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (dbRoom == null)
            {
                return BadRequest();
            }
            #region Exist Item
            //bool isExist = await _db.Rooms.AnyAsync(x => x.Name == room.Name);
            //if (isExist)
            //{
            //    ModelState.AddModelError("Name", "This department is already exist !");
            //    return View();
            //}
            #endregion
            if (room.Photo != null)
            {
                if (!room.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image type");
                    return View();
                }
                if (room.Photo.IsOlder1Mb())
                {
                    ModelState.AddModelError("Photo", "max 1mb");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "uploads/room");
                dbRoom.Image = await room.Photo.SaveFileAsync(folder);
                
               
            }


            dbRoom.Contact = room.Contact;
            dbRoom.Description = room.Description;
            dbRoom.Name = room.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
