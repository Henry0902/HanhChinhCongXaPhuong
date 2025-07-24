namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FileUpload")]
    public partial class FileUpload
    {
        [Key]
        public int Id { get; set; }
        public int objects_id { get; set; }
        public string file_name { get; set; }
        public string file_extension { get; set; }
        public string file_url { get; set; }
        public string file_original { get; set; }
        public string folder_path { get; set; }
        public int trang_thai { get; set; }
        public int file_type { get; set; }
        public int user_created_id { get; set; }
        public DateTime? ngay_tao { get; set; }
        public int IdDonThuGoc { get; set; }
    }
}