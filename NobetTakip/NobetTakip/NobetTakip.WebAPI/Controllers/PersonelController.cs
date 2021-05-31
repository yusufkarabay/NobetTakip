using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using NobetTakip.Core.Models;
using NobetTakip.WebAPI;

namespace NobetTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonelController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonelController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personel>>> GetPersonels()
        {
            return await _context.Personels.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Personel>> GetPersonel(Guid id)
        {
            var personel = await _context.Personels.FindAsync(id);

            if (personel == null)
            {
                return NotFound();
            }

            return personel;
        }

        [HttpGet("mail/{mail}")]
        public async Task<ActionResult<Personel>> GetPersonel(string mail)
        {
            var personel = await _context.Personels.Where(p => p.MailAddress.Equals(mail)).FirstOrDefaultAsync();

            if (personel == null)
            {
                return NotFound();
            }

            return personel;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonel(Guid id,[FromBody] Personel personel)
        {
            //Personel personel = JsonConvert.DeserializeObject<Personel>(personelJson);
            if (id != personel.PersonelId)
            {
                return BadRequest();
            }

            _context.Entry(personel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Personel>> PostPersonel([FromBody] Personel personel)
        {
            try { 
            _context.Personels.Add(personel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonel", new { id = personel.PersonelId }, personel);
            } catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Personel>> DeletePersonel(Guid id)
        {
            var personel = await _context.Personels.FindAsync(id);
            if (personel == null)
            {
                return NotFound();
            }

            _context.Personels.Remove(personel);
            await _context.SaveChangesAsync();

            return personel;
        }

        [HttpGet]
        [Route("{id}/nobetler")]
        public async Task<ActionResult<IEnumerable<Nobet>>> GetNobetsForPersonel(Guid id)
        {
            try
            {
                string personelId = id.ToString();
                var nobets = from n in _context.Nobets
                                where EF.Functions.Like(n.PersonelIds, $"%{personelId}%") && n.Date >= DateTime.Now.Date
                                orderby n.Date
                                select n;

                return await nobets.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new List<Nobet>();
        }

        [HttpGet]
        [Route("{id}/bildirimler")]
        public async Task<ActionResult<IEnumerable<Bildirim>>> GetBildirimlerForPersonel(Guid id)
        {
            try
            {
                string personelId = id.ToString();
                var bildirimler = 
                    _context.Bildirimler
                    .Where(m => m.PersonelId == id)
                    .OrderByDescending(m => m.Date)
                    .ToListAsync();

                return await bildirimler;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new List<Bildirim>();
        }

        private bool PersonelExists(Guid id)
        {
            return _context.Personels.Any(e => e.PersonelId == id);
        }
    }
}
