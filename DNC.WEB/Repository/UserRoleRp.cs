using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using DNC.WEB.Models;
using System.DirectoryServices;

namespace DNC.WEB.Repository
{
    public class UserRoleRp
    {
        private DbConnectContext _db = null;

        public UserRoleRp()
        {
            _db = new DbConnectContext();
        }
        public Users Login(string userName, string passWord)
        {
            var data = _db.Database.SqlQuery<Users>("Users_Login @usersname,@password",
                new SqlParameter("@usersname", userName),
                new SqlParameter("@password", passWord)).FirstOrDefault();
            return data;

        }
        public Users LoginDefault(string userName)
        {
            var data = _db.Database.SqlQuery<Users>("Users_LoginDefault @usersname",
                new SqlParameter("@usersname", userName)).FirstOrDefault();
            return data;
        }
        public List<UsersRoles> getRolesbyUserID(int id)
        {
            return _db.Database.SqlQuery<UsersRoles>("UsersRole_getbyUserId @Id",
                new SqlParameter("@Id", id)).ToList();
        }
        public void DeletebyUserId(int id)
        {
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.Database.ExecuteSqlCommand("UsersRoles_DeletebyUserId @Id",
                  new SqlParameter("@Id", id)
                  );
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }


        //New UserLogin
        public Users userLogin(string userName, string passWord)
        {
            Users data = _db.Database.SqlQuery<Users>("Users_Login @usersname,@password",
                new SqlParameter("@usersname", userName),
                new SqlParameter("@password", passWord)
                ).FirstOrDefault();
            return data;
        }

        public void updateStatus(int userID, bool status)
        {
            _db.Database.ExecuteSqlCommand("Updatestaus_byUser @UserID,@Status",
                new SqlParameter("@UserID", userID),
                new SqlParameter("@Status", status)
                );
        }
        // Lấy về 
        public List<int> getDepartmentGroupMail(int uid)
        {
            return _db.Database.SqlQuery<int>("DepartmentGroupMail_getbyUserId @Id",
                new SqlParameter("@Id", uid)).ToList();
        }
        //Lấy về các Group mail đang tồn tại trong hệ thống
        public List<int> getDepartmentExits(string goupmail)
        {

            return _db.Database.SqlQuery<int>("Department_getExitsGroupmail @Values",
                new SqlParameter("@Values", goupmail)).ToList();
        }

        public static DirectoryEntry createDirectoryEntry(string username, string password)
        {
            DirectoryEntry ldapConnection = new DirectoryEntry("LDAP://cqtd.vnpt.vn", username, password);
            return ldapConnection;
        }
        public int authen_ldap(string userName, string password)
        {
            int authentic = 0;
            try
            {
                DirectoryEntry entry = createDirectoryEntry(userName, password);
                object nativeObject = entry.NativeObject;
                authentic = 1;
            }
            catch
            {
                authentic = 0;
            }
            return authentic;
        }
    }
}