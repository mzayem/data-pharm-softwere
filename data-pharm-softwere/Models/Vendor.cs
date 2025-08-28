using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("Vendors")]
    public class Vendor
    {
        [Key, ForeignKey("Account")]
        public int AccountId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string Contact { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string Town { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string LicenceNo { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [StringLength(20)]
        public string SRACode { get; set; }

        [Required]
        [StringLength(20)]
        public string GstNo { get; set; }

        [Required]
        [StringLength(20)]
        public string NtnNo { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal AdvTaxRate { get; set; } = 0;

        [Required]
        [StringLength(20)]
        public string CompanyCode { get; set; }

        [StringLength(250)]
        public string Remarks { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Division> Divisions { get; set; }
        public virtual Account Account { get; set; }
    }
}