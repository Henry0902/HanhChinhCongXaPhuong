using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//using VTLT_DNC.Common;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class DangKyTiepDanRp
    {
        private GenericRepository<Users> _context = null;
        private DbConnectContext _obj;
        public DangKyTiepDanRp()
        {
            _obj = new DbConnectContext();
        }

        public List<DangKyTiepDanModel> SearchAll(int pageIndex, int pageSize, int status, string content, int department, DateTime? date, int? IdCongDan)
        {
            try
            {
                return _obj.Database.SqlQuery<DangKyTiepDanModel>("DangKyTiepDan_Search @IdCongDan, @Content, @NgayDangKy, @Status, @Department, @PageIndex, @PageSize",
                        new SqlParameter("@IdCongDan", (object)IdCongDan ?? DBNull.Value),
                        new SqlParameter("@Content", (object)content ?? DBNull.Value),
                        new SqlParameter("@NgayDangKy", (object)date ?? DBNull.Value),
                        new SqlParameter("@Status", status),
                        new SqlParameter("@Department", department),
                        new SqlParameter("@PageIndex", pageIndex),
                        new SqlParameter("@PageSize", pageSize)
                        ).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string GeneratedCode(string departmentId, DateTime dateTime)
        {
            try
            {
                using (var context = new DbConnectContext()) // Thay bằng DbContext thực tế
                {
                    var departmentIdParam = new SqlParameter("@DepartmentId", departmentId);
                    var ngayTaoParam = new SqlParameter("@NgayTao", dateTime);

                    var generatedCodeParam = new SqlParameter("@GeneratedCode", SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };

                    context.Database.ExecuteSqlCommand(
                        "EXEC Generate_MaDangKy @DepartmentId, @NgayTao, @GeneratedCode OUTPUT",
                        departmentIdParam,
                        ngayTaoParam,
                        generatedCodeParam
                    );

                    string generatedCode = generatedCodeParam.Value != DBNull.Value ? generatedCodeParam.Value.ToString() : null;

                    Console.WriteLine("Mã đăng ký được tạo: " + (generatedCode ?? "Không có dữ liệu"));

                    return generatedCode;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DangKyTiepDanModel GetByIdWithFiles(int id)
        {
            try
            {
                var entity = _obj.Database.SqlQuery<DangKyTiepDanModel>(
                                    "EXEC GetDangKyTiepDanById @Id",
                                    new SqlParameter("@Id", id)
                                ).FirstOrDefault();

                if (entity != null)
                {
                    entity.ListFile = _obj.DangKyTiepDanFile.Where(x => x.IdDangKy == entity.Id).ToList();
                }

                return entity;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}