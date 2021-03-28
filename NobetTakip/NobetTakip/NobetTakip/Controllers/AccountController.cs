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
                if (userModel.MailAddress == "yusufkarabay21@gmail.com" && userModel.Password == "123456")
                {
                    return RedirectToAction("Index", "Home");
                } else
                {
                    return RedirectToAction("Error", "Home");                  
                }
            }

            return View(userModel);
        }
    }
}
