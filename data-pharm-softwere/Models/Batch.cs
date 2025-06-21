using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("Batches")]
    public class Batch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BatchID { get; set; }

        [Required]
        public int BatchNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime MFGDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal DP { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TP { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MRP { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CartonQty { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CartonPrice { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}