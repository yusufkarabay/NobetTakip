using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NobetTakip.Core.Models
{
    [Table("Bildirim")]
    public class Bildirim
    {
        [Column("BildirimId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BildirimId { get; set; }

        [ForeignKey("PersonelId")]
        public Guid PersonelId { get; set; }
        public Personel Personel;

        [ForeignKey("DegisimId")]
        public Guid DegisimId { get; set; }
        public Degisim Degisim;

        public DateTime Date { get; set; }
        public bool Seen { get; set; }
    }
}
