using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("BatchesStock")]
    public class BatchStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BatchStockID { get; set; }

        [Required]
        [StringLength(50)]
        public string BatchNo { get; set; }

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

        [NotMapped]
        public decimal CartonDp => DP * (Product?.CartonSize ?? 0);

        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableQty { get; set; } = 0;

        [Required]
        [Range(0, int.MaxValue)]
        public int InTransitQty { get; set; } = 0;

        [Required]
        [Range(0, int.MaxValue)]
        public int OnHoldQty { get; set; } = 0;

        [Required]
        [Range(0, int.MaxValue)]
        public int QuarantineQty { get; set; } = 0;

        [NotMapped]
        public int TotalQty => AvailableQty + InTransitQty + OnHoldQty + QuarantineQty;

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public int ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}