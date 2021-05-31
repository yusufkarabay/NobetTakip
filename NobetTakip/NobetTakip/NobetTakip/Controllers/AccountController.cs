using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using NobetTakip.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using NobetTakip.Core.Models;
using System.Text;

namespace NobetTakip.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _context;
        private IAuthViewModel _avm;
        NobsisApiService apiService;

        public AccountController(AppDbContext appDbContext, NobsisApiService _apiService)
        {
            _context = appDbContext;
            apiService = _apiService;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies["NO_UserMailAdress"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel { ReturnPath = HttpContext.Request.Query["returnPath"].ToString() });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try {
                    LoginViewModel lvm = new LoginViewModel();
                    lvm = loginViewModel;

                    Personel p = new Personel
                    {
                        MailAddress = loginViewModel.MailAddress,
                        Password = loginViewModel.Password
                    };

                    try { 
                        p = apiService.Login(p).Result;
                    } catch(Exception ex)
                    {
                        lvm.HasError = true;
                        lvm.ErrorMessage = "Girdiğiniz bilgilerle eşleşen bir kullanıcı bulunamadı";

                        return View(lvm);
                    }

                    if (p == null)
                    {
                        lvm.HasError = true;
                        lvm.ErrorMessage = "Girdiğiniz bilgilerle eşleşen bir kullanıcı bulunamadı";

                        return View(lvm);
                    }

                    if(p.Isletme.IsActive != true)
                    {
                        lvm.HasError = true;
                        lvm.ErrorMessage = "İşletme hesabınız aktif değil";

                        return View(lvm);
                    }

                    List<Claim> userClaims = new List<Claim>();
                    userClaims.Add(new Claim("Nobsis_RealName", p.RealName));
                    userClaims.Add(new Claim("Nobsis_PersonelId", p.PersonelId.ToString()));
                    userClaims.Add(new Claim("Nobsis_MailAddress", p.MailAddress));
                    userClaims.Add(new Claim("Nobsis_Isletme", p.Isletme.IsletmeAdi));
                    userClaims.Add(new Claim("Nobsis_IsletmeId", p.IsletmeId.ToString()));

                    HttpContext.Session.SetString("Nobsis_RealName", p.RealName);
                    HttpContext.Session.SetString("Nobsis_PersonelId", p.PersonelId.ToString());
                    HttpContext.Session.SetString("Nobsis_MailAddress", p.MailAddress);
                    HttpContext.Session.SetString("Nobsis_Isletme", p.Isletme.IsletmeAdi);
                    HttpContext.Session.SetString("Nobsis_IsletmeId", p.Isletme.IsletmeId.ToString());

                    if (p.IsAdmin)
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, "admin"));
                        HttpContext.Session.SetString("Nobsis_IsAdmin", "admin");
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

                    if (!string.IsNullOrEmpty(loginViewModel.ReturnPath) && !string.IsNullOrWhiteSpace(loginViewModel.ReturnPath))
                        return Redirect(loginViewModel.ReturnPath);
                    else
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
            RegisterViewModel rvm = new RegisterViewModel();
            return View(rvm);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel rvModel)
        {
            if (ModelState.IsValid)
            {
                RegisterViewModel rvm = new RegisterViewModel();
                rvm = rvModel;

                if (!rvModel.Password.Equals(rvModel.RePassword))
                {
                    rvm.HasError = true;
                    rvm.ErrorMessage = "Girdiğiniz şifreler birbiriyle uyuşmuyor";

                    return View(rvm);
                }

                Isletme isletme = null;
                Personel existing = null;

                try {
                    isletme =  apiService.GetIsletme(rvModel.IsletmeKodu).Result;
                }
                catch (Exception iex)
                {
                    rvm.HasError = true;
                    rvm.ErrorMessage = "Girdiğiniz işletme kodu herhangi bir işletmeye ait değildir";

                    return View(rvm);
                }

                if (isletme == null)
                {
                    rvm.HasError = true;
                    rvm.ErrorMessage = "Girdiğiniz işletme kodu herhangi bir işletmeye ait değildir";

                    return View(rvm);
                }

                if (isletme != null && isletme.IsActive == false)
                {
                    rvm.HasError = true;
                    rvm.ErrorMessage = "İşletme hesabınız aktif değil, bu sebeple kaydınız yapılamadı";

                    return View(rvm);
                }

                try {
                    existing = apiService.GetPersonel(rvModel.MailAddress).Result;
                }
                catch (Exception ex) {
                    
                }

                if(existing != null)
                {
                    rvm.HasError = true;
                    rvm.ErrorMessage = "Girdiğiniz mail adresi başka bir hesapta kullanılıyor";

                    return View(rvm);
                }

                Personel newPersonel = new Personel();
                newPersonel.MailAddress = rvModel.MailAddress;
                newPersonel.IsletmeId = isletme.IsletmeId;
                newPersonel.GSMNo = rvModel.GSMNo;
                newPersonel.RealName = rvModel.RealName;
                newPersonel.Password = rvModel.Password;

                Personel pNew = apiService.CreatePersonel(newPersonel).Result;

                if (pNew.IsletmeId != Guid.Empty)
                {
                    return RedirectToAction("Login", "Account");
                } else
                {
                    rvm.HasError = true;
                    rvm.ErrorMessage = "Girdiğiniz işletme kodu herhangi bir işletmeye ait değildir";

                    return View(rvm);
                }
            }

            return View();
        }
    }
}
