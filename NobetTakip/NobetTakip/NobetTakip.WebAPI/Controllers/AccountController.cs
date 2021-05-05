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
        public async Task<ActionResult<Personel>> Login([FromBody] string lvm)
        {
            JObject jo = JObject.Parse(lvm);
            Personel foundPersonel = await _context.Personels.FirstAsync(p => p.MailAddress.Equals(jo["MailAddress"].ToString()) && p.Password.Equals(jo["Password"].ToString()));

            if (foundPersonel == null)
            {
                return Unauthorized();
            }

            return foundPersonel;
        }
    }
}
