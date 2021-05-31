using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobetTakip.WebAPI;
using NobetTakip.Core.Models;

namespace NobetTakip.WebAPI.Controllers
{
    [Route("api/isletme")]
    [ApiController]
    public class IsletmeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IsletmeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}/personels")]
        public async Task<ActionResult<IEnumerable<Personel>>> GetIsletmePersonels(Guid id)
        {
            return await _context.Personels
                .Where(p => p.IsletmeId.Equals(id)).ToListAsync();
        }

        // GET: api/Nobet
        [HttpGet("{id}/nobets")]
        public async Task<ActionResult<IEnumerable<Nobet>>> GetIsletmeNobets(Guid id)
        {
            return await _context.Nobets
                .Where(n => n.IsletmeId.Equals(id) && n.Date >= DateTime.Now.Date)
                .OrderBy(n => n.Date)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Isletme>>> GetIsletmeler()
        {
            return await _context.Isletmeler.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Isletme>> GetIsletme(Guid id)
        {
            var isletme = await _context.Isletmeler.FindAsync(id);

            if (isletme == null)
            {
                return NotFound();
            }

            return isletme;
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Isletme>> GetIsletme(string code)
        {
            var isletme = await _context.Isletmeler.Where(i => i.IsletmeKod.Equals(code)).FirstOrDefaultAsync();

            if (isletme == null)
            {
                return NotFound();
            }

            return isletme;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutIsletme(Guid id, Isletme isletme)
        {
            if (id != isletme.IsletmeId)
            {
                return BadRequest();
            }

            _context.Entry(isletme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsletmeExists(id))
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
        public async Task<ActionResult<Isletme>> PostIsletme(Isletme isletme)
        {
            _context.Isletmeler.Add(isletme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIsletme", new { id = isletme.IsletmeId }, isletme);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Isletme>> DeleteIsletme(Guid id)
        {
            var isletme = await _context.Isletmeler.FindAsync(id);
            if (isletme == null)
            {
                return NotFound();
            }

            _context.Isletmeler.Remove(isletme);
            await _context.SaveChangesAsync();

            return isletme;
        }

        private bool IsletmeExists(Guid id)
        {
            return _context.Isletmeler.Any(e => e.IsletmeId == id);
        }
    }
}
