using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NobetTakip.Core.Models;
using NobetTakip.WebAPI;
using Newtonsoft.Json.Linq;
using NobetTakip.Core.DTO;

namespace NobetTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NobetController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NobetController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Nobet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nobet>> GetNobet(Guid id)
        {
            var nobet = await _context.Nobets.FindAsync(id);

            if (nobet == null)
            {
                return NotFound();
            }

            return nobet;
        }

        // PUT: api/Nobet/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNobet(Guid id, Nobet nobet)
        {
            if (id != nobet.NobetId)
            {
                return BadRequest();
            }

            _context.Entry(nobet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NobetExists(id))
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

        // POST: api/Nobet
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Nobet>> PostNobet(Nobet nobet)
        {
            _context.Nobets.Add(nobet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNobet", new { id = nobet.NobetId }, nobet);
        }

        // DELETE: api/Nobet/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Nobet>> DeleteNobet(Guid id)
        {
            var nobet = await _context.Nobets.FindAsync(id);
            if (nobet == null)
            {
                return NotFound();
            }

            _context.Nobets.Remove(nobet);
            await _context.SaveChangesAsync();

            return nobet;
        }

        [HttpGet("{id}/personels")]
        public async Task<ActionResult<IEnumerable<Personel>>> GetNobetPersonels(Guid id)
        {
            string personelIds = null;
            Nobet n = await _context.Nobets.Where(n => n.NobetId == id).FirstOrDefaultAsync();
            personelIds = n.PersonelIds;

            List<string> personelIdArray = personelIds.Split(',').ToList();
            return await _context.Personels.Where(p => personelIdArray.Contains(p.PersonelId.ToString())).ToListAsync();
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Nobet>>> SearchNobet(NobetSearchModel model)
        {
            DateTime mDate = DateTime.Parse(model.DateString);

            var nobets =
                await _context.Nobets
                .Where(m =>
                    m.IsletmeId == model.IsletmeId &&
                    (model.DateEnabled == false || m.Date == mDate) &&
                    (model.PersonelId == Guid.Empty || m.PersonelIds.Contains(model.PersonelId.ToString())) &&
                    (model.Period == -1 || m.Period == model.Period)
                )
                .OrderBy(m => m.Date)
                .ToListAsync();

            return nobets;
        }

        private bool NobetExists(Guid id)
        {
            return _context.Nobets.Any(e => e.NobetId == id);
        }
    }
}
