using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DNC.WEB.Models;
using System.Data.SqlClient;

namespace DNC.WEB.Repository
{
    public class DepartmentsRp
    {
        private DbConnectContext _context;
        public DepartmentsRp()
        {
            _context = new DbConnectContext();
        }
        public List<Departments> GetDepartments()
        {
            var data = _context.Database.SqlQuery<Departments>("Departments_GetAll").ToList();
            return data;
        }
         
        public int checkDepartmentCode(string code)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var data = _context.Database.SqlQuery<int>("Department_CheckCode @code",
                    new SqlParameter("@code", code)).FirstOrDefault();
                    trans.Commit();
                    return data;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    return -1;
                }
            }
        }
        
        public List<Departments> getDepartmentsByUserID(int UserID)
        {
            try
            {
                List<Departments> data = _context.Database.SqlQuery<Departments>("Department_GetByUserID @UserID",
                new SqlParameter("@UserID", UserID)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return new List<Departments>();
            }
        }
        public List<Departments> getDepartmentUsersExits(int uid)
        {
            try
            {
                List<Departments> data = _context.Database.SqlQuery<Departments>("Department_UsersExits @uid",
                new SqlParameter("@uid", uid)).ToList();

                return data;
            }
            catch (Exception e)
            {
                return new List<Departments>();
            }
        }
        public int checkDepartmentName(string name)
        {
            try
            {
                var data = _context.Database.SqlQuery<int>("Departments_CheckDepName @name",
                 new SqlParameter("@name", name)).FirstOrDefault();
                return data;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Thêm mới phòng ban và Setting thư mục và nhóm quyền cho phòng ban
        /// </summary>
        public int createDepartment(Departments data, List<DepartmentRoles> lstRole)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var Id = _context.Database.SqlQuery<decimal>("Departments_Insert @Code,@Name,@Description,@ParentId,@Status,@StatusUse,@Levels,@CreatedBy,@CreatedDate"
                        , new SqlParameter("@Code", data.Code)
                        , new SqlParameter("@Name", data.Name)
                        , new SqlParameter("@Description", data.Descriptions ?? (object)DBNull.Value)
                        , new SqlParameter("@ParentId", data.ParentId)
                        , new SqlParameter("@Status", data.Status)
                        , new SqlParameter("@StatusUse", data.StatusUse)
                        , new SqlParameter("@Levels", data.Levels)
                        , new SqlParameter("@CreatedBy", data.CreatedBy)
                        , new SqlParameter("@CreatedDate", data.CreatedDate)).FirstOrDefault();
                     
                    List<string> listvaluerole = new List<string>(); 
                    string valueRole = String.Empty;
                    if (lstRole != null)
                    {
                        foreach (DepartmentRoles Obj in lstRole)
                        {
                            string value = "(" + (int)Id + "," + Obj.RoleId + "," + "'" + Obj.IsDefault + "'" + ")";
                            listvaluerole.Add(value);
                        }
                        valueRole = String.Join(",", listvaluerole);
                        _context.Database.ExecuteSqlCommand("DepartmentRoles_Insert @values",
                             new SqlParameter("@values", valueRole));
                    }

                    trans.Commit();
                    return (int)Id;
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

        ///<summary>
        ///Cập nhật phòng ban và dữ liệu bảng trung gian
        ///</summary>
        ///
        public int updateDepartment(Departments data,List<DepartmentRoles> lstRole)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("Department_Update @Id,@Code,@Name,@Description,@ParentId,@Status,@StatusUse,@Levels,@UpdateBy,@UpdateDate"
                        , new SqlParameter("@Id", data.Id)
                        , new SqlParameter("@Code", data.Code)
                        , new SqlParameter("@Name", data.Name)
                        , new SqlParameter("@Description", data.Descriptions ?? (object)DBNull.Value)
                        , new SqlParameter("@ParentId", data.ParentId)
                        , new SqlParameter("@Status", data.Status)
                        , new SqlParameter("@StatusUse", data.StatusUse)
                        , new SqlParameter("@Levels", data.Levels)
                        , new SqlParameter("@UpdateBy", data.UpdatedBy)
                        , new SqlParameter("@UpdateDate", data.UpdatedDate));
                    if (data.Status == false)
                    {
                        _context.Database.ExecuteSqlCommand("Department_UpdateChirlStatus @Pid",
                            new SqlParameter("@Pid", data.Id));
                    }

                    string valueRole = String.Empty;
                    _context.Database.ExecuteSqlCommand("DepartmentRoles_Delete @Id",
                       new SqlParameter("@Id", data.Id));
                    List<string> listvaluerole = new List<string>();
                    if (lstRole != null)
                    {
                        foreach (DepartmentRoles Obj in lstRole)
                        {
                            string value = "(" + data.Id + "," + Obj.RoleId + "," + "'" + Obj.IsDefault.ToString() + "'" + ")";
                            listvaluerole.Add(value);
                        }
                        valueRole = String.Join(",", listvaluerole);
                        _context.Database.ExecuteSqlCommand("DepartmentRoles_Insert @values",
                             new SqlParameter("@values", valueRole));
                    }

                    trans.Commit();
                    return data.Id;
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
        public bool settingRoleDepartment(string values, int dId)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("DepartmentRoles_Delete @Id",
                        new SqlParameter("@Id", dId));
                    _context.Database.ExecuteSqlCommand("DepartmentRoles_Insert @values",
                        new SqlParameter("@values", values));
                    trans.Commit();
                    return true;
                }
                catch (Exception)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
        public bool department_Delete(int id)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Database.ExecuteSqlCommand("DepartmentRoles_Delete @Id",
                        new SqlParameter("@Id", id));

                    _context.Database.ExecuteSqlCommand("Derpartments_Delete @Id",
                        new SqlParameter("@Id", id));
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

        public DepartmentCount Dep_CountContent(int id)
        {
            try
            {
                DepartmentCount data = _context.Database.SqlQuery<DepartmentCount>("Department_Countcontent @Id",
                    new SqlParameter("@Id", id)).FirstOrDefault();
                return data;
            }
            catch
            {
                return null;
            }
        }
        ///<summary>
        ///Lấy về các phòng ban mà User đang không trực thuộc
        /// </summary>
        /// Department_getbyUsernotinDepartment
        public List<DepartmentUser> getDepartmentUsernotin(int id)
        {
            try
            {
                List<DepartmentUser> data = _context.Database.SqlQuery<DepartmentUser>("Department_getbyUsernotinDepartment @Uid",
                    new SqlParameter("@Uid", id)).ToList();
                return data;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public List<DepartmentUser> GetDepartmentUserInfo(int id)
        {
            try
            {
                List<DepartmentUser> data = _context.Database.SqlQuery<DepartmentUser>("Department_getUsersInfo @Uid",
                    new SqlParameter("@Uid", id)).ToList();
                return data;

            }
            catch (Exception)
            {
                return null;
            }
        }
        // Check Department has child 
        public List<int> CheckDepHaschirl(int Uid, int DepID)
        {
            List<int> data = new List<int>();
            try
            {
                data = _context.Database.SqlQuery<int>("CheckLeafDepartment_byUserId @uId,@DepId",
                    new SqlParameter("@uId", Uid),
                    new SqlParameter("@DepId", DepID)).ToList();
                return data;
            }
            catch (Exception)
            {
                data.Add(-1);
                return data;
            }
        }
        // Count Chirl
        public int CountChirl(int dID)
        {
            try
            {
                var data = _context.Database.SqlQuery<int>("Department_CheckHasChirl @depID", new SqlParameter("@depID", dID)).FirstOrDefault();
                return data;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        // get Department in Users Folder Function
        public List<DepartmentDDL> getDepartmentinFolderFunction(int id)
        {
            try
            {
                var data = _context.Database.SqlQuery<DepartmentDDL>("Department_GetbyUserFolderFunction @Uid",
                    new SqlParameter("@Uid", id)).ToList();
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        } 
        public List<DepartmentDDL> GetListEdit(int id)
        {
            var data = _context.Database.SqlQuery<DepartmentDDL>("Departments_GetEditById @Id", new SqlParameter("@Id", id)).ToList();
            return data;
        }
        public List<DepartmentDDL> getAllDepartmentLevel()
        {
            var data = _context.Database.SqlQuery<DepartmentDDL>("Department_GetAllDropDown").ToList();
            return data;
        }
        public List<DepartmentDDL> GetAllLevelByParentId(int parentId)
        {
            var data = _context.Database.SqlQuery<DepartmentDDL>("Department_GetAllLevelBy_ParentId @ParentId", new SqlParameter("@ParentId", parentId)).ToList();
            return data;
        }

        //dungnc - lấy danh sách đơn vị sắp xếp theo cấp đơn vị cha, con
        public List<Departments> GetAllOrderByLevel()
        {
            var data = _context.Database.SqlQuery<Departments>("Departments_GetAll_OrderByLevel").ToList();
            return data;
        }

        public List<DepartmentDDL> getAllDropDown_ByParentId(int parentId)
        {
            var data = _context.Database.SqlQuery<DepartmentDDL>("Department_GetAllDropDown_ByParentId @ParentId", new SqlParameter("@ParentId", parentId)).ToList();
            return data;
        }

        //lấy danh sách đơn vị có level 1 
        public List<DepartmentDDL> getDropDownLevel1_ByParentId(int parentId)
        {
            var data = _context.Database.SqlQuery<DepartmentDDL>("Department_GetDepartmentLevel1By_ParentId @ParentId", new SqlParameter("@ParentId", parentId)).ToList();
            return data;
        }

        public List<DepartmentDDL> getAllDropDown_ByParentId_NoSpecialCharacter(int parentId)
        {
            var data = _context.Database.SqlQuery<DepartmentDDL>("Department_GetAllDropDown_ByParentId_NoSpecialCharacter @ParentId", new SqlParameter("@ParentId", parentId)).ToList();
            return data;
        }
    }
}