namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PageFunctionRoles
    {
        public int Id { get; set; }

        public int PageId { get; set; }

        public int FunctionId { get; set; }

        public int RoleId { get; set; }
    }
    public class PageFunctionRolesAdd
    {

        public int FunctionId { get; set; }
        public int PageId { get; set; }

    }

    public class RolePage
    {
        public int PageId { get; set; }
        public string FunctionId { get; set; }
    }


}
