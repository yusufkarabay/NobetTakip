using Microsoft.AspNetCore.Mvc.Rendering;
using NobetTakip.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{
    public class NobetListViewModel
    {
        public bool DateEnabled { get; set; }
        public string DateString { get; set; }
        public Guid PersonelId { get; set; }
        public int Period { get; set; }
        public SelectList Personels { get; set; }
        public List<Nobet> Nobetler { get; set; }
    }
}
