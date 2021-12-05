using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCNX.Api.Models
{
    [Table("tblUser")]
    public partial class TblUser
    {
        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
    }
}
