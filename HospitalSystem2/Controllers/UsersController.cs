using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using HospitalSystem2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalSystem2.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly SignInManager<AppUser> _signInManager;
        public UsersController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager
                                 /*SignInManager<AppUser> signInManager*/)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            //_signInManager = signInManager;

        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> dbUsers = await _userManager.Users.ToListAsync();
            List<UserVM> usersVm = new List<UserVM>();
            foreach (AppUser dbUser in dbUsers)
            {
                UserVM userVm = new UserVM
                {
                    Id = dbUser.Id,
                    Name = dbUser.Name,
                    Surname = dbUser.Surname,
                    Username = dbUser.UserName,
                    Email = dbUser.Email,
                    IsDeactive = dbUser.IsDeactive,
                    Role =(await _userManager.GetRolesAsync(dbUser))[0],
                };
                usersVm.Add(userVm);
            }
            return View(usersVm);
        }
        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            if (dbUser.IsDeactive)
            {
                dbUser.IsDeactive = false;
            }
            else
            {
                dbUser.IsDeactive = true;
            }
            await _userManager.UpdateAsync(dbUser);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.SuperAdmin.ToString(),
            };
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM userVM,string role)
        {
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.SuperAdmin.ToString(),
            };
            AppUser newUser = new AppUser
            {
                UserName = userVM.Username,
                Name = userVM.Name,
                Surname = userVM.Surname,
                Email = userVM.Email,
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, userVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, role);
            return RedirectToAction("Index");
        }
        [Authorize(Roles="SuperAdmin")]
        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                Name = dbUser.Name,
                Username = dbUser.UserName,
                Surname = dbUser.Surname,
                Email = dbUser.Email,
                Role = (await _userManager.GetRolesAsync(dbUser))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.SuperAdmin.ToString(),
            };
            return View(dbUpdateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id,UpdateVM updateVM,string role)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new UpdateVM
            {
                Name = dbUser.Name,
                Username = dbUser.UserName,
                Surname = dbUser.Surname,
                Email = dbUser.Email,
                Role = (await _userManager.GetRolesAsync(dbUser))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                Roles.SuperAdmin.ToString(),
            };
            dbUser.Name=updateVM.Name;
            dbUser.UserName = updateVM.Username;
            dbUser.Surname=updateVM.Surname;
            dbUser.Email=updateVM.Email;
            if(dbUpdateVM.Role !=role) 
            {
              IdentityResult removeIdentityResult =  await _userManager.RemoveFromRoleAsync(dbUser, dbUpdateVM.Role);
                if (!removeIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in removeIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(dbUser, role);
                if (!addIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in addIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }
            await _userManager.UpdateAsync(dbUser);
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            TempData["userid"] = id;
            AppUser user = await _userManager.FindByIdAsync(id);
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            TempData["token"] = passwordResetToken;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            var userid = TempData["userid"];
            var token = TempData["token"];
            if (userid == null || token == null)
            {
                ModelState.AddModelError("password", "Istifadeci tapilmadi!");
                return View();
            }
            var user = await _userManager.FindByIdAsync(userid.ToString());
            var result = await _userManager.ResetPasswordAsync(user, token.ToString(), resetPasswordVM.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("login", "account");
            }
            return View();
        }
    }
}
