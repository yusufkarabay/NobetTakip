using Microsoft.AspNetCore.Mvc;
using NobetTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

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
            
            return View(new Personel());
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Personel userModel)
        {
            if (ModelState.IsValid)
            {
                try { 
                    Personel p = _context.Personels.First(p => p.MailAddress.Equals(userModel.MailAddress) && p.Password.Equals(userModel.Password));
                    return RedirectToAction("Index", "Home");
                }
                catch (InvalidOperationException iex)
                {
                    // girilen bilgilere ait kullanıcı bulunamadı.
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(userModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new Personel());
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(Personel userModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add<Personel>(userModel);
                int affected = _context.SaveChanges();

                if (affected > 0)
                {
                    return RedirectToAction("Login", "Account");
                } else
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(userModel);
        }
    }
}
