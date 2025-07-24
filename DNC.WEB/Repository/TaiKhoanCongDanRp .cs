using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Razor.Tokenizer.Symbols;

//using VTLT_DNC.Common;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class TaiKhoanCongDanRp
    {
        private GenericRepository<Users> _context = null;
        private DbConnectContext _obj;
        public TaiKhoanCongDanRp()
        {
            _obj = new DbConnectContext();
        }

        public List<UserModel> SearchAll(string keyword, DateTime? createdDate, string idCard, int gender, int status, string address, int pageNumber, int pageSize)
        {
            try
            {
                var data = _obj.Database.SqlQuery<UserModel>(
                        "TaiKhoanCongDan_Search @Keyword, @CreatedDate, @IdCard, @Gender, @Status, @Address, @PageNumber, @PageSize",
                        new SqlParameter("@Keyword", (object)keyword ?? DBNull.Value),
                        new SqlParameter("@CreatedDate", (object)createdDate ?? DBNull.Value),
                        new SqlParameter("@IdCard", (object)idCard ?? DBNull.Value),
                        new SqlParameter("@Gender", (object)gender ?? DBNull.Value),
                        new SqlParameter("@Status", (object)status ?? DBNull.Value),
                        new SqlParameter("@Address", (object)address ?? DBNull.Value),
                        new SqlParameter("@PageNumber", pageNumber),
                        new SqlParameter("@PageSize", pageSize)
                    ).ToList();

                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}