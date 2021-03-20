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
            //User user = new User() { MailAddress = "yusufkarabay21@gmail.com", Password = "süleymansüleyman" };
            return View(new User());
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User userModel)
        {
            if (ModelState.IsValid)
            {
                if (userModel.MailAddress == "yusufkarabay21@gmail.com" && userModel.Password == "1234567")
                {
                    return RedirectToAction("Index", "Home");
                } else
                {
                    return RedirectToAction("Error", "Homee");

                    //deneme
                }
            }

            return View(userModel);
        }
    }
}
