using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NobetTakip.Core.Models;

namespace NobetTakip.Controllers
{
    public class IsletmeController : BaseController
    {
        private readonly NobsisApiService apiService;
        public IsletmeController(NobsisApiService _apiService)
        {
            apiService = _apiService;
        }

        public IActionResult Index()
        {
            string isletmeId = HttpContext.Session.GetString("Nobsis_IsletmeId").ToString();
            Guid iGuid = Guid.Parse(isletmeId);
            Isletme isletme = apiService.GetIsletme(iGuid).Result;
            return View("View", isletme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Isletme isletme)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Error", "Home");

            string isletmeId = HttpContext.Session.GetString("Nobsis_IsletmeId").ToString();
            Guid iGuid = Guid.Parse(isletmeId);
            Isletme existing = apiService.GetIsletme(iGuid).Result;
            if (existing == null)
                return RedirectToAction("Error", "Home");

            existing.IsletmeAdi = isletme.IsletmeAdi;
            existing.IsletmeKod = isletme.IsletmeKod;
            existing.MailAddress = isletme.MailAddress;
            existing.Phone = isletme.Phone;

            try
            {
                apiService.UpdateIsletme(existing.IsletmeId, existing);
                HttpContext.Session.SetString("Nobsis_IsletmeAdi", existing.IsletmeAdi);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
