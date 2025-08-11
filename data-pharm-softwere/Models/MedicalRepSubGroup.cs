using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace data_pharm_softwere.Models
{
    [Table("MedicalRepSubGroups")]
    public class MedicalRepSubGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int MedicalRepID { get; set; }

        [Required]
        public int SubGroupID { get; set; }

        public virtual MedicalRep MedicalRep { get; set; }
        public virtual SubGroup SubGroup { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}