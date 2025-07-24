using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using DNC.WEB.Models;

namespace DNC.WEB.LogException
{
    public class ActiveLog
    {
        private DbConnectContext _db = null;
        private SqlCommand _objCommand;
        public ActiveLog()
        {
            _db = new DbConnectContext();
        }
        public void InsertData(string userName, DateTime actionDate, string actionName, string ipAddress, string macAddress, int type, string message, string innerException, string userAgent, string rawUrl, string method, bool status)
        {
            using (_db = new DbConnectContext())
            {
                    _db.Database.Connection.Open();   
                    SqlConnection connection = (SqlConnection)_db.Database.Connection;
                    const string strQuery = "SystemLogs_Insert";
                    _objCommand = new SqlCommand(strQuery, connection);
                    _objCommand.CommandType = CommandType.StoredProcedure;
                    _objCommand.Parameters.AddWithValue("@UserName", userName);
                    _objCommand.Parameters.AddWithValue("@ActionDate", actionDate);
                    _objCommand.Parameters.AddWithValue("@ActionName", actionName);
                    _objCommand.Parameters.AddWithValue("@IPAddress", ipAddress);
                    _objCommand.Parameters.AddWithValue("@MacAddress", macAddress);
                    _objCommand.Parameters.AddWithValue("@Type", type);
                    _objCommand.Parameters.AddWithValue("@Message", message);
                    _objCommand.Parameters.AddWithValue("@InnerException", innerException);
                    _objCommand.Parameters.AddWithValue("@UserAgent", userAgent);
                    _objCommand.Parameters.AddWithValue("@RawURL", rawUrl);
                    _objCommand.Parameters.AddWithValue("@Method", method); 
                    _objCommand.Parameters.AddWithValue("@Status", status); 
                    _objCommand.ExecuteNonQuery();
            }
        }
        //public string GetIpAdress()
        //{
        //    string ipHost = Dns.GetHostName();
        //    string ip = Dns.GetHostByName(ipHost).AddressList[0].ToString();
        //    return ip;
        //}

        public string GetIpAdress()
        {
            //string visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //bool GetLan = false;
            //if (String.IsNullOrEmpty(visitorIPAddress))
            //    visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //if (string.IsNullOrEmpty(visitorIPAddress))
            //    visitorIPAddress = HttpContext.Current.Request.UserHostAddress;

            //if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
            //{
            //    GetLan = true;
            //    visitorIPAddress = string.Empty;
            //}

            //if (GetLan && string.IsNullOrEmpty(visitorIPAddress))
            //{
            //    //This is for Local(LAN) Connected ID Address
            //    string stringHostName = Dns.GetHostName();
            //    //Get Ip Host Entry
            //    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
            //    //Get Ip Address From The Ip Host Entry Address List
            //    IPAddress[] arrIpAddress = ipHostEntries.AddressList;

            //    try
            //    {
            //        visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
            //    }
            //    catch
            //    {
            //        try
            //        {
            //            visitorIPAddress = arrIpAddress[0].ToString();
            //        }
            //        catch
            //        {
            //            try
            //            {
            //                arrIpAddress = Dns.GetHostAddresses(stringHostName);
            //                visitorIPAddress = arrIpAddress[0].ToString();
            //            }
            //            catch
            //            {
            //                visitorIPAddress = "127.0.0.1";
            //            }
            //        }
            //    }

            //}
            //return visitorIPAddress;

            var context = HttpContext.Current;
            string ip = String.Empty;

            if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (!String.IsNullOrWhiteSpace(context.Request.UserHostAddress))
                ip = context.Request.UserHostAddress;

            if (ip == "::1")
            {
                string ipHost = Dns.GetHostName();
                ip = Dns.GetHostByName(ipHost).AddressList[0].ToString();
            } 
            return ip;

            //string ipAdd = "";
            //IPHostEntry host = default(IPHostEntry);
            //string hostName = null;
            //hostName = System.Environment.MachineName;
            //host = Dns.GetHostEntry(hostName);
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        ipAdd = Convert.ToString(ip);
            //    }
            //}
            //return ipAdd;
        }

        public string GetMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }
    }
}