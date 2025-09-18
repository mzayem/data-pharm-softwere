using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using data_pharm_softwere.Models;

namespace data_pharm_softwere.Models
{
    [Table("Sales")]
    public class Sales
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesId { get; set; }

        [Required]
        [StringLength(20)]
        public string VoucherNumber { get; set; }

        [Required]
        public VoucherType VoucherType { get; set; }

        public bool Posted { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime SalesDate { get; set; }

        [Required]
        public TaxBaseType AdvTaxOn { get; set; }

        [Required]
        public TaxBaseType GSTType { get; set; }

        [Required]
        public BillType BillType { get; set; }

        [Required]
        [StringLength(200)]
        public string Remarks { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal AdvTaxRate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal AdvTaxAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal AdditionalCharges { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal DiscountedAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal GrossAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal NetAmount { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int SalesmanBookerTownId { get; set; }

        [Required]
        public int SalesmanSupplierTownId { get; set; }

        [Required]
        public int SalesmanDriverTownId { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string Updatedby { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("SalesmanBookerTownId")]
        public virtual SalesmanTown SalesmanBooker { get; set; }

        [ForeignKey("SalesmanSupplierTownId")]
        public virtual SalesmanTown SalesmanSupplier { get; set; }

        [ForeignKey("SalesmanDriverTownId")]
        public virtual SalesmanTown SalesmanDriver { get; set; }

        public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
    }
}