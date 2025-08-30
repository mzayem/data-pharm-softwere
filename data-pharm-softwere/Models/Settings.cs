using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("Settings")]
    public class Setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // You can add other settings here
        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(50)]
        public string DefaultCurrency { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string StockInHandAccountNo { get; set; }

        // ✅ Transaction Heads
        [Required, StringLength(50)]
        public string PurchaseHead { get; set; } = "P";

        [Required, StringLength(50)]
        public string PurchaseReturnHead { get; set; } = "PR";

        [Required, StringLength(50)]
        public string TransferInHead { get; set; } = "TI";

        [Required, StringLength(50)]
        public string TransferOutHead { get; set; } = "TO";

        [Required, StringLength(50)]
        public string SalesHead { get; set; } = "S";

        [Required, StringLength(50)]
        public string SalesReturnHead { get; set; } = "SR";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}