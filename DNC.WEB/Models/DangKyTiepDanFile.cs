using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNC.WEB.Models
{
    [Table("cdDangKyTiepDanFile")]
    public partial class DangKyTiepDanFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdDangKy { get; set; }

        [Required]
        [StringLength(255)]
        public string TenTep { get; set; }

        [Required]
        [StringLength(255)]
        public string Url { get; set; }

        [Required]
        [StringLength(255)]
        public string Path { get; set; }
    }
}
