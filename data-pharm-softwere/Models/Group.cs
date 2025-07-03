using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("Groups")]
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Foreign key to Vendor
        [Required]
        public int DivisionID { get; set; }

        [ForeignKey("DivisionID")]
        public virtual Division Division { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<SubGroup> SubGroups { get; set; }
    }
}