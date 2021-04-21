using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.Models
{
    [Table("Personel")]
    public class Personel
    {
        [Column("PersonelId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PersonelId { get; set; }
        public Isletme Isletme { get; set; }
        public string RealName { get; set; }
        public string MailAddress { get; set; }
        public string GSMNo { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get;set; }
    }
}
