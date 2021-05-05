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
    [Route("api/[controller]")]
    [ApiController]
    public class IsletmeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public IsletmeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Isletmes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Isletme>>> GetIsletmeler()
        {
            return await _context.Isletmeler.ToListAsync();
        }

        // GET: api/Isletmes/5
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

        // PUT: api/Isletmes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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

        // POST: api/Isletmes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Isletme>> PostIsletme(Isletme isletme)
        {
            _context.Isletmeler.Add(isletme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIsletme", new { id = isletme.IsletmeId }, isletme);
        }

        // DELETE: api/Isletmes/5
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
