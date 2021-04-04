using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NobetTakip.Models;
using NobetTakip.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NobetTakip.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        List<Personel> personels = new List<Personel>();
        private readonly Random rnd = new Random(0);


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            Personel p1 = new Personel();
            p1.RealName = "Hasan Cemre Ok";

            Personel p2 = new Personel();
            p2.RealName = "Yusuf Karabay";

            Personel p3 = new Personel();
            p3.RealName = "Ahmet Uzun";

            Personel p4 = new Personel();
            p4.RealName = "Onur Türkoğlu";

            Personel p5 = new Personel();
            p5.RealName = "Mehmet Ağır";

            personels.Add(p1);
            personels.Add(p2);
            personels.Add(p3);
            personels.Add(p4);
            personels.Add(p5);
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
                List<Personel> ps = new List<Personel>();
                int num = rnd.Next(0, 8);

                for (int k = 0; k < num; k++)
                {
                    ps.Add(GetRandomPersonel());
                }

                Nobet n2 = new Nobet();
                n2.IsEnYakin = false;
                n2.DayNight = i % 2 == 0;
                n2.NobetId = new Guid();
                n2.Period = i % 2 == 0 ? 1 : 0;
                n2.Type = 1;
                n2.Date = DateTime.Now.AddDays(i);
                n2.Nobetciler = ps;
                nobetler.Add(n2);

            }

            int bildirimSayisi = 3;

            HomeViewModel hvm = new HomeViewModel();
            hvm.Nobetler = nobetler;
            hvm.BildirimSayisi = bildirimSayisi;

            return View(hvm);
        }

        private Personel GetRandomPersonel()
        {
            int num = rnd.Next(0, 4);
            return personels[num];
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
    
        [Route("nobet/{id}")]
        public IActionResult NobetDetay()
        {

            Nobet n1 = new Nobet();
            n1.IsEnYakin = true;
            n1.DayNight = false;
            n1.NobetId = new Guid();
            n1.Period = 0;
            n1.Type = 0;
            n1.Nobetciler = personels;
            n1.Date = DateTime.Now;


            return View(n1);
        }
    
        [Route("nobet/takvim")]
        public ActionResult TakvimOlustur()
        {
            Nobet nobet = new Nobet();
            nobet.IsEnYakin = true;
            nobet.DayNight = false;
            nobet.NobetId = new Guid();
            nobet.Period = 0;
            nobet.Type = 0;
            nobet.Nobetciler = personels;
            nobet.Date = DateTime.Now;

            DateTime eventStart = nobet.Date;
            StringBuilder str = new StringBuilder(); 
            str.AppendLine("BEGIN:VCALENDAR"); 
            str.AppendLine("PRODID:-//Nöbet Paylaşım Bilgisi // Nöbsis"); 
            str.AppendLine("VERSION:2.0"); 
            str.AppendLine("METHOD:REQUEST");
            str.AppendLine("BEGIN:VEVENT"); 
            str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", eventStart)); 
            str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.Now)); 
            str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", eventStart.AddMinutes(+30))); 
            str.AppendLine("LOCATION: Online"); 
            str.AppendLine(string.Format("UID:{0}", Guid.NewGuid())); 
            str.AppendLine(string.Format("DESCRIPTION:{0}", nobet.ShareText)); 
            str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", nobet.ShareText)); 
            str.AppendLine(string.Format("SUMMARY:{0}", "Nöbsis Nöbet Paylaşımı")); 
            str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", "info@nobsis.com")); 
            str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", "Hasan Cemre Ok", "okhasancemre@gmail.com")); 
            str.AppendLine("BEGIN:VALARM"); str.AppendLine("TRIGGER:-PT15M"); 
            str.AppendLine("ACTION:DISPLAY"); str.AppendLine("DESCRIPTION:Reminder"); 
            str.AppendLine("END:VALARM"); str.AppendLine("END:VEVENT"); 
            str.AppendLine("END:VCALENDAR");

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(str.ToString()));
            return File(ms, "text/calendar");
        }

    }
}
