namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Departments
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Descriptions { get; set; }
        public int ParentId { get; set; }
        public int Levels { get; set; }
        public bool Status { get; set; }
        public bool StatusUse { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }


    public class DepartmentDDL
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

    }

    public class DepartmentCount
    {
        public int RolesCount { get; set; }
        public int DepChild { get; set; }
    }
    public class DepartmentUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public string Code { get; set; }
    }

    

}
