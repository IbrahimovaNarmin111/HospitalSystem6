using HospitalSystem2.Helper;
using HospitalSystem2.Models;
using HospitalSystem2.ViewModels;
using MailKit.Net.Smtp;//
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;//
using System.Data;
using System.Threading.Tasks;

namespace HospitalSystem2.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.Mail);
            if(user==null)
            {
                return View("Error");  
            }
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetTokenLink = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                token = passwordResetToken
            }, HttpContext.Request.Scheme);

            MimeMessage mimeMessage = new MimeMessage();

            MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "narmish03@gmail.com");

            mimeMessage.From.Add(mailboxAddressFrom);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", forgetPasswordVM.Mail);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = passwordResetTokenLink;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            mimeMessage.Subject = "Şifrə Dəyişiklik Tələbi";

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("narmish03@gmail.com", "ppbijbhbshgyqwrt");
            client.Send(mimeMessage);
            client.Disconnect(true);

            return View("Index");
        }
        [HttpGet]
        public IActionResult ResetPassword(string userid, string token)
        {
            TempData["userid"] = userid;
            TempData["token"] = token;
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
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ResetPasswordAsync(user, token.ToString(), resetPasswordVM.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("login", "account");
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser user = await _userManager.FindByNameAsync(loginVM.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }
            if (user.IsDeactive)
            {
                ModelState.AddModelError("", "Your Account is deactive");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemember, true);
            if (signInResult.IsLockedOut)
            {

                ModelState.AddModelError("", "Your Account is blocked");
                return View();

            }
            if (!signInResult.Succeeded)
            {


                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }

            return RedirectToAction("Index", "Dashboard");
        }
        //public IActionResult Register()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterVM registerVM)
        //{

        //    AppUser newUser = new AppUser
        //    {
        //        UserName = registerVM.Username,
        //        Name = registerVM.Name,
        //        Surname = registerVM.Surname,   
        //        Email = registerVM.Email,
        //    };
        //    IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
        //    if (!identityResult.Succeeded)
        //    {
        //        foreach (IdentityError error in identityResult.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //        return View();
        //    }
        //    await _userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());
        //    await _signInManager.SignInAsync(newUser, registerVM.IsRemember);
        //    return RedirectToAction("Index", "Dashboard");
        //}
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Dashboard");
        }
        //public async Task CreateRoles()
        //{
        //    if (!await _roleManager.RoleExistsAsync(Roles.SuperAdmin.ToString()))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.SuperAdmin.ToString() });
        //    }
        //    if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
        //    }
        //}
    }
}
