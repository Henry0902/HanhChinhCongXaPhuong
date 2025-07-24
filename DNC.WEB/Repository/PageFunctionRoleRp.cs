using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DNC.WEB.Models;
using System.Data.SqlClient;

namespace DNC.WEB.Repository
{
    public class PageFunctionRoleRp
    {

        private DbConnectContext context;
        public PageFunctionRoleRp()
        {
            context = new DbConnectContext();
        }
        

        public void Delete(int ID)
        {
            context = new DbConnectContext();
            context.Database.ExecuteSqlCommand("PagesFunctionRole_DeleteByPageID @id",
                new SqlParameter("@id",ID)
                );
        }
        public int countByRole(int ID)
        {
            context = new DbConnectContext();
        var data = context.Database.SqlQuery<int>("Pages_CountbyRole @roleID",
            new SqlParameter("@roleID", ID)).FirstOrDefault();
        return data;
        }

        public List<PageRole> setRoleforUsers(string uid)
        {
            context = new DbConnectContext();
            try
            {
                List<PagesView> data = context.Database.SqlQuery<PagesView>("PageFuctionRole_GetByRoleId @UserID",
                    new SqlParameter("@UserID",uid)
                    ).ToList();
                List<PageRole> result = new List<PageRole>();
                
                foreach (PagesView p in data)
                {
                    PageRole role = new PageRole();
                    role.PageId = p.PageId;
                    role.FunctionId = p.FunctionId;
                    result.Add(role);
                }
                return result;
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }

}