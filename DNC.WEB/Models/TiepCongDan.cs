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
    [Table("TiepCongDan")]
    public partial class TiepCongDan
    {
        [Key]
        public int Id { get; set; }
        public int KieuTiepDan { get; set; }
        public DateTime? NgayTiep { get; set; }
        public int IdPhienTCD { get; set; }
        public int IdDoiTuong { get; set; }
        public int SoNguoi { get; set; }
        public string HoTen { get; set; }
        public string SoGiayTo { get; set; }
        public int GioiTinh { get; set; }
        public DateTime? NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string DiaChi { get; set; }
        public int IdPhuongXa { get; set; }
        public int IdQuanHuyen { get; set; }
        public int IdTinhThanh { get; set; }
        public int IdQuocTich { get; set; }
        public int IdDanToc { get; set; }
        public int IdLoaiVu { get; set; }
        public int IdLoaiVuKNTC { get; set; }
        public int IdLoaiVuKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public bool IdUyQuyen { get; set; }
        public string NoiDungTiepDan { get; set; }
        public string KetQuaTiepDan { get; set; }
        public int IdDonVi { get; set; }
        public int IdNguoiTiep { get; set; }
        public bool IdLuotTiep { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? VuViecId { get; set; }
        public int? Pre_Status { get; set; }
    }

    public class TiepCongDanSearch
    {
        [Key]
        public int Id { get; set; }
        public int KieuTiepDan { get; set; }
        public DateTime? NgayTiep { get; set; }
        public int IdPhienTCD { get; set; }
        public string TenPhienTCD { get; set; }
        public int IdDoiTuong { get; set; }
        public int SoNguoi { get; set; }
        public string HoTen { get; set; }
        public string SoGiayTo { get; set; }
        public int GioiTinh { get; set; }
        public DateTime? NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string DiaChi { get; set; }
        public int IdPhuongXa { get; set; }
        public string TenPhuongXa { get; set; }
        public int IdQuanHuyen { get; set; }
        public string TenQuanHuyen { get; set; }
        public int IdTinhThanh { get; set; }
        public string TenTinhThanh { get; set; }
        public int IdQuocTich { get; set; }
        public int IdDanToc { get; set; }
        public int IdLoaiVu { get; set; }
        public int IdLoaiVuKNTC { get; set; }
        public int IdLoaiVuKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public bool IdUyQuyen { get; set; }
        public string NoiDungTiepDan { get; set; }
        public string KetQuaTiepDan { get; set; }
        public string TenDoiTuong { get; set; }
        public string TenNguoiTiep { get; set; }
        public int IdDonVi { get; set; }
        public int IdNguoiTiep { get; set; }
        public DateTime? NgayTao { get; set; }
        public bool IdLuotTiep { get; set; }
        public int TotalRecords { get; set; }
        public int? VuViecId { get; set; }
        public int? Pre_Status { get; set; }
    }

    public partial class TiepCongDanInfo
    {
        [Key]
        public int Id { get; set; }
        public int KieuTiepDan { get; set; }
        public DateTime? NgayTiep { get; set; }
        public int IdPhienTCD { get; set; }
        public int IdDoiTuong { get; set; }
        public int SoNguoi { get; set; }
        public string HoTen { get; set; }
        public string SoGiayTo { get; set; }
        public int GioiTinh { get; set; }
        public DateTime? NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string DiaChi { get; set; }
        public int IdPhuongXa { get; set; }
        public int IdQuanHuyen { get; set; }
        public int IdTinhThanh { get; set; }
        public int IdQuocTich { get; set; }
        public int IdDanToc { get; set; }
        public int IdLoaiVu { get; set; }
        public int IdLoaiVuKNTC { get; set; }
        public int IdLoaiVuKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public bool IdUyQuyen { get; set; }
        public string NoiDungTiepDan { get; set; }
        public string KetQuaTiepDan { get; set; }
        public int IdDonVi { get; set; }
        public int IdNguoiTiep { get; set; }
        public bool IdLuotTiep { get; set; }
        public DateTime? NgayTao { get; set; }
        public string TenDoiTuong { get; set; }
        public string TenTinhThanh { get; set; }
        public string TenQuanHuyen { get; set; }
        public string TenPhuongXa { get; set; }
        public string TenQuocTich { get; set; }
        public string TenDanToc { get; set; }
        public string TenLoaiDonThu { get; set; }
        public string TenLoaiKNTC { get; set; }
        public string TenLoaiKNTCChiTiet { get; set; }

        public int? IdDonThu { get; set; }        
        public string NoiDungDonThu { get; set; }
        public string TenTrangThai { get; set; }
        public string TenHuongXuLy { get; set; }

        public int? VuViecId { get; set; }
        public int? Pre_Status { get; set; }
    }

    public partial class TiepCongDanInfoView
    {
        [Key]
        public int Id { get; set; }
        public int KieuTiepDan { get; set; }
        public DateTime? NgayTiep { get; set; }
        public int IdPhienTCD { get; set; }
        public int IdDoiTuong { get; set; }
        public int SoNguoi { get; set; }
        public string HoTen { get; set; }
        public string SoGiayTo { get; set; }
        public int GioiTinh { get; set; }
        public DateTime? NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string DiaChi { get; set; }
        public int IdPhuongXa { get; set; }
        public int IdQuanHuyen { get; set; }
        public int IdTinhThanh { get; set; }
        public int IdQuocTich { get; set; }
        public int IdDanToc { get; set; }
        public int IdLoaiVu { get; set; }
        public int IdLoaiVuKNTC { get; set; }
        public int IdLoaiVuKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public bool IdUyQuyen { get; set; }
        public string NoiDungTiepDan { get; set; }
        public string KetQuaTiepDan { get; set; }
        public int IdDonVi { get; set; }
        public int IdNguoiTiep { get; set; }
        public DateTime? NgayTao { get; set; }
        public string TenPhienTCD { get; set; }
        public string TenDoiTuong { get; set; }
        public string TenTinhThanh { get; set; }
        public string TenQuanHuyen { get; set; }
        public string TenPhuongXa { get; set; }
        public string TenQuocTich { get; set; }
        public string TenDanToc { get; set; }       
        public string TenLoaiDonThu { get; set; }        
        public string TenLoaiKNTC { get; set; }
        public string TenLoaiKNTCChiTiet { get; set; }

        public int? IdDonThu { get; set; }  
        public string NoiDungDonThu { get; set; }
        public string TenTrangThai { get; set; }
        public string TenHuongXuLy { get; set; }
        public virtual List<FileUpload> FileUpload { get; set; }
        public int? VuViecId { get; set; }
        public int? Pre_Status { get; set; }
    }
}