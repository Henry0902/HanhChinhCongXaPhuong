using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNC.WEB.Models
{
    [Table("UsersQuenMatKhau")]
    public class UsersQuenMatKhau
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdCongDan { get; set; }

        [StringLength(45)]
        public string IpAddress { get; set; }

        [Required]
        public string Otp { get; set; }

        [Required]
        public DateTime OtpThoiGianTao { get; set; }

        [Required]
        public DateTime OtpHsd { get; set; }

        public int OtpTrangThai { get; set; }

        public int OtpLanNhap { get; set; }

        public int OtpLanGui { get; set; }

        public DateTime OtpGuiLanDau { get; set; }
    }
}
