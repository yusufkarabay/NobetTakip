using System;
using System.Collections.Generic;
using System.Text;

namespace NobetTakip.Core.DTO
{
    public class NobetSearchModel
    {
        public bool DateEnabled { get; set; }
        public string DateString { get; set; }
        public Guid PersonelId { get; set; }
        public int Period { get; set; }
        public Guid IsletmeId { get; set; }
    }
}
