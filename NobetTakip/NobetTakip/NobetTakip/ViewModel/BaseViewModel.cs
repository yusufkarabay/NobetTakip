using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.ViewModel
{
    public class BaseViewModel
    {
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
