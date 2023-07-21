using HospitalSystem2.DAL;
using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CostsController : Controller
    {
        private readonly AppDbContext _db;
        public CostsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(string search, int page = 1)
        {
            List<Cost> cost = new List<Cost>();
            if (!string.IsNullOrEmpty(search))
            {
                var costt = from d in _db.Costs select d;
                cost = await _db.Costs.Where(x => x.Description.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
                return View(cost);
            }
            decimal take = 6;
            ViewBag.PageCount = Math.Ceiling((await _db.Costs.CountAsync() / take));
            ViewBag.CurrentPage = page;
            cost = await _db.Costs.Skip((page - 1) * 6).Take((int)take).ToListAsync();
            return View(cost);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Cost cost)
        {
            if (cost.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Xərci düzgün daxil edin");
                return View();
            }

            cost.By = User.Identity.Name;
            Total total = await _db.Totals.FirstOrDefaultAsync();
            if (total == null)
            {
                total = new Total();
                _db.Totals.Add(total);
            }

            total.LastModifiedAmount = cost.Amount;
            total.LastModifiedDescription = cost.Description;
            total.LastModifiedBy = cost.By;
            total.LastModifiedTime = cost.CreatedTime;
            total.TotalCash -= cost.Amount;
            await _db.Costs.AddAsync(cost);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Cost dbCost = await _db.Costs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCost == null)
            {
                return BadRequest();
            }
            if (dbCost.IsDeactive)
            {
                dbCost.IsDeactive = false;
            }
            else
            {
                dbCost.IsDeactive = true;
            }
            Total total = await _db.Totals.FirstOrDefaultAsync();
            if (dbCost.IsDeactive)
            {
                total.TotalCash += dbCost.Amount;
            }
            else
            {
                total.TotalCash -= dbCost.Amount;
            }
            dbCost.By = User.Identity.Name;
            total.LastModifiedAmount = dbCost.Amount;
            total.LastModifiedDescription = dbCost.Description;
            total.LastModifiedBy = dbCost.By;
            total.LastModifiedTime = dbCost.CreatedTime;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            Cost dbCost = await _db.Costs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCost == null)
            {
                return BadRequest();
            }
            return View(dbCost);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Cost cost)
        {

            if (id == null)
            {
                return NotFound();
            }
            Cost dbCost = await _db.Costs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCost == null)
            {
                return BadRequest();
            }
            double amountDifference = cost.Amount - dbCost.Amount;
            dbCost.Amount = cost.Amount;
            dbCost.Description = cost.Description;

            await _db.SaveChangesAsync();

            Total total = await _db.Totals.FirstOrDefaultAsync();
            total.TotalCash -= amountDifference;
            cost.By = User.Identity.Name;
            total.LastModifiedAmount = cost.Amount;
            total.LastModifiedDescription = cost.Description;
            total.LastModifiedBy = cost.By;
            total.LastModifiedTime = cost.CreatedTime;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
