using Microsoft.AspNetCore.Mvc;
using NobetTakip.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NobetTakip.Controllers
{
    [Route("degisim")]
    public class DegisimController : Controller
    {
        private readonly NobsisApiService apiService;

        public DegisimController(NobsisApiService _apiService)
        {
            apiService = _apiService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDegisim(Guid id)
        {
            Guid personelId = Guid.Parse(HttpContext.Session.GetString("Nobsis_PersonelId"));
            Degisim degisim = (Degisim)await apiService.GetDegisim(id);

            Nobet nobet1, nobet2;
            nobet1 = (Nobet)await apiService.GetNobet(degisim.FirstNobetId);
            nobet2 = (Nobet)await apiService.GetNobet(degisim.SecondNobetId);

            nobet1.Personels = (List<Personel>)await apiService.GetNobetPersonels(degisim.FirstNobetId);
            nobet2.Personels = (List<Personel>)await apiService.GetNobetPersonels(degisim.SecondNobetId);

            degisim.FirstNobet = nobet1;
            degisim.SecondNobet = nobet2;

            await apiService.ChangeBildirimState(id);

            return View("DegisimDetay", degisim);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> PostDegisim(Guid id)
        {
            try { 
                await apiService.AcceptDegisim(id);

                Guid personelId = Guid.Parse(HttpContext.Session.GetString("Nobsis_PersonelId"));
                Degisim degisim = (Degisim)await apiService.GetDegisim(id);

                Nobet nobet1, nobet2;
                nobet1 = (Nobet)await apiService.GetNobet(degisim.FirstNobetId);
                nobet2 = (Nobet)await apiService.GetNobet(degisim.SecondNobetId);

                List<string> ids = nobet1.PersonelIdsArray;
                ids.Remove(degisim.FirstPersonelId.ToString());
                ids.Add(degisim.SecondPersonelId.ToString());
                nobet1.PersonelIds = string.Join(",", ids);
                ids = null;

                ids = nobet2.PersonelIdsArray;
                ids.Remove(degisim.SecondPersonelId.ToString());
                ids.Add(degisim.FirstPersonelId.ToString());
                nobet2.PersonelIds = string.Join(",", ids);

                await apiService.UpdateNobet(degisim.FirstNobetId, nobet1);
                await apiService.UpdateNobet(degisim.SecondNobetId, nobet2);

                await apiService.ChangeBildirimState(id);
            }
            catch (Exception ex)
            {

            }


            return RedirectToAction("Index", "Home");
        }

    }
}
