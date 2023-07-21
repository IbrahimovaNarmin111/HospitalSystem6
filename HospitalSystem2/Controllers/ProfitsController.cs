using HospitalSystem2.DAL;
using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ProfitsController : Controller
    {
        private readonly AppDbContext _db;
        public ProfitsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(string search,int page=1)
        {
            List<Profit> profit = new List<Profit>();
            if(!string.IsNullOrEmpty(search)) 
            {
                var profitt = from d in _db.Profits select d;
                profit=await _db.Profits.Where(x=>x.Description.Contains(search)).OrderByDescending(x=>x.Id).ToListAsync();
            return View(profit);    
            }
            decimal take = 6;
            ViewBag.PageCount=Math.Ceiling((await _db.Profits.CountAsync() / take));
            ViewBag.CurrentPage = page;
            profit=await _db.Profits.Skip((page-1)*6).Take((int)take).ToListAsync();
            return View(profit);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(Profit profit)
        {
            if(profit.Amount<=0) 
            {
                ModelState.AddModelError("Amount", "Gəliri düzgün daxil edin");
                return View();
            
            }
            profit.By = User.Identity.Name;
            Total total = await _db.Totals.FirstOrDefaultAsync();
            if(total==null) 
            {
                total = new Total();
                _db.Totals.Add(total);
            }
            total.LastModifiedAmount=profit.Amount;
            total.LastModifiedDescription=profit.Description;
            total.LastModifiedBy=profit.By;
            total.LastModifiedTime = profit.CreatedTime;
            total.TotalCash += profit.Amount;
            await _db.Profits.AddAsync(profit);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Profit dbProfit = await _db.Profits.FirstOrDefaultAsync(x => x.Id == id);
            if (dbProfit == null)
            {
                return BadRequest();
            }
            if (dbProfit.IsDeactive)
            {
                dbProfit.IsDeactive = false;
            }
            else
            {
                dbProfit.IsDeactive = true;
            }
            Total total = await _db.Totals.FirstOrDefaultAsync();
            if (dbProfit.IsDeactive)
            {
                total.TotalCash -= dbProfit.Amount;
            }
            else
            {
                total.TotalCash += dbProfit.Amount;
            }
            dbProfit.By = User.Identity.Name;
            total.LastModifiedAmount = dbProfit.Amount;
            total.LastModifiedDescription = dbProfit.Description;
            total.LastModifiedBy = dbProfit.By;
            total.LastModifiedTime = dbProfit.CreatedTime;
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
            Profit dbProfit = await _db.Profits.FirstOrDefaultAsync(x => x.Id == id);
            if (dbProfit == null)
            {
                return BadRequest();
            }
            return View(dbProfit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id, Profit profit)
        {

            if (id == null)
            {
                return NotFound();
            }
            Profit dbProfit = await _db.Profits.FirstOrDefaultAsync(x => x.Id == id);
            if (dbProfit == null)
            {
                return BadRequest();
            }
            double amountDifference = profit.Amount - dbProfit.Amount;
            dbProfit.Amount = profit.Amount;
            dbProfit.Description = profit.Description;

            await _db.SaveChangesAsync();

            Total total = await _db.Totals.FirstOrDefaultAsync();
            total.TotalCash += amountDifference;
            profit.By = User.Identity.Name;
            total.LastModifiedAmount = profit.Amount;
            total.LastModifiedDescription = profit.Description;
            total.LastModifiedBy = profit.By;
            total.LastModifiedTime = profit.CreatedTime;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
