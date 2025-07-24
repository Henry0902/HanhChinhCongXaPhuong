using System.Web.Mvc;
using System.Web.Routing;

namespace DNC.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("DMSService/DocumentService.asmx/{*pathInfo}");

            routes.MapRoute(
                name: "Quên mật khẩu",
                url: "tai-khoan/quen-mat-khau",
                defaults: new { controller = "Users", action = "ForgotPassword" }
            );

            routes.MapRoute(
                name: "Danh sách tài khoản công dân",
                url: "tai-khoan-cong-dan/danh-sach",
                defaults: new { controller = "TaiKhoanCongDan", action = "CitizenList" }
            );

            routes.MapRoute(
                name: "Chi tiết tài khoản công dân",
                url: "tai-khoan-cong-dan/chi-tiet/{id}",
                defaults: new { controller = "TaiKhoanCongDan", action = "CitizenDetail", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Công dân đăng ký",
                url: "cong-dan-dang-ky/danh-sach",
                defaults: new { controller = "DangKyTiepDan", action = "RegisterListAdmin" }
            );

            routes.MapRoute(
                name: "Chi tiết công dân đăng ký",
                url: "cong-dan-dang-ky/chi-tiet/{id}",
                defaults: new { controller = "DangKyTiepDan", action = "RegisterDetailAdmin", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Danh sách đăng ký",
                url: "dang-ky-tiep-dan/danh-sach",
                defaults: new { controller = "DangKyTiepDan", action = "RegisterList" }
            );

            routes.MapRoute(
                name: "Tạo đăng ký tiếp dân",
                url: "dang-ky-tiep-dan/them-moi",
                defaults: new { controller = "DangKyTiepDan", action = "RegisterAdd" }
            );

            routes.MapRoute(
                name: "Chỉnh sửa đăng ký tiếp dân",
                url: "dang-ky-tiep-dan/chinh-sua/{id}",
                defaults: new { controller = "DangKyTiepDan", action = "RegisterEdit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Chi tiết đăng ký tiếp dân",
                url: "dang-ky-tiep-dan/chi-tiet/{id}",
                defaults: new { controller = "DangKyTiepDan", action = "RegisterDetail", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Chỉnh sửa thông tin cá nhân",
                url: "thong-tin-ca-nhan/chinh-sua",
                defaults: new { controller = "Users", action = "EditProfile" }
            );

            routes.MapRoute(
                name: "Thông tin cá nhân",
                url: "thong-tin-ca-nhan/chi-tiet",
                defaults: new { controller = "Users", action = "Profile" }
            );

            routes.MapRoute(
                name: "Đăng ký tài khoản",
                url: "tai-khoan-cong-dan/dang-ky",
                defaults: new { controller = "Users", action = "RegisterAccount" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}