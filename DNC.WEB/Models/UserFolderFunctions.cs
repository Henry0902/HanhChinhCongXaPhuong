namespace DNC.WEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserFolderFunctions
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int FolderId { get; set; }

        public int UserId { get; set; }

        public string Type { get; set; }
    }


    public class ConfigUserFolder
    {

        public int DepartmentId { get; set; }
        public int FolderId { get; set; }
        public int UserId { get; set; }
        public List<int> Type { get; set; }
    }

    public class FolderRole
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
