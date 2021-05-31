using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        public string IsletmeKodu { get; set; }
        public string RealName { get; set; }
        public string MailAddress { get; set; }
        public string GSMNo { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}
