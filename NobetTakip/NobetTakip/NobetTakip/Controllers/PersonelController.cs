using Microsoft.AspNetCore.Mvc;
using NobetTakip.Core.Models;
using NobetTakip.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NobetTakip.Controllers
{
    [Route("personel")]
    public class PersonelController : BaseController
    {
        private readonly AppDbContext context;
        private readonly NobsisApiService apiService;

        public PersonelController(AppDbContext _context, NobsisApiService _apiService)
        {
            context = _context;
            apiService = _apiService;
        }

        public IActionResult Index()
        {
            string isletmeId = HttpContext.Session.GetString("Nobsis_IsletmeId");
            List<Personel> personels = apiService.GetPersonels(Guid.Parse(isletmeId)).Result.ToList();
            return View(personels);
        }

        [HttpGet("{id}")]
        public IActionResult GetPersonel(Guid id)
        {
            Personel personel = apiService.GetPersonel(id).Result; 
            return View("PersonelDetay", personel);
        }

        
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Personel personel)
        {
            try
            {
                Personel existingPersonel = context.Personels.Where(p => p.PersonelId.Equals(id)).FirstOrDefault();
                existingPersonel.GSMNo = personel.GSMNo;
                existingPersonel.MailAddress = personel.MailAddress;
                existingPersonel.RealName = personel.RealName;
                existingPersonel.Password = personel.Password;

                string result = apiService.UpdatePersonel(id, existingPersonel).Result;
                
                return RedirectToAction("Index", "Personel");
            } catch(Exception ex)
            {
                Console.Write(ex.Message);
                return View("PersonelDetay", personel);
            }
        }

        [HttpGet("{id}/sil")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                string result = await apiService.DeletePersonel(id);
                return RedirectToAction("Index", "Personel");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return View();
        }
    }
}
