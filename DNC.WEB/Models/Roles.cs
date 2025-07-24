namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Roles
    {
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set;}
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
    public class RolesGrid
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public DateTime CreatedDate { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public int CountUserRoles { get; set; }
        public int CountPageRoles { get; set; }
        public int CountTotal { get; set; }
        public bool IsDefault { get; set; }

    }
    public class RoleCount
    {
        public int CountUserRoles { get; set; }
        public int CountPageRoles { get; set; }
    }

    public class RoleCheckbox
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IsDefault { get; set; }
       
    }
    public class RoleInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public bool Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; } 
    }
}
