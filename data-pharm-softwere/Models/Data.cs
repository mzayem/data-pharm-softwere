using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("data")]
    public class Data
    {
        [Key]
        [Column("Sr#")]
        public int Sr { get; set; }

        [Column("Vr#")]
        public int? Vr { get; set; }

        [Column("VrDate", TypeName = "date")]
        public DateTime? VrDate { get; set; }

        [Column("AccountId")]
        public int? AccountId { get; set; }

        [Column("AccountTitle")]
        [StringLength(255)]
        public string AccountTitle { get; set; }

        [Column("Dr", TypeName = "money")]
        public decimal? Dr { get; set; }

        [Column("Cr", TypeName = "money")]
        public decimal? Cr { get; set; }

        [Column("Remarks")]
        [StringLength(255)]
        public string Remarks { get; set; }

        [Column("Type")]
        [StringLength(255)]
        public string Type { get; set; }

        [Column("Status")]
        [StringLength(255)]
        public string Status { get; set; }

        [Column("Creator")]
        [StringLength(255)]
        public string Creator { get; set; }

        [Column("Vtype")]
        [StringLength(100)]
        public string Vtype { get; set; }

        [Column("Updater")]
        [StringLength(255)]
        public string Updater { get; set; }

        [Column("Updetail")]
        [StringLength(255)]
        public string Updetail { get; set; }

        [Column("recamt", TypeName = "money")]
        public decimal? RecAmt { get; set; }
    }
}