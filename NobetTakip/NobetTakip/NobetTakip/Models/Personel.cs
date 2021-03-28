using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.Models
{
    public class Personel
    {

        public string RealName;
        public string Username { get; set; }
        public string MailAddress { get; set; }
        public string GSMNo;
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public bool IsAdmin; // yönetici mi değil mi ?
    }
}
