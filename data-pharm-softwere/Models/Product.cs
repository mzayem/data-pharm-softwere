using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    public enum PackingType
    {
        Tablet,
        Capsule,
        Syrup,
        Injection,
        Cream
    }

    public enum ProductType
    {
        Medicine,
        Neutra,
        NonWare,
        Narcotics,
        Cosmetic,
        Consumer
    }

    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public PackingType PackingType { get; set; }

        [Required]
        public ProductType Type { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "bigint")]
        public long ProductCode { get; set; }

        [Required]
        public int HSCode { get; set; }

        [Required]
        [StringLength(250)]
        public string PackingSize { get; set; }

        [Required]
        public int CartonSize { get; set; }

        [Required]
        [StringLength(100)]
        public String Uom { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal PurchaseDiscount { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal ReqGST { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal UnReqGST { get; set; }

        public bool IsAdvTaxExempted { get; set; }

        public bool IsGSTExempted { get; set; }

        [Required]
        public int SubGroupID { get; set; }

        [ForeignKey("SubGroupID")]
        public virtual SubGroup SubGroup { get; set; }

        [Required]
        public int DivisionID { get; set; }

        [ForeignKey("DivisionID")]
        public virtual Division Division { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<BatchStock> BatchesStock { get; set; }
    }
}