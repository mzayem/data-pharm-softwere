using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    public class ProductBonus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductBonusID { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int MinQty { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int BonusItems { get; set; }

        public bool IsActive { get; set; } = true;
        public virtual Product Product { get; set; }
        public DateTime AssignedOn { get; set; } = DateTime.Now;
    }
}