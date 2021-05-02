using Microsoft.AspNetCore.Mvc;
using NobetTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using NobetTakip.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace NobetTakip.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _context;
        private IAuthViewModel _avm;

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies["NO_UserMailAdress"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try { 
                    Personel p = _context.Personels.First(p => p.MailAddress.Equals(loginViewModel.MailAddress) && p.Password.Equals(loginViewModel.Password));
                    //_avm.SetRealName(p.RealName);
                    //_avm.SetIsletmeAdi(p.MailAddress);
                    
                    HttpContext.Session.SetString("RealName", p.RealName);
                    HttpContext.Session.SetString("IsletmeAdi", p.MailAddress);

                    List<Claim> userClaims = new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, p.PersonelId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Email, p.MailAddress));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, p.RealName));
                    userClaims.Add(new Claim("IsletmeId", p.IsletmeId.ToString()));

                    if (p.IsAdmin)
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                        HttpContext.Session.SetString("IsAdmin", "admin");
                    }

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = loginViewModel.RememberMe
                    };

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties
                    );

                    return RedirectToAction("Index", "Home");
                }
                catch (InvalidOperationException iex)
                {
                    // girilen bilgilere ait kullanıcı bulunamadı.
                    return RedirectToAction("Error", "Home");
                }
            }

            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Logout()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel rvModel)
        {
            if (ModelState.IsValid)
            {
                Isletme isletme = null;
                Personel existing = null;

                try {
                    isletme = _context.Isletmeler.First(p => p.IsletmeKod.Equals(rvModel.IsletmeKodu));
                }
                catch (InvalidOperationException iex)
                {
                    // girilen işletme koduna ait bir işletme bulunamadı
                    return RedirectToAction("Error", "Home");
                }

                if(isletme.IsActive == false)
                {
                    // kayıt olunmak istenen işletme aktif değil.
                    return RedirectToAction("Error", "Home");
                }

                try {
                    existing = _context.Personels.First(p => p.MailAddress.Equals(rvModel.MailAddress));
                }
                catch (InvalidOperationException iex) { }

                if(existing != null)
                {
                    // girilen mail adresi zaten bir personele ait. ikinci kez kullanılamaz.
                    return RedirectToAction("Error", "Home");
                }

                if (!rvModel.Password.Equals(rvModel.RePassword))
                {
                    // şifreleriniz uyuşmuyor.
                    return RedirectToAction("Error", "Home");
                }

                Personel newPersonel = new Personel();
                newPersonel.MailAddress = rvModel.MailAddress;
                newPersonel.Isletme = isletme;
                newPersonel.GSMNo = rvModel.GSMNo;
                newPersonel.RealName = rvModel.RealName;
                newPersonel.Password = rvModel.Password;

                _context.Add(newPersonel);
                int affected = _context.SaveChanges();

                if (affected > 0)
                {
                    return RedirectToAction("Login", "Account");
                } else
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            return View();
        }
    }
}
