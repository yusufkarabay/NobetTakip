using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NobetTakip.Models;
using NobetTakip.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Nobet> nobetler = new List<Nobet>();
            Nobet n1 = new Nobet();
            n1.IsEnYakin = true;
            n1.DayNight = false;
            n1.NobetId = new Guid();
            n1.Period = 0;
            n1.Type = 0;
            n1.Date = DateTime.Now;
            nobetler.Add(n1);

            for (int i = 0; i < 11; i++)
            {
                Nobet n2 = new Nobet();
                n2.IsEnYakin = false;
                n2.DayNight = i % 2 == 0;
                n2.NobetId = new Guid();
                n2.Period = i % 2 == 0 ? 1 : 0;
                n2.Type = 1;
                n2.Date = DateTime.Now.AddDays(i);
                nobetler.Add(n2);

            }

            int bildirimSayisi = 3;

            HomeViewModel hvm = new HomeViewModel();
            hvm.Nobetler = nobetler;
            hvm.BildirimSayisi = bildirimSayisi;

            return View(hvm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
