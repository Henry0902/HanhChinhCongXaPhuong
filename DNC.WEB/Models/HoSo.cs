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
    [Table("HoSo")]
    public partial class HoSo
    {
        [Key]
        public int hoso_id { get; set; }
        public string hoso_ma { get; set; }
        public string hoso_ten { get; set; }
        public string hoso_sohieu { get; set; }
        public string hoso_number { get; set; }
        public int duan_id { get; set; }
        public string noidung { get; set; }
        public int kho_id { get; set; }
        public int kegia_id { get; set; }
        public int hopcap_id { get; set; }
        public int donvi_id { get; set; }
        public int loaihs_id { get; set; }
        public int ttvl_id { get; set; }
        public int domat_id { get; set; }
        public int mdtc_id { get; set; }
        public int thoihan_id { get; set; }
        public DateTime? ngay_baoquan_from { get; set; }
        public DateTime? ngay_baoquan_to { get; set; }
        public DateTime? ngay_luutru_from { get; set; }
        public DateTime? ngay_luutru_to { get; set; }
        public string buttich { get; set; }
        public string chugiai { get; set; }
        public int hoso_type { get; set; }
        public int trang_thai { get; set; }
        public DateTime? ngay_tao { get; set; }
    }

    public class HoSoSearch
    {
        [Key]
        public int hoso_id { get; set; }
        public string hoso_ma { get; set; }
        public string hoso_ten { get; set; }
        public string hoso_sohieu { get; set; }
        public string hoso_number { get; set; }
        public int duan_id { get; set; }
        public string noidung { get; set; }
        public int kho_id { get; set; }
        public int kegia_id { get; set; }
        public int hopcap_id { get; set; }
        public int donvi_id { get; set; }
        public int loaihs_id { get; set; }
        public int ttvl_id { get; set; }
        public int domat_id { get; set; }
        public int mdtc_id { get; set; }
        public int thoihan_id { get; set; }
        public DateTime? ngay_baoquan_from { get; set; }
        public DateTime? ngay_baoquan_to { get; set; }
        public DateTime? ngay_luutru_from { get; set; }
        public DateTime? ngay_luutru_to { get; set; }
        public string buttich { get; set; }
        public string chugiai { get; set; }
        public int trang_thai { get; set; }
        public DateTime? ngay_tao { get; set; }
        public string loaihs_ten { get; set; }
        public int sotl_vb { get; set; }
        public int TotalRecords { get; set; }
    }
}