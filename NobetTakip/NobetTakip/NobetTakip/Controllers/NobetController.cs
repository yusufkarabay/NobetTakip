using Microsoft.AspNetCore.Mvc;
using NobetTakip.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using NobetTakip.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using NobetTakip.Core.DTO;

namespace NobetTakip.Controllers
{
    [Route("nobet")]
    public class NobetController : BaseController
    {
        private readonly NobsisApiService apiService;

        public NobetController(NobsisApiService _apiService)
        {
            apiService = _apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Guid iGuid = Guid.Parse(HttpContext.Session.GetString("Nobsis_IsletmeId"));

            List<Nobet> ns = (List<Nobet>)await apiService.GetNobets(iGuid);
            foreach (Nobet nobet in ns)
                nobet.Personels = (List<Personel>)await GetNobetPersonels(nobet.NobetId);

            List<Personel> existingPrs = (List<Personel>)await apiService.GetPersonels(iGuid);

            NobetListViewModel nlvm = new NobetListViewModel();
            nlvm.DateString = DateTime.Now.ToString("yyyy-MM-dd");
            nlvm.PersonelId = Guid.Empty;
            nlvm.Personels = new SelectList(existingPrs, "PersonelId", "RealName");
            nlvm.Period = -1;
            nlvm.Nobetler = ns;

            return View(nlvm);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search(NobetListViewModel model)
        {
            Guid iGuid = Guid.Parse(HttpContext.Session.GetString("Nobsis_IsletmeId"));

            NobetSearchModel parameters = new NobetSearchModel();
            parameters.DateString = model.DateString;
            parameters.Period = model.Period;
            parameters.PersonelId = model.PersonelId;
            parameters.IsletmeId = iGuid;
            parameters.DateEnabled = model.DateEnabled;

            List<Nobet> ns = (List<Nobet>)await apiService.SearchNobet(parameters);
            foreach (Nobet nobet in ns)
                nobet.Personels = (List<Personel>)await GetNobetPersonels(nobet.NobetId);

            List<Personel> existingPrs = (List<Personel>)await apiService.GetPersonels(iGuid);

            NobetListViewModel nlvm = new NobetListViewModel();
            nlvm.DateEnabled = model.DateEnabled;
            nlvm.DateString = model.DateString;
            nlvm.PersonelId = model.PersonelId;
            nlvm.Personels = new SelectList(existingPrs, "PersonelId", "RealName");
            nlvm.Period = model.Period;
            nlvm.Nobetler = ns;

            return View("Index", nlvm);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Nobet(Guid id)
        {
            Nobet n = (Nobet)await apiService.GetNobet(id);
            if (n != null)
                n.Personels = (List<Personel>)await GetNobetPersonels(id);

            return View("NobetDetay", n);
        }

        [HttpGet("{id}/duzenle")]
        public async Task<IActionResult> Duzenle(Guid id)
        {
            Guid isletmeId = Guid.Parse(HttpContext.Session.GetString("Nobsis_IsletmeId"));
            Nobet nobet = (Nobet)await apiService.GetNobet(id);
            if (nobet != null)
                nobet.Personels = (List<Personel>)await GetNobetPersonels(id);

            NobetCreateViewModel ncvm = new NobetCreateViewModel();
            ncvm.Date = nobet.Date;
            ncvm.DateString = nobet.Date.ToString("yyyy-MM-dd");
            ncvm.Period = nobet.Period;
            ncvm.Personels = (List<Personel>)await apiService.GetPersonels(isletmeId);
            ncvm.NobetId = id;
            ncvm.SelectedPersonels = nobet.PersonelIdsArray;

            return View("NobetOlustur", ncvm);
        }

        [HttpGet("{id}/sil")]
        public async Task<IActionResult> Sil(Guid id)
        {
            string asd = await apiService.DeleteNobet(id);
            Console.WriteLine(asd);
            return RedirectToAction("Index", "Nobet");
        }

        [HttpGet("{id}/takvim")]
        public async Task<IActionResult> TakvimOlustur(Guid id)
        {
            Nobet nobet = (Nobet)await apiService.GetNobet(id);

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

        [HttpGet("olustur")]
        public async Task<IActionResult> Olustur()
        {
            Guid isletmeId = Guid.Parse(HttpContext.Session.GetString("Nobsis_IsletmeId"));

            NobetCreateViewModel ncvm = new NobetCreateViewModel();
            ncvm.Date = DateTime.Now;
            ncvm.DateString = DateTime.Now.ToString("yyyy-MM-dd");
            ncvm.Period = 0;
            ncvm.Personels = (List<Personel>)await apiService.GetPersonels(isletmeId);
            ncvm.NobetId = Guid.Empty;

            return View("NobetOlustur", ncvm);
        }

        [HttpPost("olustur")]
        public async Task<ActionResult> Olustur(NobetCreateViewModel ncvm)
        {
            Nobet n = new Nobet();
            n.NobetId = ncvm.NobetId;
            n.Date = DateTime.Parse(ncvm.DateString);
            n.PersonelIds = ncvm.SelectedPersonelsAsString;
            n.IsletmeId = Guid.Parse(HttpContext.Session.GetString("Nobsis_IsletmeId"));
            n.Period = ncvm.Period;

            if (n.NobetId == Guid.Empty)
                await apiService.CreateNobet(n);
            else
                await apiService.UpdateNobet(ncvm.NobetId, n);
            return RedirectToAction("Index", "Nobet");
        }

        [HttpGet("{myNobetId}/degistir")]
        public async Task<IActionResult> Degistir(Guid myNobetId)
        {
            Guid isletmeId = Guid.Parse(HttpContext.Session.GetString("Nobsis_IsletmeId"));
            Guid personelId = Guid.Parse(HttpContext.Session.GetString("Nobsis_PersonelId"));

            Nobet myNobet = (Nobet)await apiService.GetNobet(myNobetId);
            myNobet.Personels = (List<Personel>)await GetNobetPersonels(myNobetId);

            List<Nobet> nobetler = (List<Nobet>)await apiService.GetNobets(isletmeId);
            nobetler.RemoveAll(m => m.NobetId == myNobetId || m.PersonelIdsArray.Contains(personelId.ToString()));

            foreach (Nobet nb in nobetler)
                nb.Personels = (List<Personel>)await GetNobetPersonels(nb.NobetId);

            NobetDegistirViewModel nobetDegistirViewModel = new NobetDegistirViewModel();
            nobetDegistirViewModel.DateEnabled = true;
            nobetDegistirViewModel.DateString = DateTime.Now.ToString("yyyy-MM-dd");
            nobetDegistirViewModel.Period = 0;
            nobetDegistirViewModel.Personels = new SelectList((List<Personel>)await apiService.GetPersonels(isletmeId), "PersonelId", "RealName");
            nobetDegistirViewModel.MyNobetId = myNobetId;
            nobetDegistirViewModel.MyNobet = myNobet;
            nobetDegistirViewModel.Nobetler = nobetler;

            return View("NobetDegistir", nobetDegistirViewModel);
        }

        [HttpPost("{myNobetId}/degistir")]
        public async Task<IActionResult> Degistir(Guid myNobetId, NobetDegistirViewModel ndvm)
        {
            Guid isletmeId = Guid.Parse(HttpContext.Session.GetString("Nobsis_IsletmeId"));
            Guid personelId = Guid.Parse(HttpContext.Session.GetString("Nobsis_PersonelId"));

            Nobet myNobet = (Nobet)await apiService.GetNobet(myNobetId);
            myNobet.Personels = (List<Personel>)await GetNobetPersonels(myNobetId);

            NobetSearchModel nsc = new NobetSearchModel();
            nsc.DateEnabled = ndvm.DateEnabled;
            nsc.DateString = ndvm.DateString;
            nsc.Period = ndvm.Period;
            nsc.PersonelId = ndvm.PersonelId;
            nsc.IsletmeId = isletmeId;
            List<Nobet> nobetler = (List<Nobet>)await apiService.SearchNobet(nsc);
            nobetler.RemoveAll(m =>
                   m.NobetId == myNobetId 
                || m.PersonelIdsArray.Contains(personelId.ToString())
                || m.Date < DateTime.Now.Date
            );

            foreach (Nobet nb in nobetler)
                nb.Personels = (List<Personel>)await GetNobetPersonels(nb.NobetId);

            NobetDegistirViewModel returnModel = new NobetDegistirViewModel();
            returnModel.DateEnabled = ndvm.DateEnabled;
            returnModel.DateString = ndvm.DateString;
            returnModel.Period = ndvm.Period;
            returnModel.PersonelId = ndvm.PersonelId;
            returnModel.Personels = new SelectList((List<Personel>)await apiService.GetPersonels(isletmeId), "PersonelId", "RealName");
            returnModel.MyNobet = myNobet;
            returnModel.MyNobetId = myNobetId;
            returnModel.Nobetler = nobetler;
            return View("NobetDegistir", returnModel);
        }

        [HttpGet("degisim-onay")]
        public async Task<IActionResult> DegisimOnay([FromQuery] string myNobetId, string wantedNobetId)
        {
            
            Guid idMy, idWanted, idPersonel;
            idMy = Guid.Parse(myNobetId);
            idWanted = Guid.Parse(wantedNobetId);

            Nobet myNobet, wantedNobet;
            myNobet = (Nobet)await apiService.GetNobet(idMy);
            wantedNobet = (Nobet)await apiService.GetNobet(idWanted);

            myNobet.Personels = (List<Personel>)await GetNobetPersonels(idMy);
            wantedNobet.Personels = (List<Personel>)await GetNobetPersonels(idWanted);

            NobetDegisimOnayModel ndo = new NobetDegisimOnayModel();
            ndo.MyNobet = myNobet;
            ndo.WantedNobet = wantedNobet;

            return View("NobetDegisimOnay", ndo);
        }

        [HttpPost("degisim-onay")]
        public async Task<IActionResult> DegisimOnay(NobetDegisimOnayModel model)
        {
            Guid isletmeId = Guid.Parse(HttpContext.Session.GetString("Nobsis_IsletmeId"));
            Guid personelId = Guid.Parse(HttpContext.Session.GetString("Nobsis_PersonelId"));

            Degisim request = new Degisim();
            request.FirstNobetId = model.MyNobetId;
            request.SecondNobetId = model.WantedNobetId;
            request.IsletmeId = isletmeId;
            request.RequestDate = DateTime.Now.Date;
            request.FirstPersonelId = personelId;
            request.SecondPersonelId = model.PersonelId;

            Degisim justCreated = (Degisim)await apiService.CreateDegisim(request);
            if(justCreated.DegisimId != Guid.Empty)
            {
                Bildirim bildirim = new Bildirim();
                bildirim.DegisimId = justCreated.DegisimId;
                bildirim.PersonelId = model.PersonelId;
                bildirim.Date = DateTime.Now.Date;

                await apiService.CreateBildirim(bildirim);
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task<IEnumerable<Personel>> GetNobetPersonels(Guid nobetId)
        {
            return await apiService.GetNobetPersonels(nobetId);
        }
    }
}
