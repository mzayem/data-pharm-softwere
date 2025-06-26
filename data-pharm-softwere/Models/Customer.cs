using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace data_pharm_softwere.Models
{
    public enum PartyType
    {
        Doctor,
        Pharmacy,
        RetailerWholesaler,
        Dispenser,
        MedicalStore,
        Distributor,
        Institute,
    }

    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Contact { get; set; }

        [Required]
        [StringLength(50)]
        public string CNIC { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        public int TownID { get; set; }

        [ForeignKey("TownID")]
        public virtual Town Town { get; set; }

        [StringLength(50)]
        public string LicenceNo { get; set; }

        [Required]
        public PartyType PartyType { get; set; }

        [Required]
        [StringLength(20)]
        public string NtnNo { get; set; }

        public bool NorcoticsSaleAllowed { get; set; }
        public bool InActive { get; set; }
        public bool IsAdvTaxExempted { get; set; }
        public bool FbrInActiveGST { get; set; }
        public bool FBRInActiveTax236H { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}