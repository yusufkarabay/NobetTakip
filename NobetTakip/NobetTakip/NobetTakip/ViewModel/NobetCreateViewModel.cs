using Microsoft.AspNetCore.Mvc;
using NobetTakip.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{
    public class NobetCreateViewModel
    {
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public int Period { get; set; }
        public List<Personel> Personels { get; set; }

        public Guid NobetId { get; set; }

        [BindProperty]
        public List<string> SelectedPersonels { get; set; }

        public string SelectedPersonelsAsString
        {
            get { return string.Join(",", SelectedPersonels); }
        }

        public NobetCreateViewModel()
        {
            Date = DateTime.Now;
            Period = 0;
        }
    }
}
