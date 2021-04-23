using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{

    public interface IAuthViewModel
    {

    }
    public class AuthViewModel : IAuthViewModel
    {
        public string RealName;
        public string IsletmeAdi;
        public string PhotoUrl;
    }
}
