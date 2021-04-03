using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.Models
{
    public class Nobet
    {
        public Guid NobetId { get; set; }
        public List<Personel> Nobetciler { get; set; }
        public int Type { get; set; } // 0 = tam gün 24 saat, 1 = günde iki nöbet
        public DateTime Date { get; set; }
        public int Period { get; set; } // 08:00 - 08:00 --- 08:00 - 20:00 ve 20:00 - 08:00, 08:00 - 16:00 ve 16:00 -- 00:00 ve 00:00 - 08:00


        public bool IsEnYakin { get; set; }
        public bool DayNight { get; set; } // 0 = Day, 1 = Night
        
        public string ShareText { get
            {
                string _sz = string.Format("Merhaba, {0} tarihinde {1} nöbetideyim.", Date.ToString("dd MMMM dddd"), DayNight ? "gece" : "gündüz");
                return _sz;
            }
        }
        
        public Nobet()
        {
            IsEnYakin = false;
            DayNight = false;
            Period = 0;
        }
    }
}
