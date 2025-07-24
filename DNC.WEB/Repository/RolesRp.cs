using System;
using System.Data.SqlClient;
using System.Linq;
using DNC.WEB.Models;
using System.Collections.Generic;
using DNC.WEB.Controllers;
using DNC.CM;

namespace DNC.WEB.Repository
{
    public class RolesRp
    {
        private Ultils logs;
        private DbConnectContext _obj;
        public RolesRp()
        {
            _obj = new DbConnectContext();
           logs = new Ultils();
        }

        // Tìm kiếm và lấy dữ liệu
        public List<RolesGrid> SearchAll(int pageIndex, int pageSize, string keyword, int status)
        {
            try
            {
                var data = _obj.Database.SqlQuery<RolesGrid>("Roles_Search @Keyword,@Status,@Pagesize,@Pageindex",
                        new SqlParameter("@Keyword", keyword),
                        new SqlParameter("@Status", status),
                        new SqlParameter("@Pagesize", pageSize),
                        new SqlParameter("@Pageindex", pageIndex)
                        ).ToList();

                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // Đếm số người dùng và chức năng của nhóm quyền
        public RoleCount countUsersPage(int id)
        {
            try
            {
                var result = _obj.Database.SqlQuery<RoleCount>("Roles_CountUsers_Pages @Id",
                    new SqlParameter("@Id", id)).FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        // Lấy thông tin nhóm quyền theo Id
        public RolesGrid getRolebyId(int id)
        {
            try
            {
                var data = _obj.Database.SqlQuery<RolesGrid>("Roles_GetbyId @Id", new SqlParameter("@Id", id)).FirstOrDefault();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        public bool Role_Delete(int id)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {

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

        // Xóa dữ liệu bảng trung gian liên quan đến nhóm quyền
        public bool deleteURandPageRole(int id,int Uid,string RoleName)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("UsersRolesPageRole_DeletebyRoleId @Id", new SqlParameter("@Id", id));
                    _obj.Database.ExecuteSqlCommand("Roles_UpdateIsDelete @Id",
                          new SqlParameter("@Id", id));
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    logs.SystemLog_Insert_Error(Uid, logs.ActionDelete, " nhóm quyền " + RoleName + logs.NotifyError, (int)ActionType.DELETE, ex.Message,
                           ex.InnerException.ToString(), ex.Source, ex.TargetSite.ToString(), "Roles/Delete");
                    return false;
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }
        // Kiểm tra trùng tên nhóm quyền
        public int checkRoleName(string name)
        {
            try
            {
                int count = _obj.Database.SqlQuery<int>("Roles_CheckName @Name",
                new SqlParameter("@Name", name)).FirstOrDefault();
                return count;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        ///<summary>
        ///Lấy về dữ liệu bảng PagesFunctionRole by RoleId
        /// </summary>
        public List<PageFunctionRoles> getPageFunctionbyRole(int id)
        {
            try
            {
                var data = _obj.Database.SqlQuery<PageFunctionRoles>("PagesFunctionRole_GetbyRoleId @Id",
                    new SqlParameter("@Id", id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        ///<summary>
        ///Phân quyền cho nhóm người dùng theo trang 
        /// </summary>
        public bool createPagesFunctionRole(string values, int Roleid)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("PagesFunctionRole_DeletebyRoleId @Id",
                        new SqlParameter("@Id", Roleid));
                    if (values != String.Empty)
                    {
                        _obj.Database.ExecuteSqlCommand("PageFunctionRoles_Insert @values",
                            new SqlParameter("@values", values));
                    }
                    trans.Commit();
                    return true;
                }
                catch (Exception e)
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
        ///<summary>
        ///Lấy về nhóm quyền theo DepartmentId
        /// </summary>
        /// 
        public List<RoleCheckbox> getRolebyDepartment(int id)
        {
            try
            {
                List<RoleCheckbox> data = _obj.Database.SqlQuery<RoleCheckbox>("Roles_getDefalutIdbyDepId @Id",
                    new SqlParameter("@Id", id)).ToList();
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///<summary>
        ///Lấy về tất cả nhóm quyền để setting cho Phòng ban
        /// </summary>
        ///
        ///
        public List<RoleCheckbox> getAllRole()
        {
            try
            {
                List<RoleCheckbox> data = _obj.Database.SqlQuery<RoleCheckbox>("Roles_getforDepartment").ToList();
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }


        ///<summary>
        ///Cập nhật nhóm quyền
        ///</summary>
        public bool RolesUpdate(RoleInfo data)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("Roles_update @Id,@Name,@Status,@UpdatedDate,@UpdatedBy",
                        new SqlParameter("@Id", data.Id),
                        new SqlParameter("@Name", data.Name),
                        new SqlParameter("@Status", data.Status),
                        new SqlParameter("@UpdatedDate", data.UpdatedDate),
                        new SqlParameter("@UpdatedBy", data.UpdatedBy));

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

        public List<RoleCheckbox> GetRoleByUserId(int id)
        {
            try
            {
                var data = _obj.Database.SqlQuery<RoleCheckbox>("Roles_GetByUserId @Id",
                    new SqlParameter("@Id", id)).ToList();
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
         
    }
}