using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("PurchaseDetail")]
    public class PurchaseDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseDetailId { get; set; }

        [Required]
        public int PurchaseId { get; set; }

        [ForeignKey("PurchaseId")]
        public virtual Purchase Purchase { get; set; }

        [Required]
        public int BatchStockID { get; set; }

        [ForeignKey("BatchStockID")]
        public virtual BatchStock BatchStock { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}