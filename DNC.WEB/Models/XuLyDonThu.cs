using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("XuLyDonThu")]
    public partial class XuLyDonThu
    {
        [Key]
        public int Id { get; set; }
        public int IdDonThu { get; set; }
        public int IdTrangThai { get; set; }
        public int IdDonVi { get; set; }
        public int IdNguoiTao { get; set; }
        public string ThaoTac { get; set; }
        public string NoiDung { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? IdDonViTiepNhan { get; set; }
        public int? VuViecId { get; set; }
        public int? IdDonThuGoc { get; set; }
    }

    public partial class XuLyDonThuSearch
    {
        [Key]
        public int Id { get; set; }
        public int IdDonThu { get; set; }
        public int IdTrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public int IdDonVi { get; set; }
        public string TenDonVi { get; set; }
        public int IdNguoiTao { get; set; }
        public string TenNguoiTao { get; set; }
        public string ThaoTac { get; set; }
        public string NoiDung { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? VuViecId { get; set; }
        public int? IdDonViTiepNhan { get; set; }
        public int? IdDonThuGoc { get; set; }
    }

    public partial class XuLyDonThuSearchView
    {
        [Key]
        public int Id { get; set; }
        public int IdDonThu { get; set; }
        public int IdTrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public int IdDonVi { get; set; }
        public string TenDonVi { get; set; }
        public int IdNguoiTao { get; set; }
        public string TenNguoiTao { get; set; }
        public string ThaoTac { get; set; }
        public string NoiDung { get; set; }
        public DateTime? NgayTao { get; set; }
        public virtual List<FileUpload> FileUpload { get; set; }
        public int? VuViecId { get; set; }
        public int? IdDonViTiepNhan { get; set; }
        public int? IdDonThuGoc { get; set; }
    }
}