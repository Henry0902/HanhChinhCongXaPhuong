using DNC.WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DNC.WEB.Repository
{
    public class DepartmentUsersRp
    {

        private DbConnectContext context;
        public DepartmentUsersRp()
        {
            context = new DbConnectContext();
        }

        public List<DepartmentUsers> GetByUserId(int id)
        {
            List<DepartmentUsers> data = context.Database.SqlQuery<DepartmentUsers>("DepartmentUser_GetByUserID @id",
                  new SqlParameter("@id", id)
                  ).ToList();
            return data;
        }

        public void Delete(int ID)
        {

            context.Database.ExecuteSqlCommand("DepartmentUser_Delete @id",
                new SqlParameter("@id", ID)
                );
        }
    }
}