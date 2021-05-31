using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.Core.Models
{
    [Table("Nobet")]
    public class Nobet
    {
        [Column("NobetId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid NobetId { get; set; }
        [ForeignKey("IsletmeId")]
        public Guid IsletmeId { get; set; }
        public Isletme Isletme { get; set; }
        public String PersonelIds { get; set; }
        public DateTime Date { get; set; }
        public int Period { get; set; } // 08:00 - 08:00 --- 08:00 - 20:00 ve 20:00 - 08:00, 08:00 - 16:00 ve 16:00 -- 00:00 ve 00:00 - 08:00

        [NotMapped]
        public List<string> PersonelIdsArray { 
            get { return PersonelIds?.Split(',').ToList(); }
        }
        [NotMapped]
        public virtual List<Personel> Personels { get; set; }
        [NotMapped]
        public bool IsEnYakin { get; set; }
        [NotMapped]
        public string ShareText {
            get {
                return string.Format("Merhaba, {0} tarihinde {1} nöbetindeyim.{2}{2}Nöbetimle ilgili bilgilere aşağıdaki linkten ulaşabilirsin. https://81.214.1.208:5001/nobet/{3}", Date.ToString("dd MMMM dddd"), DayNightAsString, Environment.NewLine, NobetId.ToString());
            }
        }
        [NotMapped]
        public string DayNightAsString { 
            
            /*get {
                if (DayNight == 0)
                    return "GÜNDÜZ";
                else if (DayNight == 1)
                    return "Gece";
                else
                    return "Tüm Gün";
            }*/

            get
            {
                if (Period == 0) return "TÜM GÜN";
                else if (Period == 1 || Period == 4) return "GÜNDÜZ";
                else return "GECE";
            }
        }
        [NotMapped]
        public string PeriodAsString
        {
            /*get  {
                if (Type == 0 && Period == 0)
                    return "08:00 - 08:00";
                else if (Type == 1 && Period == 0)
                    return "08:00 - 20:00";
                else if (Type == 1 && Period == 1)
                    return "20:00 - 08:00";
                else if (Type == 2 && Period == 0)
                    return "00:00 - 08:00";
                else if (Type == 2 && Period == 1)
                    return "08:00 - 16:00";
                else //(Type == 2 && Period == 2)
                    return "16:00 - 00:00";
            }*/

            get
            {
                if (Period == 0) return "08:00 - 08:00 (Ertesi Gün)";
                else if (Period == 1) return "08:00 - 20:00";
                else if (Period == 2) return "20:00 - 08:00 (Ertesi Gün)";
                else if (Period == 3) return "00:00 - 08:00";
                else if (Period == 4) return "08:00 - 16:00";
                else if (Period == 5) return "16:00 - 24:00";
                else return "Tanımsız";
            }

            /*
             *  0 = 00:00 - 24:00
             *  1 = 08:00 - 20:00
             *  2 = 20:00 - 08:00
             *  3 = 00:00 - 08:00
             *  4 = 08:00 - 16:00
             *  5 = 16:00 - 24:00
             */
        }
        
        public Nobet() { }
    }
}
