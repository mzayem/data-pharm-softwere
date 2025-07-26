using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("Divisions")]
    public class Division
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DivisionID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Foreign key to Vendor
        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public virtual Vendor Vendor { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Group> Groups { get; set; }
    }
}