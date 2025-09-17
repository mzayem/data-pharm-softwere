using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    public enum AssignmentType
    {
        Booker,
        Supplier,
        Driver
    }

    [Table("SalesmanTowns")]
    public class SalesmanTown
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesmanTownID { get; set; }

        [Required]
        public int SalesmanID { get; set; }

        [ForeignKey("SalesmanID")]
        public virtual Salesman Salesman { get; set; }

        [Required]
        public int TownID { get; set; }

        [ForeignKey("TownID")]
        public virtual Town Town { get; set; }

        [Required]
        public AssignmentType AssignmentType { get; set; } = AssignmentType.Booker;

        [Required]
        [Range(0, 100)]
        public decimal Percentage { get; set; } = 0;

        public DateTime AssignedOn { get; set; } = DateTime.Now;

        public virtual ICollection<Sales> SalesAsBooker { get; set; } = new List<Sales>();
        public virtual ICollection<Sales> SalesAsSupplier { get; set; } = new List<Sales>();
        public virtual ICollection<Sales> SalesAsDriver { get; set; } = new List<Sales>();
    }
}