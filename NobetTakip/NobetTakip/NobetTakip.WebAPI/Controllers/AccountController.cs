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

namespace NobetTakip.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<Personel>> Login([FromBody] Personel personel)
        {
            Personel foundPersonel = 
                await _context.Personels
                .Include(s => s.Isletme)
                .FirstAsync(
                    p => p.MailAddress.Equals(personel.MailAddress) 
                    && p.Password.Equals(personel.Password));

            if (foundPersonel == null)
            {
                return Unauthorized("Mailadresi ve ya şifre ile eşleşen kullanıcı bulunamadı.");
            } else if(foundPersonel.Isletme.IsActive == false)
            {
                return Forbid("Bağlı olduğunuz işletme aktif değil.");
            }

            return foundPersonel;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<Personel>> Register([FromBody] Personel personel)
        {
            Personel foundPersonel =
                await _context.Personels.Where(p => p.MailAddress.Equals(personel.MailAddress)).FirstOrDefaultAsync();

            if (foundPersonel != null)
            {
                return Conflict("Girilen bilgilere ait bir kullanıcı zaten mevcut");
            }
            else if (foundPersonel.Isletme.IsActive == false)
            {
                return Forbid("Kayıt olunmak istenen işletme hesabı aktif değil");
            }

            await _context.Personels.AddAsync(personel);
            await _context.SaveChangesAsync();

            return foundPersonel;
        }

    }
}
