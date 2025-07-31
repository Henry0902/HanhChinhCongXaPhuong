using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//using VTLT_DNC.Common;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class UsersRp
    {
        private GenericRepository<Users> _context = null;
        private DbConnectContext _obj;
        public UsersRp()
        {
            _obj = new DbConnectContext();
        }


        public Users Create(Users info)
        {
            Users data = null;
            data = _context.Insert(info);
            _context.Save();
            return data;
        }
        public List<UsersGridView> SearchAll(int pageIndex, int pageSize, string keyword, int islocked, int isdeleted, int roleID)
        {
            try
            {
                var data = _obj.Database.SqlQuery<UsersGridView>("Users_Search @Keyword,@IsLocked,@IsDelete,@RoleID,@PageIndex,@PageSize",
                        new SqlParameter("@Keyword", keyword),
                        new SqlParameter("@IsLocked", islocked),
                        new SqlParameter("@IsDelete", isdeleted),
                        new SqlParameter("@RoleID", roleID),
                        new SqlParameter("@PageIndex", pageIndex),
                        new SqlParameter("@PageSize", pageSize)
                        ).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // lấy user theo role id
        public List<UsersGridView> GetUsersByRole(int roleId)
        {
            var data = _obj.Database.SqlQuery<UsersGridView>("Users_GetByRole @RoleId",
                    new SqlParameter("@RoleId", roleId)
                    ).ToList();
            return data;
        }

        public List<UsersGridView> GetUserById(int IdNguoiTao)
        {
            var data = _obj.Database.SqlQuery<UsersGridView>("Users_GetByRole @RoleId",
                    new SqlParameter("@RoleId", IdNguoiTao)
                    ).ToList();
            return data;
        }

        //public Users GetById(int id)
        //{
        //    try
        //    {
        //        return _obj.Users.FirstOrDefault(u => u.Id == id);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public List<int> CheckExisted(string phoneNumber, string email, string idCard)
        {
            var listResponse = new List<int>();
            if (_obj.Users.Count(x => x.Mobile == phoneNumber) > 0) listResponse.Add(1);
            if (_obj.Users.Count(x => x.Email.ToLower() == email.ToLower()) > 0) listResponse.Add(2);
            if (_obj.Users.Count(x => x.IdCard.ToLower() == idCard.ToLower()) > 0) listResponse.Add(3);
            return listResponse;
        }

        /// <summary>
        ///Update thông tin và quyền của người dùng sử dụng với form quản trị người dùng
        /// </summary>
        public bool updateUserinfoAdmin(Users data, string value, int depID)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("Users_Update @Id,@Position,@Gender,@Moblie,@Email,@UpdatedDate",
                        new SqlParameter("@Id", data.Id),
                        new SqlParameter("@Position", data.Position ?? (object)DBNull.Value),
                        new SqlParameter("@Gender", data.Gender),
                        new SqlParameter("@Moblie", data.Mobile ?? (object)DBNull.Value),
                        new SqlParameter("@Email", data.Email ?? (object)DBNull.Value),
                          new SqlParameter("@UpdatedDate", data.UpdatedDate)
                        );

                    _obj.Database.ExecuteSqlCommand("UsersRoles_DeletebyUserId @Id,@DepID",
                        new SqlParameter("@Id", data.Id),
                        new SqlParameter("@DepID", depID));
                    if (value != "")
                    {
                        _obj.Database.ExecuteSqlCommand("UsersRole_Insert @values",
                            new SqlParameter("@values", value));
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
        public bool userUpdateRole(int uId, List<int> DepGroup, string NewDepGroup, string DisplayName)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    string content = "Hệ thống cập nhật người dùng sang các Group " + NewDepGroup;

                    //List<int> DepGroup = _obj.Database.SqlQuery<int>("Department_getIdDepartment @Values",
                    //    new SqlParameter("@Values", departmentCode)).ToList();

                    List<string> values = new List<string>();

                    foreach (int Obj in DepGroup)
                    {
                        string value = "(" + Obj + "," + uId + ")";
                        values.Add(value);
                    }
                    _obj.Database.ExecuteSqlCommand("Users_UpdateContent @Content,@Id,@UpdateDate",
                        new SqlParameter("@Content", content),
                        new SqlParameter("@Id", uId),
                        new SqlParameter("@UpdateDate", DateTime.Now));
                    // Xóa các phòng ban đang DepartmentUser
                    _obj.Database.ExecuteSqlCommand("UserRole_DeletebyUserId @Uid",
                        new SqlParameter("@Uid", uId));
                    _obj.Database.ExecuteSqlCommand("Department_DeletebyUserId @Id", new SqlParameter("@Id", uId));
                    // Cập nhật thông tin người dùng
                    _obj.Database.ExecuteSqlCommand("Users_UpdateDepartment @values",
                        new SqlParameter("@values", String.Join(",", values)));
                    //Xóa quyền Download trong bảng UserFolderFunction
                    _obj.Database.ExecuteSqlCommand("UserFolderFunction_DeletebyUserId @Id", new SqlParameter("@Id", uId));
                    //Insert lại Quyền Folder 
                    string functions = "1,2";
                    for (int i = 0; i < DepGroup.Count; i++)
                    {
                        _obj.Database.ExecuteSqlCommand("UsersFolderFunction_ReInsertRole @dpID,@userID,@Function",
                       new SqlParameter("@dpID", DepGroup[i]),
                       new SqlParameter("@userID", uId),
                       new SqlParameter("@Function", functions));
                    }


                    _obj.Database.ExecuteSqlCommand("Users_UpdateDisplayName @Id,@Displayname",
                        new SqlParameter("@Id", uId),
                        new SqlParameter("@Displayname", DisplayName));


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

        public List<UsersSuggest> getSuggestUsers(string keyword)
        {
            try
            {
                List<UsersSuggest> data = _obj.Database.SqlQuery<UsersSuggest>("Users_getForSuggest @Keyword",
                   new SqlParameter("@Keyword", keyword)).ToList();
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool updateStatus(int id, bool status)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("Users_UpdateStatus @Id,@Status",
                        new SqlParameter("@Id", id),
                        new SqlParameter("@Status", status));
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
        public List<int> getDepartmentUser(int Uid)
        {
            try
            {
                List<int> data = _obj.Database.SqlQuery<int>("DepartmentUser_getByUserId @Id",
                    new SqlParameter("@Id", Uid)).ToList();
                return data;
            }
            catch (Exception)
            {
                return null;
            }

        }
         
        // Update Thông tin người dùng
        public bool UpdateUsersInfo(Users data)
        {
            using (var trans = _obj.Database.BeginTransaction())
            {
                try
                {
                    _obj.Database.ExecuteSqlCommand("Users_UpdateInfo @Id,@Position,@Email,@Mobile ,@Gender,@UpdateDate",
                             new SqlParameter("@Id", data.Id),
                             new SqlParameter("@Position", data.Position),
                             new SqlParameter("@Email", data.Email),
                             new SqlParameter("@Mobile", data.Mobile),
                             new SqlParameter("@Gender", data.Gender),
                             new SqlParameter("@UpdateDate", DateTime.Now));

                    trans.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
 
    }
}