using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Controllers;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class FunctionRp
    {
        private GenericRepository<Functions> _context = null;
        private DbConnectContext _obj;
        private Ultils logs = new Ultils();
        public FunctionRp()
        {
            _obj = new DbConnectContext();
        }

        public List<PageFunctions> GetByPageId(int PageId)
        {
            try
            {
                var data = _obj.Database.SqlQuery<PageFunctions>("PagesFunction_GetByPageId @PageId", new SqlParameter("@PageId", PageId)).ToList();
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public bool DeleteByPageId(int PageId)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("PagesFunction_DeleteByPageId @PageId", new SqlParameter("@PageId", PageId));

                    trans.Commit();
                    return true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }
        public int update(int PageId, string FunctionId)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("PagesFunction_Update @PageId,@FunctionId"
                        , new SqlParameter("@PageId", PageId)
                        , new SqlParameter("@FunctionId", FunctionId)
                        );
                    trans.Commit();
                    return PageId;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return -1;
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }
        //public LinhVucSearch GetById(int id)
        //{
        //    var data = _obj.Database.SqlQuery<LinhVucSearch>("LinhVuc_GetById @Id", new SqlParameter("@Id", id)).FirstOrDefault();
        //    return data;
        //}
        //public List<LinhVucSearch> SearchAll(string keyword, string description, int status, string order, int pageSize, int pageIndex)
        //{
        //    var data = _obj.Database.SqlQuery<LinhVucSearch>("LinhVuc_Search @Keyword, @Description, @Status, @Order, @PageSize, @PageIndex",
        //            new SqlParameter("@Keyword", keyword),
        //            new SqlParameter("@Description", description),
        //            new SqlParameter("@Order", order),
        //            new SqlParameter("@Status", status),
        //            new SqlParameter("@PageSize", pageSize),
        //            new SqlParameter("@PageIndex", pageIndex)
        //            ).ToList();
        //    return data;
        //}
         
        

        //public int GetCountChiTieu(int id)
        //{
        //    var data = _obj.Database.SqlQuery<int>("LinhVuc_GetCountChiTieu @Id",
        //         new SqlParameter("@Id", id)).FirstOrDefault();
        //    return data;
        //}

        //public List<LinhVuc> GetByStatus(int Status)
        //{
        //    var data = _obj.Database.SqlQuery<LinhVuc>("LinhVuc_GetByStatus @Status", new SqlParameter("@Status", Status)).ToList();
        //    return data;
        //}
        //public List<LinhVuc> GetByBoTieuChi(int botieuchiid)
        //{
        //    var data = _obj.Database.SqlQuery<LinhVuc>("LinhVuc_GetAllByBoTieuChi @BoTieuChiId", new SqlParameter("@BoTieuChiId", botieuchiid)).ToList();
        //    return data;
        //}
    }
}