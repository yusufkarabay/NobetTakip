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
    public class DegisimController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DegisimController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/degisim/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Degisim>> GetDegisim(Guid id)
        {
            var degisim = await _context.Degisimler
                .Include(m => m.FirstPersonel)
                .Where(m => m.DegisimId == id)
                .FirstOrDefaultAsync();

            if (degisim == null)
            {
                return NotFound();
            }

            return degisim;
        }

        [HttpPost]
        public async Task<ActionResult<Degisim>> CreateDegisim(Degisim model)
        {
            await _context.Degisimler.AddAsync(model);
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Degisim>> AcceptDegisim(Guid id)
        {
            Degisim degisim = await _context.Degisimler.Where(m => m.DegisimId == id).FirstOrDefaultAsync();
            degisim.IsAccepted = true;
            _context.Entry(degisim).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
