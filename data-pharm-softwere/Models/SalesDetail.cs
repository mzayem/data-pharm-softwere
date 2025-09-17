using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("SalesDetail")]
    public class SalesDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesDetailId { get; set; }

        [Required]
        public int SalesId { get; set; }

        [ForeignKey("SalesId")]
        public virtual Sales Sales { get; set; }

        [Required]
        public int BatchStockID { get; set; }

        [ForeignKey("BatchStockID")]
        public virtual BatchStock BatchStock { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required]
        public int BonusQty { get; set; } = 0;

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal DiscountAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal GSTAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal NetAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}