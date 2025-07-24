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
    [Table("DonThu")]
    public partial class DonThu
    {
        [Key]
        public int Id { get; set; }
        public int? IdTiepCongDan { get; set; }
        public int IdDonThuXuLy { get; set; }
        public int IdNguonDon { get; set; }
        public string SoVanBan { get; set; }
        public DateTime? NgayNhap { get; set; }
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
        public int IdLoaiDonThu { get; set; }
        public int IdLoaiKNTC { get; set; }
        public int IdLoaiKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public string NoiDungDonThu { get; set; }        
        public int IdDonViNhap { get; set; }
        public int IdNguoiNhap { get; set; }
        public int? IdHuongXuLy { get; set; }
        public int? IdDonViXuLy { get; set; }
        public int? IdDonViXacMinh { get; set; }
        public int? IdDonViTiepNhan { get; set; }
        public int IdTrangThai { get; set; }
        public int IsDelete { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? VuViecId { get; set; }
        public DateTime? NgayThoiHanThuLy { get; set; }
        public int? IdDonThuGoc { get; set; }
        public DateTime? Approve_at { get; set; }
        public int? Pre_Status { get; set; }
    }

    public class DonThuSearch
    {
        [Key]
        public int Id { get; set; }
        public int? IdTiepCongDan { get; set; }
        public int IdDonThuXuLy { get; set; }
        public int IdNguonDon { get; set; }

        public string TenNguonDon { get; set; }
        public string SoVanBan { get; set; }
        public DateTime? NgayNhap { get; set; }
        public int IdDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
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
        public int IdLoaiDonThu { get; set; }
        public int IdLoaiKNTC { get; set; }
        public int IdLoaiKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public string NoiDungDonThu { get; set; }              
        public int IdDonViNhap { get; set; }
        public int IdNguoiNhap { get; set; }
        public string TenNguoiNhap { get; set; }
        public int? IdHuongXuLy { get; set; }
        public string TenHuongXuLy { get; set; }
        public int? IdDonViXuLy { get; set; }
        public int? IdDonViXacMinh { get; set; }
        public string TenDonViXacMinh { get; set; }
        public int? IdDonViTiepNhan { get; set; }
        public string TenDonViTiepNhan { get; set; }
        public DateTime? NgayThoiHanThuLy { get; set; }
        public int IdTrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public int TinhTrangHanXuLy { get; set; }
        public int NgayConXuLy { get; set; }
        public DateTime? NgayTao { get; set; }
        public int TotalRecords { get; set; }
        public int? VuViecId { get; set; }
        public int? IdDonThuGoc { get; set; }
        public DateTime? Approve_at { get; set; }
        public int? Pre_Status { get; set; }
        public int IsChuyenDon { get; set; }
    }

    public class DonThuSearchHuy
    {
        [Key]
        public int Id { get; set; }
        public int? IdTiepCongDan { get; set; }
        public int IdDonThuXuLy { get; set; }
        public int IdNguonDon { get; set; }

        public string TenNguonDon { get; set; }
        public string SoVanBan { get; set; }
        public DateTime? NgayNhap { get; set; }
        public int IdDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
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
        public int IdLoaiDonThu { get; set; }
        public int IdLoaiKNTC { get; set; }
        public int IdLoaiKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public string NoiDungDonThu { get; set; }
        public int IdDonViNhap { get; set; }
        public int IdNguoiNhap { get; set; }
        public string TenNguoiNhap { get; set; }
        public int? IdHuongXuLy { get; set; }
        public string TenHuongXuLy { get; set; }
        public int? IdDonViXuLy { get; set; }
        public int? IdDonViXacMinh { get; set; }
        public string TenDonViXacMinh { get; set; }
        public int? IdDonViTiepNhan { get; set; }
        public string TenDonViTiepNhan { get; set; }
        public DateTime? NgayThoiHanThuLy { get; set; }
        public int IdTrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public int TinhTrangHanXuLy { get; set; }
        public int NgayConXuLy { get; set; }
        public int IsDelete { get; set; }
        public DateTime? NgayTao { get; set; }
        public int TotalRecords { get; set; }
        public int? VuViecId { get; set; }
        public int? IdDonThuGoc { get; set; }
        public DateTime? Approve_at { get; set; }
        public int? Pre_Status { get; set; }
    }

    public class DonThuInfo
    {
        [Key]
        public int Id { get; set; }
        public int? IdTiepCongDan { get; set; }
        public int IdDonThuXuLy { get; set; }
        public int IdNguonDon { get; set; }

        public string TenNguonDon { get; set; }
        public string SoVanBan { get; set; }
        public DateTime? NgayNhap { get; set; }
        public int IdDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
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
        public string TenQuocTich { get; set; }
        public int IdDanToc { get; set; }
        public string TenDanToc { get; set; }
        public int IdLoaiDonThu { get; set; }
        public string TenLoaiDonThu { get; set; }
        public int IdLoaiKNTC { get; set; }
        public string TenLoaiKNTC { get; set; }
        public int IdLoaiKNTCChiTiet { get; set; }
        public string TenLoaiKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public string NoiDungDonThu { get; set; }         
        public int IdDonVi { get; set; }
        public int IdNguoiNhap { get; set; }
        public string TenNguoiNhap { get; set; }
        public int? IdHuongXuLy { get; set; }
        public string TenHuongXuLy { get; set; }
        public int? IdDonViXuLy { get; set; }
        public int? IdDonViXacMinh { get; set; }
        public string TenDonViXacMinh { get; set; }
        public int? IdDonViTiepNhan { get; set; }
        public string TenDonViTiepNhan { get; set; }
        public DateTime? NgayThoiHanThuLy { get; set; }
        public int IdTrangThai { get; set; }
        public string TenTrangThai { get; set; }       
        public DateTime? NgayTao { get; set; }
        public int? VuViecId { get; set; }
        public int? IdDonThuGoc { get; set; }
        public DateTime? Approve_at { get; set; }
        public int? Pre_Status { get; set; }
    }

    public class DonThuInfoView
    {
        [Key]
        public int Id { get; set; }
        public int? IdTiepCongDan { get; set; }
        public int IdDonThuXuLy { get; set; }
        public int IdNguonDon { get; set; }

        public string TenNguonDon { get; set; }
        public string SoVanBan { get; set; }
        public DateTime? NgayNhap { get; set; }
        public int IdDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
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
        public string TenQuocTich { get; set; }
        public int IdDanToc { get; set; }
        public string TenDanToc { get; set; }
        public int IdLoaiDonThu { get; set; }
        public string TenLoaiDonThu { get; set; }
        public int IdLoaiKNTC { get; set; }
        public string TenLoaiKNTC { get; set; }
        public int IdLoaiKNTCChiTiet { get; set; }
        public string TenLoaiKNTCChiTiet { get; set; }
        //public int LanGiaiQuyet { get; set; }
        public int LanTiepNhan { get; set; }
        public string NoiDungDonThu { get; set; }
        public int IdDonVi { get; set; }
        public int IdNguoiNhap { get; set; }
        public string TenNguoiNhap { get; set; }
        public int? IdHuongXuLy { get; set; }
        public string TenHuongXuLy { get; set; }
        public int? IdDonViXuLy { get; set; }
        public int? IdDonViXacMinh { get; set; }
        public string TenDonViXacMinh { get; set; }
        public int? IdDonViTiepNhan { get; set; }
        public string TenDonViTiepNhan { get; set; }
        public DateTime? NgayThoiHanThuLy { get; set; }
        public int IdTrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public virtual List<FileUpload> FileUpload { get; set; }
        public int? VuViecId { get; set; }
        public int? IdDonThuGoc { get; set; }
        public DateTime? Approve_at { get; set; }
        public int? Pre_Status { get; set; }
    }

    public class DonThuInfoPrint
    {
        [Key]
        public int Id { get; set; }
        public string NgayNhap { get; set; }
        public string SoVanBan { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }        
        public string TenLoaiDonThu { get; set; }
        public string NoiDungDonThu { get; set; }
    }

    public class ThongBaoCount
    {
        public int DonThu { get; set; }
        public int DonThuXuLy { get; set; }
        public int DonThuDuyetXuLy { get; set; }
        public int DonThuThuLy { get; set; }
        public int DonThuKetLuan { get; set; }
        public int DonThuDuyetKetLuan { get; set; }
        public int DonThuTraKetQua { get; set; }
    }
}