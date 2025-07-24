using System;
using System.IO;
using log4net;
using DNC.WEB.Controllers;

namespace DNC.WEB.Controllers
{
    public class Ultils
    {
        #region "Notify - HungPV"
        // Thông báo
        public string NotifySuccess = " thành công";
        public string NotifyError = " thất bại";
        // Thông báo hành động
        public string ActionLogin = "Đăng nhập";
        public string ActionLogout = "Đăng xuất";
        public string ActionCreate = "Tạo mới";
        public string ActionUpdate = "Cập nhật";
        public string ActionDelete = "Xóa dữ liệu";
        public string ActionSetting = "Thiết lập";
        public string ActionImport = "Import dữ liệu";
        public string ActionExport = "Export dữ liệu";
        public string ActionDownload = "Tải về dữ liệu";
        public string ActionLoadData = "Lấy dữ liệu";
        // Loại hành động trên form
        public string FormCreate = "ADD";
        public string FormUpdate = "UPDATE";
        public string FormDelete = "DELETE";
        public string FormData = "DATA";
        public string FromSetting = "SETTING";
        public string FormDownload = "DOWNLOAD";
        public string FormComment = "COMMENT";
        public string FormLogin = "LOGIN";
        public string FormLogout = "LOGOUT";

        #endregion

        #region  "Xử lý cho Users"
        public static string GetNameByUserId(int userId)
        {
            try
            {
                UsersController ctlUser = new UsersController();
                var obj = ctlUser.GetUserInfoById(userId);
                return obj.DisplayName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string GetUserNameByUserId(int userId)
        {
            try
            {
                UsersController ctlUser = new UsersController();
                var obj = ctlUser.GetUserInfoById(userId);
                return obj.UserName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        #endregion

        #region "Ghi logs"
        private readonly ILog Loginfo = LogManager.GetLogger("CentralCMS_INFO");
        public void SystemLog_Insert_Error(int userName, string actionName,string datatype,int type, string message, string innerException, string userAgent, string rawUrl, string method)
        {
            LogException.ActiveLog objActiveLog = new LogException.ActiveLog();
            objActiveLog.InsertData(GetUserNameByUserId(userName), DateTime.Now, actionName + datatype, objActiveLog.GetIpAdress(), objActiveLog.GetMacAddress(), type, message, innerException, userAgent, rawUrl, method, false);
            LogException.ProviderConfig objProviderConfig = new LogException.ProviderConfig();
            objProviderConfig.Config();
            Loginfo.Info(GetNameByUserId(userName) + "(" + GetUserNameByUserId(userName) + ")" + actionName +" >> "+ datatype +" >> "+ userAgent +" >> "+ rawUrl +" >> "+ method);
        }
        public void SystemLog_Insert_Success(int userName, string actionName, string datatype, int type, string method)
        {
            LogException.ActiveLog objActiveLog = new LogException.ActiveLog();
            objActiveLog.InsertData(GetUserNameByUserId(userName), DateTime.Now, actionName + datatype, objActiveLog.GetIpAdress(), objActiveLog.GetMacAddress(), type, "", "", "", "", method, true);
            LogException.ProviderConfig objProviderConfig = new LogException.ProviderConfig();
            objProviderConfig.Config();
            Loginfo.Info(GetNameByUserId(userName) + "(" + GetUserNameByUserId(userName) + ")" + actionName + " >> " + datatype + " >> " + method);
        }

        #endregion
        #region "Action"
        private void FileDelete(string path)
        {
            string[] fileName = Directory.GetFiles(path);
            foreach (string file in fileName)
            {
                if (File.Exists(file) == true)
                {
                    File.Delete(file);
                }
            }
        }
        #endregion

    }
}