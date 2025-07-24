using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNC.WEB.Models
{
    [Table("UsersHistory")]
    public partial class UsersHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdCongDan { get; set; }

        [Required]
        public int IdNguoiDuyet { get; set; }

        [StringLength(255)]
        public string GhiChu { get; set; }

        [Required]
        public DateTime NgayDuyet { get; set; }
    }
}
