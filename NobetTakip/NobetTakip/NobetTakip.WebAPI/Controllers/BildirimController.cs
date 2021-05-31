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
    public class BildirimController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BildirimController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("count/{personelId}")]
        public async Task<ActionResult<int>> GetBildirimCount(Guid personelId)
        {
            int count = await _context.Bildirimler.Where(m => m.PersonelId == personelId && m.Seen == false).CountAsync();
            return count;
        }

        // GET: api/bildirim/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bildirim>> GetBildirim(Guid id)
        {
            var bildirim =
                await _context.Bildirimler
                .Include(m => m.Degisim)
                .Where(m => m.BildirimId == id)
                .FirstOrDefaultAsync();

            if (bildirim == null)
            {
                return NotFound();
            }

            return bildirim;
        }

        [HttpPost]
        public async Task<ActionResult<Nobet>> CreateBildirim(Bildirim bildirim)
        {
            _context.Bildirimler.Add(bildirim);
            await _context.SaveChangesAsync();

            return Ok(bildirim);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Degisim>> ChangeBildirimState(Guid id)
        {
            Bildirim bildirim = await _context.Bildirimler.Where(m => m.DegisimId == id).FirstOrDefaultAsync();
            bildirim.Seen = true;
            _context.Entry(bildirim).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
