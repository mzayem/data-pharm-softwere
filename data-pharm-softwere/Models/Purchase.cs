using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    public enum TaxBaseType
    {
        Net,
        Gross
    }

    public enum PurchaseType
    {
        Purchase,
        Return
    }

    [Table("Purchases")]
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseId { get; set; }

        public bool Posted { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public TaxBaseType AdvTaxOn { get; set; }

        [Required]
        public TaxBaseType GSTType { get; set; }

        [StringLength(100)]
        public string PoNumber { get; set; }

        [StringLength(200)]
        public string Reference { get; set; }

        [Required]
        public PurchaseType PurchaseType { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal AdvTaxRate { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal AdvTaxAmount { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal AdditionalCharges { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal DiscountedAmount { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal GrossAmount { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal NetAmount { get; set; }

        [Required]
        public int VendorId { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Updatedby { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("VendorId")]
        public virtual Vendor Vendor { get; set; }

        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; } = new List<PurchaseDetail>();
    }
}