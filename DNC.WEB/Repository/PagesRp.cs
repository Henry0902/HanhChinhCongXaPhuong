using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DNC.WEB.Models;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace DNC.WEB.Repository
{
    public class PagesRp
    {
     
        private DbConnectContext _context;

        public PagesRp()
        {
            _context = new DbConnectContext();
        }
        public List<PagesViewRole> GetPages()
        {
            var data = _context.Database.SqlQuery<PagesViewRole>("Pages_GetAll").ToList();
            return data;
        }
        public List<PagesView> GetAllPage(int Uid)
        {
            var data = _context.Database.SqlQuery<PagesView>("PageFuctionRole_GetByRoleId @UserID", new SqlParameter("@UserID",Uid)).ToList();
            return data;
        }
        public List<PageRole> GetPagesByRoleId(string userId, string roleId)
        {
            List<PageRole> data = _context.Database.SqlQuery<PageRole>("PagesFuctionRole_GetByRoleId @UserId,@RoleId",
                new SqlParameter("@UserId", userId),
                new SqlParameter("@RoleId", roleId)
                ).ToList();
            return data;
        }
        public List<PagesView> GetAll()
        {
            var data = _context.Database.SqlQuery<PagesView>("Pages_GetAll").ToList();
            return data;
        }
        public int CountByParentId(int id)
        {
            return  _context.Database.SqlQuery<int>("Pages_CountChild @id",
                new SqlParameter("@id", id)
                ).FirstOrDefault();
        }

        public int GetLinkIdbyLink(string path)
        {
            return _context.Database.SqlQuery<int>("Pages_getIdbyPath @Path",
                new SqlParameter("@Path",path)).FirstOrDefault();
        }

        public ThongBaoCount ThongBaoCount(string IdDonVi)
        {
            if (IdDonVi != null)
            {
                var data = _context.Database.SqlQuery<ThongBaoCount>("ThongBao_Count @IdDonVi", new SqlParameter("@IdDonVi", IdDonVi)).First();
                return data;
            }else
            {
                return null;
            }
        }
    }

}