using HospitalSystem2.DAL;
using HospitalSystem2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using HospitalSystem2.ViewModels;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : Controller
    {
        //private readonly UserManager<AppUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly AppDbContext _appDbContext;

        //public DashboardController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        //{
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //    _appDbContext = appDbContext;
        //}
        private readonly AppDbContext _db;
        
        public DashboardController(AppDbContext db)
        {
            _db = db;
            
        }
        public async Task<IActionResult> Index()
        {
            DashboardVM dashboardVM = new DashboardVM
            {
                Total = await _db.Totals.FirstOrDefaultAsync(),
                Profits = await _db.Profits.Where(p => !p.IsDeactive).ToListAsync(),
                Costs = await _db.Costs.Where(p => !p.IsDeactive).ToListAsync()
            };

            return View(dashboardVM);
        }

        //public async Task<IActionResult> CreateSuperAdmin()
        //{
        //    AppUser superAdmin = new AppUser
        //    {
        //        UserName = "SuperAdmin",
        //        Name = "SuperAdmin",
        //        Surname = "SuperAdminov",
        //        Email="superadmin@superadmin.com"
        //    };

        //    var result = await _userManager.CreateAsync(superadmin, "Admin1234");
        //    return Ok(result);
        //}

        //public async Task<IActionResult> AddRole()
        //{
        //    AppUser appUser = await _userManager.FindByNameAsync("SuperAdmin");

        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Ok("Role Added");
        //}
    }
}
