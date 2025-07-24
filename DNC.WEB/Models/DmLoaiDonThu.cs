namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dmLoaiDonThu")]
    public partial class DmLoaiDonThu
    {
        [Key]
        public int Id { get; set; }
        public string TenLoaiDonThu { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
    }

    public partial class DmLoaiDonThuSearch
    {
        [Key]
        public int id { get; set; }
        public string TenLoaiDonThu { get; set; }
        public int TrangThai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? TotalRecords { get; set; }
    }
    public class Dropdown1
    {
        public List<dmDropdown> listLoaiDonThu;
        public List<dmDropdown> listLoaiKNTC;
        public List<dmDropdown> listLoaiKNTCById;
    }
}