using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DNC.WEB.Models
{
    public class DbConnectContext : DbContext
    {
        public virtual DbSet<VuViec> VuViec { get; set; }
        public virtual DbSet<DmQuocTich> DmQuocTich { get; set; }
        public virtual DbSet<DmDanToc> DmDanToc { get; set; }
        public virtual DbSet<DmDiaDiemTiepDan> DmDiaDiemTiepDan { get; set; }
        public virtual DbSet<DmChucVu> DmChucVu { get; set; }
        public virtual DbSet<DmLoaiDoiTuong> DmLoaiDoiTuong { get; set; }
        public virtual DbSet<DmTinhThanh> DmTinhThanh { get; set; }
        public virtual DbSet<DmQuanHuyen> DmQuanHuyen { get; set; }
        public virtual DbSet<DmPhuongXa> DmPhuongXa { get; set; }
        public virtual DbSet<DmLoaiDonThu> DmLoaiDonThu { get; set; }
        public virtual DbSet<DmLoaiKNTC> DmLoaiKNTC { get; set; }
        public virtual DbSet<DmLoaiKNTCCT> DmLoaiKNTCCT { get; set; }
        public virtual DbSet<DmNguonDon> DmNguonDon { get; set; }
        public virtual DbSet<DmHuongXuLy> DmHuongXuLy { get; set; }
        public virtual DbSet<DmTrangThai> DmTrangThai { get; set; }
        public virtual DbSet<DmPhienTiepCongDan> DmPhienTiepCongDan { get; set; }
        public virtual DbSet<DmCoQuan> DmCoQuan { get; set; }
        public virtual DbSet<DmLoaiKetQua> DmLoaiKetQua { get; set; }
        public virtual DbSet<UserService> UserService { get; set; }
        public virtual DbSet<FileUpload> FileUpload { get; set; }
        public virtual DbSet<TiepCongDan> TiepCongDan { get; set; }
        public virtual DbSet<DonThu> DonThu { get; set; }
        public virtual DbSet<XuLyDonThu> XuLyDonThu { get; set; }
        public virtual DbSet<KetLuanGiaiQuyet> KetLuanGiaiQuyet { get; set; }
        public virtual DbSet<HoSo> HoSo { get; set; }
        public virtual DbSet<DuAn> DuAn { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<DepartmentUsers> DepartmentUsers { get; set; }
        public virtual DbSet<PageFunctionRoles> PageFunctionRoles { get; set; }
        public virtual DbSet<PageFunctions> PageFunctions { get; set; }
        public virtual DbSet<Pages> Pages { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        //public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<SystemConfigs> SystemConfig { get; set; }
        public virtual DbSet<SystemLogs> SystemLogs { get; set; }
        public virtual DbSet<UserFolderFunctions> UserFolderFunctions { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersRoles> UsersRoles { get; set; }
        public virtual DbSet<Functions> Functions { get; set; }
        public virtual DbSet<DepartmentRoles> DepartmentRoles { get; set; }

        public virtual DbSet<UsersTemp> UsersTemps { get; set; }
        public virtual DbSet<DangKyTiepDan> DangKyTiepDan { get; set; }
        public virtual DbSet<DangKyTiepDanFile> DangKyTiepDanFile { get; set; }
        public virtual DbSet<UsersHistory> UsersHistories { get; set; }
        public virtual DbSet<UsersQuenMatKhau> UsersQuenMatKhau { get; set; }

        //public virtual DbSet<News> News { get; set; }
        //public virtual DbSet<Files> Files { get; set; }
        //public virtual DbSet<NewsFiles> NewsFiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departments>().ToTable("Departments");
            modelBuilder.Entity<Users>().ToTable("Users");

            base.OnModelCreating(modelBuilder);
        }
    }
}