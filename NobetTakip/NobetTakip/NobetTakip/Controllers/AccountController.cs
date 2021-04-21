using Microsoft.AspNetCore.Mvc;
using NobetTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using NobetTakip.ViewModel;

namespace NobetTakip.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext _context;

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
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try { 
                    Personel p = _context.Personels.First(p => p.MailAddress.Equals(loginViewModel.MailAddress) && p.Password.Equals(loginViewModel.Password));
                    if(loginViewModel.RememberMe) { 
                        Response.Cookies.Append("NO_UserMailAdress", p.MailAddress);
                        Response.Cookies.Append("NO_UserRealName", p.RealName);
                        //Response.Cookies.Append("NO_UserIsletme", p.Isletme.IsletmeAdi);
                    }

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
