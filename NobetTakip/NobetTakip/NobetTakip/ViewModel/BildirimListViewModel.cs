using NobetTakip.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{
    public class BildirimListViewModel : ErrorViewModel
    {
        public BildirimListViewModel() { }

        public List<Bildirim> Seen;
        public List<Bildirim> Unseen;
    }
}
