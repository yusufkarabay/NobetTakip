using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: api/Personel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personel>>> GetPersonels()
        {
            return await _context.Personels.ToListAsync();
        }

        // GET: api/Personel/5
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

        // PUT: api/Personel/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonel(Guid id, Personel personel)
        {
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

        // POST: api/Personel
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Personel>> PostPersonel(Personel personel)
        {
            _context.Personels.Add(personel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonel", new { id = personel.PersonelId }, personel);
        }

        // DELETE: api/Personel/5
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

        private bool PersonelExists(Guid id)
        {
            return _context.Personels.Any(e => e.PersonelId == id);
        }
    }
}
