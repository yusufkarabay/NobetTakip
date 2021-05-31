using Microsoft.AspNetCore.Mvc;
using NobetTakip.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NobetTakip.ViewModel;

namespace NobetTakip.Controllers
{
    [Route("bildirim")]
    public class BildirimController : Controller
    {
        private readonly NobsisApiService apiService;

        public BildirimController(NobsisApiService _apiService)
        {
            apiService = _apiService;
        }

        public async Task<IActionResult> Index()
        {
            string personelId = HttpContext.Session.GetString("Nobsis_PersonelId");
            List<Bildirim> bildirimler = (List<Bildirim>) await apiService.GetBildirimler(Guid.Parse(personelId));
            foreach (Bildirim b in bildirimler)
                b.Degisim = (Degisim)await apiService.GetDegisim(b.DegisimId);

            List<Bildirim> seen, unseen;
            seen = bildirimler.Where(m => m.Seen == false).ToList();
            unseen = bildirimler.Where(m => m.Seen == true).ToList();

            BildirimListViewModel blvm = new BildirimListViewModel();
            blvm.Seen = seen;
            blvm.Unseen = unseen;

            return View(blvm);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get()
        {
            return View();
        }
    }
}
