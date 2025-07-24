using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Linq;

namespace DNC.WEB.Models
{
    [Table("cdDangKyTiepDan")]
    public partial class DangKyTiepDan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string MaDangKy { get; set; }

        [Required]
        public int IdCongDan { get; set; }

        [Required]
        public int IdDonViTiepDan { get; set; }

        public DateTime? NgayDangKy { get; set; }

        public DateTime? NgayGui { get; set; }

        public string ChuDe { get; set; }

        public DateTime? NgayTiepNhan { get; set; }

        public int? IdNguoiXuLy { get; set; }

        public DateTime? NgayXuLy { get; set; }

        public string KetQuaXuLy { get; set; }

        public DateTime? ThoiGianHen { get; set; }

        [Required]
        public int TrangThai { get; set; }

        public int? NoiTiep { get; set; } 

        public DateTime? NgayHuy { get; set; }

        [StringLength(2000)]
        public string LyDoHuy { get; set; }

        [Required]
        public DateTime NgayTao { get; set; } 

        public DateTime? NgayCapNhat { get; set; }
    }

    public partial class CreateOrUpdateDangKyTiepDanRequest
    {
        [Required]
        [StringLength(30)]
        public string MaDangKy { get; set; }

        [Required]
        public int IdCongDan { get; set; }

        [Required]
        public int IdDonViTiepDan { get; set; }

        public string NgayDangKy { get; set; }

        public string ChuDe { get; set; }

        [Required]
        public int TrangThai { get; set; }

        public string LyDoHuy { get; set; }

        public List<int> FileDelete { get; set; }
    }

    public class DangKyTiepDanModel
    {
        public int Id { get; set; }

        public string MaDangKy { get; set; }

        public int IdCongDan { get; set; }

        public int IdDonViTiepDan { get; set; }

        public string TenDonVi { get; set; }

        public DateTime? NgayDangKy { get; set; }

        public DateTime? NgayGui { get; set; }

        public string ChuDe { get; set; }

        public DateTime? NgayTiepNhan { get; set; }

        public int? IdNguoiXuLy { get; set; }

        public DateTime? NgayXuLy { get; set; }

        public string KetQuaXuLy { get; set; }

        public DateTime? ThoiGianHen { get; set; }

        public int TrangThai { get; set; }

        public string TrangThaiText { get; set; }

        public int? NoiTiep { get; set; }

        public DateTime? NgayHuy { get; set; }

        public string LyDoHuy { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string NguoiXuLy { get; set; }

        public string NoiTiepText { get; set; }

        [Required]
        public DateTime NgayTao { get; set; }

        public int TotalRecords { get; set; }

        public List<DangKyTiepDanFile> ListFile { get; set; }
    }
}
