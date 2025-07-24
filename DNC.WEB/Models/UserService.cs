namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserService")]
    public partial class UserService
    {
        [Key]
        public int userservice_id { get; set; }
        public string userservice_name { get; set; }
        public string userservice_password { get; set; }
        public string userservice_email { get; set; }
        public int trang_thai { get; set; }
        public int user_created_id { get; set; }
        public DateTime? ngay_tao { get; set; }
    }

    public class UserserviceCount
    {
        public int TotalRecords { get; set; }
    }
}
