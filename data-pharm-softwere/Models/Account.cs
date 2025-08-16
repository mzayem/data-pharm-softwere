using data_pharm_softwere.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere
{
    [Table("Accounts")]
    public partial class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountType { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountDetail { get; set; }

        [Required]
        [StringLength(100)]
        public string Status { get; set; }

        [Required]
        [StringLength(100)]
        public string salary_s { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Vendor Vendor { get; set; }
    }
}