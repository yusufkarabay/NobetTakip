using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NobetTakip.Core.Models
{
    [Table("Isletme")]
    public class Isletme
    {
        [Column("IsletmeId")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid IsletmeId { get; set; }
        public string IsletmeAdi { get; set; }
        
        public string IsletmeKod { get; set; }
        public string MailAddress { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        
    }

}
