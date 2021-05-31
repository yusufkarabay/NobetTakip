using NobetTakip.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{
    public class NobetDegisimOnayModel
    {
        public Nobet MyNobet { get; set; }
        public Nobet WantedNobet { get; set; }

        public Guid MyNobetId { get; set; }
        public Guid WantedNobetId { get; set; }
        public Guid PersonelId { get; set; }
    }
}
