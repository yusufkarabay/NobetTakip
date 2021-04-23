using NobetTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{
    public class HomeViewModel : AuthViewModel
    {
        public int BildirimSayisi { get; set; }
        public List<Nobet> Nobetler { get; set; }
    }
}
