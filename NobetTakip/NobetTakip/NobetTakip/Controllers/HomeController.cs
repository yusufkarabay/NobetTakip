using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using NobetTakip.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NobetTakip.Core.Models;
using Newtonsoft.Json;
using System.Net.Http;

namespace NobetTakip.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {

        AppDbContext _context;
        private readonly NobsisApiService apiService;

        public HomeController(AppDbContext context, NobsisApiService _apiService)
        {
            _context = context;
            apiService = _apiService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            Guid personelId = Guid.Parse(HttpContext.Session.GetString("Nobsis_PersonelId").ToString());
            List<Nobet> ns = (List<Nobet>)await apiService.GetPersonelNobets(personelId);

            foreach (Nobet nobet in ns)
                nobet.Personels = (List<Personel>)await apiService.GetNobetPersonels(nobet.NobetId);

            int bildirimCount = await apiService.GetBildirimCount(personelId);

            HomeViewModel hvm = new HomeViewModel();
            hvm.Nobetler = ns;
            hvm.BildirimSayisi = bildirimCount;
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
