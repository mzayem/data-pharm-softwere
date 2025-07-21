using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace data_pharm_softwere.Models
{
    [Table("Salesmen")]
    public class Salesman
    {
        public int SalesmanID { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Contact { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<SalesmanTown> SalesmanTowns { get; set; } = new List<SalesmanTown>();
    }
}