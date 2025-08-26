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

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}