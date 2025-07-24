namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dmTinhThanh")]
    public partial class DmTinhThanh
    {
        [Key]
        public int id { get; set; }
        public string TenTinhThanh { get; set; }
        public string MaTinhThanh { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    public partial class DmTinhThanhSearch
    {
        [Key]
        public int id { get; set; }
        public string TenTinhThanh { get; set; }
        public string MaTinhThanh { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TotalRecords { get; set; }
    }
    public class Dropdown
    {
        public List<dmDropdown> listTinhThanh;
        public List<dmDropdown> listQuanHuyen;
        public List<dmDropdown> listPhuongXa;
        public List<dmDropdown> listQuanHuyenById;
    }
    public class Delete
    {
        public int Messeger { get; set; }
    }
    public class dmDropdown
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}