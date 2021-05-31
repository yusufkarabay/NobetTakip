using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NobetTakip.Core.Models { 

    [Table("Degisim")]
    public class Degisim
    {
        [Column("DegisimId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DegisimId { get; set; }

        [ForeignKey("IsletmeId")]
        public Guid IsletmeId { get; set; }

        [ForeignKey("FirstNobetId")]
        public Guid FirstNobetId { get; set; }
        public Nobet FirstNobet { get; set; }

        [ForeignKey("SecondNobetId")]
        public Guid SecondNobetId { get; set; }
        public Nobet SecondNobet { get; set; }

        [ForeignKey("FirstPersonelId")]
        public Guid FirstPersonelId { get; set; }
        public Personel FirstPersonel { get; set; }

        [ForeignKey("SecondPersonelId")]
        public Guid SecondPersonelId { get; set; }
        public Personel SecondPersonel { get; set; }

        public DateTime RequestDate { get; set; }

        public bool IsAccepted { get; set; }
    }
}
