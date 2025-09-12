using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("DiscountPolicies")]
    public class DiscountPolicy
    {
        [Key]
        public int DiscountPolicyID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        public int CustomerAccountId { get; set; }

        [Range(0, 100)]
        public decimal FlatDiscount { get; set; } = 0;

        public DateTime? FlatDiscountExpiry { get; set; }

        [Range(0, 100)]
        public decimal CreditDiscount { get; set; } = 0;

        public DateTime? CreditDiscountExpiry { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("CustomerAccountId")]
        public virtual Customer Customer { get; set; }
    }
}