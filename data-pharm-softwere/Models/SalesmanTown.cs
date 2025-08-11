using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace data_pharm_softwere.Models
{
    [Table("SalesmanTowns")]
    public class SalesmanTown
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalesmanTownID { get; set; }

        [Required]
        public int SalesmanID { get; set; }

        public virtual Salesman Salesman { get; set; }

        [Required]
        public int TownID { get; set; }

        public virtual Town Town { get; set; }

        public DateTime AssignedOn { get; set; } = DateTime.Now;
    }
}