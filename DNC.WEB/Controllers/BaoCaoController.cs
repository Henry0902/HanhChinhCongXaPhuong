using System;
using System.Net;
using System.Web.Mvc;
using DNC.WEB.Models;
using DNC.WEB.Repository;
using System.Globalization;
using System.Data;
using System.IO;
using Aspose.Cells;

namespace DNC.WEB.Controllers
{
    public class BaoCaoController : Controller
    {
        private BaoCaoRp _obj = null;
        DbConnectContext db = new DbConnectContext();
        Ultils logs = new Ultils();

        public ActionResult Index()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopTiepCongDan()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopDonThu()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoDonThuTheoDiaBan()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopKetQuaXuLyDon()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopKetQuaXuLyDon01()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopKetQuaXuLyDonKhieuNai()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopKetQuaXuLyDonToCao()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopKetQuaXuLyDonKienNghiPhanAnh()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopKetQuaTiepCongDan()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult BaoCaoTongHopKetQuaTiepCongDan01()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        public ActionResult Charts()
        {
            var session = Session["Users"];
            if (session == null)
            {
                return Redirect("/Account/Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult BaoCao_TiepCongDan_DonThu_ByYear(string Nam)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TiepCongDan_DonThu_ByYear(Nam), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_DonThuTheoLoai(string Nam)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_DonThuTheoLoai(Nam), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_ThongKeDonThuTiepNhan(string Nam)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_ThongKeDonThuTiepNhan(Nam), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_ThongKeDonThuGiaiQuyet(string Nam)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_ThongKeDonThuGiaiQuyet(Nam), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_ThongKeKetQuaTiepDan(string Nam)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_ThongKeKetQuaTiepDan(Nam), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_TongHop_TiepCongDan(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHop_TiepCongDan(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_TongHop_DonThu(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHop_DonThu(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_DonThuTheoDiaBan(string IdTinhThanh, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_DonThuTheoDiaBan(IdTinhThanh, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_TongHopKetQuaXuLyDonThu(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHopKetQuaXuLyDonThu(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_TongHopKetQuaXuLyDonThu01(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHopKetQuaXuLyDonThu01(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_TongHopKetQuaXuLyDonThuKhieuNai(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHopKetQuaXuLyDonThuKhieuNai(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_TongHopKetQuaXuLyDonThuToCao(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHopKetQuaXuLyDonThuToCao(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_TongHopKetQuaTiepCongDan(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHopKetQuaTiepCongDan(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult BaoCao_TongHopKetQuaTiepCongDan01(string IdDonVi, string TuNgay, string DenNgay)
        {
            try
            {
                _obj = new BaoCaoRp();
                return Json(_obj.BaoCao_TongHopKetQuaTiepCongDan01(IdDonVi, TuNgay, DenNgay), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult BC_SoTiepCongDan(string IdDonVi, string KieuTiepDan, string IdPhienTCD, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, 
                                                string IdLoaiVu, string IdLoaiVuKNTC, string IdLoaiVuKNTCChiTiet, string NoiDung, string TuNgay, string DenNgay, string order)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Ngày tiếp ");
                dt.Columns.Add("Họ tên - Địa chỉ - CMND/Hộ chiếu của công dân");
                dt.Columns.Add("Nội dung vụ việc");
                dt.Columns.Add("Phân loại đơn/Số người");
                dt.Columns.Add("Cơ quan đã giải quyết");
                dt.Columns.Add("Thụ lý để giải quyết");
                dt.Columns.Add("Trả lại đơn và hướng dẫn");
                dt.Columns.Add("Chuyển đến cơ quan, tổ chức đơn vị có thẩm quyền");
                dt.Columns.Add("Theo dõi kết quả giải quyết");
                int i = 1;
                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_SoTiepCongDan(IdDonVi, KieuTiepDan, IdPhienTCD, IdDoiTuong, KeyWord, IdTinhThanh, IdQuanHuyen, IdPhuongXa, 
                                                IdLoaiVu, IdLoaiVuKNTC, IdLoaiVuKNTCChiTiet, NoiDung, TuNgay, DenNgay, order);
                int location = 7;
                foreach (BaoCao_SoTiepCongDan item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++, item.NgayTiep.ToString(), item.ThongTin, item.NoiDungTiepDan, item.PhanloaiDon, item.CQDaGiaiQuyet, item.ThuLyGiaiQUyet, item.HuongDan, item.ChuyenDon, item.TenTrangThai);
                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_SOTIEPCONGDAN.xls");
                string filename = "BC_SOTIEPCONGDAN" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                string folderPath = Server.MapPath(foldername);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                DeleteFile(folderPath);
                string savefile = Path.Combine(folderPath, filename);
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                ws.Cells.ImportDataTable(dt, false, 7, 0);

                //ws.Cells["A1"].PutValue(pTitle.ToUpper() + "\r\n" + pUnit.ToUpper());
                //ws.Cells["C1"].PutValue("BÁO CÁO ĐÁNH GIÁ CCHC " + pTitle.ToUpper());
                //ws.Cells["C3"].PutValue("Năm " + nam);

                //ws.Cells["D" + Convert.ToInt16(location + 3)].PutValue("Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);
                //ws.Cells["D" + Convert.ToInt16(location + 7)].PutValue(displayName.ToUpper());

                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult BC_SoTiepNhanDonThu(string IdDonVi, string IdNguonDon, string IdDoiTuong, string KeyWord, string IdTinhThanh, string IdQuanHuyen, string IdPhuongXa, string IdLoaiDonThu, string IdLoaiKNTC, string IdLoaiKNTCChiTiet
                                        , string IdDonThuXuLy, string IdTrangThai, string IdHuongXuLy, string NoiDung, string TuNgay, string DenNgay, string order)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Ngày tiếp nhận đơn ");
                dt.Columns.Add("Nguồn đơn chuyển đến");
                dt.Columns.Add("Họ tên - Địa chỉ - CMND/Hộ chiếu của người KNTC");
                dt.Columns.Add("Nội dung vụ việc");
                dt.Columns.Add("Phân loại đơn/Số người");
                dt.Columns.Add("Cơ quan đã giải quyết");
                dt.Columns.Add("Thụ lý để giải quyết");
                dt.Columns.Add("Trả lại đơn và hướng dẫn");
                dt.Columns.Add("Chuyển đến cơ quan, tổ chức đơn vị có thẩm quyền");
                dt.Columns.Add("Ra văn bản đôn đốc"); 
                dt.Columns.Add("Lưu theo dõi");
                dt.Columns.Add("Theo dõi kết quả giải quyết");
                int i = 1;

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_SoTiepNhanDonThu(IdDonVi, IdNguonDon, IdDoiTuong, KeyWord, IdTinhThanh, IdQuanHuyen, IdPhuongXa, IdLoaiDonThu, IdLoaiKNTC, 
                                                IdLoaiKNTCChiTiet, IdDonThuXuLy, IdTrangThai, IdHuongXuLy, NoiDung, TuNgay, DenNgay, order);
                int location = 7;
                foreach (BaoCao_SoTiepNhanDonThu item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++, item.NgayNhap.ToString(), item.TenNguonDon, item.ThongTin, item.NoiDungDonThu, item.PhanloaiDon, item.CQDaGiaiQuyet, item.ThuLyGiaiQUyet, item.HuongDan, item.ChuyenDon, item.VBDonDoc, item.LuuTheoDoi, item.TenTrangThai);
                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_SOTIEPNHANDONTHU.xls");
                string filename = "BC_SOTIEPNHANDONTHU" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                string folderPath = Server.MapPath(foldername);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                DeleteFile(folderPath);
                string savefile = Path.Combine(folderPath, filename);
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                ws.Cells.ImportDataTable(dt, false, 7, 0);

                //ws.Cells["A1"].PutValue(pTitle.ToUpper() + "\r\n" + pUnit.ToUpper());
                //ws.Cells["C1"].PutValue("BÁO CÁO ĐÁNH GIÁ CCHC " + pTitle.ToUpper());
                //ws.Cells["C3"].PutValue("Năm " + nam);

                //ws.Cells["D" + Convert.ToInt16(location + 3)].PutValue("Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);
                //ws.Cells["D" + Convert.ToInt16(location + 7)].PutValue(displayName.ToUpper());

                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult BC_DonThuTheoDiaBan(string IdTinhThanh, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Đơn vị");
                dt.Columns.Add("Tổng đơn");
                dt.Columns.Add("Vụ việc");
                dt.Columns.Add("Đơn đủ điều kiện");
                dt.Columns.Add("Đơn không đủ điều kiện");
                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_DonThuTheoDiaBan(IdTinhThanh, TuNgay, DenNgay);
                int location = 6;
                foreach (BaoCao_DonThuTheoDiaBan item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++, item.TenQuanHuyen.ToString(), item.TongDT, item.TongVu, item.DTDuDieuKien, item.DTKhongDuDieuKien);
                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_DONTHUTHEODIABAN.xls");
                string filename = "BC_DONTHUTHEODIABAN" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 6, 0);
                ws.Cells.DeleteRows(data.Count + 6, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult BC_TongHopKetQuaXuLyDonThu(string IdDonVi, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Tên đơn vị");
                dt.Columns.Add("Tổng số đơn");
                dt.Columns.Add("Đơn có nhiều người đứng tên");
                dt.Columns.Add("Đơn một người đứng tên");
                dt.Columns.Add("Đơn khác");
                dt.Columns.Add("Đơn có nhiều người đứng tên 1");
                dt.Columns.Add("Đơn một người đứng tên 1");
                dt.Columns.Add("Đơn khác 1");
                dt.Columns.Add("Số đơn đã xử lý");
                dt.Columns.Add("Số đơn");
                dt.Columns.Add("Số vụ việc");
                dt.Columns.Add("Khiếu nại");
                dt.Columns.Add("Tố cáo");
                dt.Columns.Add("Kiến nghị, phản ánh");
                dt.Columns.Add("Tranh chấp");
                dt.Columns.Add("Lần đầu");
                dt.Columns.Add("Nhiều lần");
                dt.Columns.Add("Đang giải quyết");
                dt.Columns.Add("Chưa giải quyết");
                dt.Columns.Add("Tổng số");
                dt.Columns.Add("Khiếu nại 1");
                dt.Columns.Add("Tố cáo 1");
                dt.Columns.Add("Kiến nghị, phản ánh 1");
                dt.Columns.Add("Tranh chấp 1");
                dt.Columns.Add("Tổng số 1");
                dt.Columns.Add("Hướng dẫn");
                dt.Columns.Add("Chuyển đơn");
                dt.Columns.Add("Đôn đốc, giải quyết");
                dt.Columns.Add("Số văn bản phúc đáp nhận được do chuyển đơn");

                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_TongHopKetQuaXuLyDonThu(IdDonVi, TuNgay, DenNgay);
                int location = 8;
                foreach (BaoCao_TongHopKetQuaXuLyDonThu item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++, item.TenDonVi, item.TNTrongKy, 0, 0, 0, item.TNTrongKyDoanDongNguoi, 
                        item.TNTrongKyCaNhan, item.TNTrongKyKhac, item.DonDaXuLy, item.DonDuDieuKien, 
                        item.VuDuDieuKien, item.DonKhieuNai, item.DonToCao, item.DonPhanAnh, item.DonTranhChap,
                        item.GiaiQuyetLanDau, item.GiaiQuyetNhieuLan, 0, item.ChuaGiaiQuyet, item.DonThuocThamQuyen, 
                        item.DonThuocThamQuyenKhieuNai, item.DonThuocThamQuyenToCao, item.DonThuocThamQuyenPhanAnh,
                        item.DonThuocThamQuyenTranhChap, item.DonKhongThuocThamQuyen, item.DonHuongDan, item.DonChuyenDon, 
                        item.DonDonDoc, item.DonChuyenPhucDap);
                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_KETQUAXULYDON.xls");
                string filename = "BC_KETQUAXULYDON" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername); 
                }
                
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 8, 0);
                ws.Cells.DeleteRows(data.Count + 8, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult BC_KetQuaTiepCongDan(string IdDonVi, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Tên đơn vị");
                dt.Columns.Add("Tổng số lượt tiếp");
                dt.Columns.Add("Tổng số người được tiếp");
                dt.Columns.Add("Tổng số vụ việc tiếp");
                dt.Columns.Add("Số lượt tiếp");
                dt.Columns.Add("Số người được tiếp");
                dt.Columns.Add("Tiếp lần đầu");
                dt.Columns.Add("Tiếp nhiều lần");
                dt.Columns.Add("Số đoàn được tiếp");
                dt.Columns.Add("Số người được tiếp 1");
                dt.Columns.Add("Tiếp lần đầu 1");
                dt.Columns.Add("Tiếp nhiều lần 1");
                dt.Columns.Add("Số kỳ tiếp");
                dt.Columns.Add("Số lượt tiếp 1");
                dt.Columns.Add("Số người được tiếp 2");
                dt.Columns.Add("Tiếp lần đầu 2");
                dt.Columns.Add("Tiếp nhiều lần 2");
                dt.Columns.Add("Số đoàn được tiếp 1");
                dt.Columns.Add("Số người được tiếp 3");
                dt.Columns.Add("Tiếp lần đầu 3");
                dt.Columns.Add("Tiếp nhiều lần 3");
                dt.Columns.Add("Số kỳ tiếp 1");
                dt.Columns.Add("Số lượt tiếp 2");
                dt.Columns.Add("Số người được tiếp 4");
                dt.Columns.Add("Tiếp lần đầu 4");
                dt.Columns.Add("Tiếp nhiều lần 4");
                dt.Columns.Add("Số đoàn được tiếp 2");
                dt.Columns.Add("Số người được tiếp 5");
                dt.Columns.Add("Tiếp lần đầu 5");
                dt.Columns.Add("Tiếp nhiều lần 5");

                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_TongHopKetQuaTiepCongDan(IdDonVi, TuNgay, DenNgay);
                int location = 9;
                foreach (BaoCao_TongHopKetQuaTiepCongDan item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++, item.TenDonVi, item.TSLuot, item.TSNguoi,
                        item.TSVuViec, item.TTXSoLuot, item.TTXSoNguoi, item.TTXVuViecLanDau,
                        item.TTXVuViecNhieuLan, item.TTXSoDoan, item.TTXSoNguoiTrongDoan, item.TTXSoDoanLanDau, item.TTXSoDoanNhieuLan,
                        item.TDKSoky, item.TDKSoLuot, item.TDKSoNguoi, item.TDKVuViecLanDau,
                        item.TDKVuViecNhieuLan, item.TDKSoDoan, item.TDKSoNguoiTrongDoan,
                        item.TDKSoDoanLanDau, item.TDKSoDoanNhieuLan, item.TDK_UQSoKy, item.TDK_UQSoLuot,
                        item.TDK_UQSoNguoi, item.TDK_UQVuViecLanDau, item.TDK_UQVuViecNhieuLan, item.TDK_UQSoDoan, item.TDK_UQSoNguoiTrongDoan,
                        item.TDK_UQSoDoanLanDau, item.TDK_UQSoDoanNhieuLan);
                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_KETQUATIEPCONGDAN.xls");
                string filename = "BC_KETQUATIEPCONGDAN" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 9, 0);
                ws.Cells.DeleteRows(data.Count + 9, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult BC_TongHopTiepCongDan(string IdDonVi, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Tên đơn vị");
                dt.Columns.Add("Số lượt 1");
                dt.Columns.Add("Số người 1");
                dt.Columns.Add("Số vụ");
                dt.Columns.Add("Số lượt 2");
                dt.Columns.Add("Số người 2");
                dt.Columns.Add("Số lượt 3");
                dt.Columns.Add("Số người 3");
                dt.Columns.Add("Số lượt 4");
                dt.Columns.Add("Số người 4");
                dt.Columns.Add("Số lượt 5");
                dt.Columns.Add("Số người 5");
                dt.Columns.Add("Số lượt 6");
                dt.Columns.Add("Số người 6");
                dt.Columns.Add("Số lượt 7");
                dt.Columns.Add("Số người 7");
                dt.Columns.Add("Số lượt 8");
                dt.Columns.Add("Số người 8");
                dt.Columns.Add("Cá nhân");
                dt.Columns.Add("Đoàn đông người");
                dt.Columns.Add("Cơ quan, tổ chức");
                dt.Columns.Add("Hành chính 1");
                dt.Columns.Add("Tư pháp 1");
                dt.Columns.Add("Chính sách 1");
                dt.Columns.Add("Đất đai 1");
                dt.Columns.Add("Hành chính 2");
                dt.Columns.Add("Tư pháp 2");
                dt.Columns.Add("Tham nhũng");
                dt.Columns.Add("Đất đai 2");
                dt.Columns.Add("Tranh chấp");
                dt.Columns.Add("Chính sách 2");
                dt.Columns.Add("Đất đai 3");
                dt.Columns.Add("Tư pháp 3");
                dt.Columns.Add("Khác");
                dt.Columns.Add("Chưa được giải quyết");
                dt.Columns.Add("Đã được giải quyết lần đầu");
                dt.Columns.Add("Đã được giải quyết nhiều lần");

                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_TongHop_TiepCongDan(IdDonVi, TuNgay, DenNgay);
                int location = 8;
                foreach (BaoCao_TongHop_TiepCongDan item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++, item.TenDonVi, item.TTiepDan, item.TSoNguoi,
                        item.TSoVu, item.TTTinhUyTD, item.TTTinhUySN, item.TTHdndTD,
                        item.TTHdndSN, item.TTBantcdTD, item.TTBantcdSN, item.HdndTD, item.HdndSN,
                        item.DbqdTD, item.DbqdSN, item.CTUbndTD, item.CTUbndSN,
                        item.BTTinhUyTD, item.BTTinhUySN, item.CaNhan,
                        item.DoanDongNguoi, item.CoQuanToChuc, item.KhieuNaiHanhChinh, item.KhieuNaiTuPhap,
                        item.KhieuNaiChinhSach, item.KhieuNaiDatDai, item.ToCaoHanhChinh, item.ToCaoTuPhap, item.ToCaoThamNhung,
                        item.ToCaoDatDai, item.TranhChap, item.PhanAnhChinhSach, item.PhanAnhDatDai, item.PhanAnhTuPhap,
                        item.PhanAnhKhac, item.ChuaGiaiQuyet, item.GiaiQuyetLanDau, item.GiaiQuyetNhieuLan);
                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_TONGHOPTIEPCONGDAN.xls");
                string filename = "BC_TONGHOPTIEPCONGDAN" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 8, 0);
                ws.Cells.DeleteRows(data.Count + 8, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult BC_TongHopKetQuaXuLyDon01(string IdDonVi, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Tên đơn vị");
                dt.Columns.Add("Tổng số 1");
                dt.Columns.Add("Kỳ trước chuyển sang");
                dt.Columns.Add("Tiếp nhận trong kỳ");
                dt.Columns.Add("Số đơn đã xử lý");
                dt.Columns.Add("Số đơn chưa xử lý (chuyển kỳ sau xử lý)");
                dt.Columns.Add("Số đơn");
                dt.Columns.Add("Số vụ việc");
                dt.Columns.Add("Khiếu nại 1");
                dt.Columns.Add("Tố cáo 1");
                dt.Columns.Add("Kiến nghị, phản ánh 1");
                dt.Columns.Add("Lần đầu");
                dt.Columns.Add("Nhiều lần");
                dt.Columns.Add("Chưa giải quyết xong");
                dt.Columns.Add("Tổng số 2");
                dt.Columns.Add("Khiếu nại 2");
                dt.Columns.Add("Tố cáo 2");
                dt.Columns.Add("Kiến nghị, phản ánh 2");
                dt.Columns.Add("Tổng số 3");
                dt.Columns.Add("Hướng dẫn");
                dt.Columns.Add("Chuyển đơn");
                dt.Columns.Add("Đôn đốc, giải quyết");
                dt.Columns.Add("Số văn bản phúc đáp nhận được do chuyển đơn");
                

                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_TongHopKetQuaXuLyDonThu01(IdDonVi, TuNgay, DenNgay);
                int location = 8;
                foreach (BaoCao_TongHopKetQuaXuLyDonThu01 item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++, item.TenDonVi,
                         item.TNTruocKy + item.TNTrongKy,
                         item.TNTruocKy, item.TNTrongKy, item.DonDaXuLy, item.DonChuaXyLy, 
                         item.DonKhieuNai + item.DonToCao + item.DonPhanAnh,
                         item.VuDuDieuKien, item.DonKhieuNai, item.DonToCao, item.DonPhanAnh,
                         item.GiaiQuyetLanDau, item.GiaiQuyetNhieuLan, item.ChuaGiaiQuyet,
                         item.DonThuocThamQuyenKhieuNai + item.DonThuocThamQuyenToCao + item.DonThuocThamQuyenPhanAnh,
                         item.DonThuocThamQuyenKhieuNai, item.DonThuocThamQuyenToCao, item.DonThuocThamQuyenPhanAnh,
                         item.DonHuongDan + item.DonChuyenDon + item.DonDonDoc,
                         item.DonHuongDan, item.DonChuyenDon, item.DonDonDoc, item.DonChuyenPhucDap);

                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_TONGHOPKETQUAXULYDON01.xls");
                string filename = "BC_TONGHOPKETQUAXULYDON01" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 8, 0);
                ws.Cells.DeleteRows(data.Count + 8, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult BC_TongHopKetQuaXuLyDonKhieuNai(string IdDonVi, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Tên đơn vị");
                dt.Columns.Add("Tổng số 1");
                dt.Columns.Add("Kỳ trước chuyển sang 1");
                dt.Columns.Add("Tiếp nhận trong kỳ 1");
                dt.Columns.Add("Tổng 1");
                dt.Columns.Add("Kỳ trước chuyển sang 2");
                dt.Columns.Add("Tiếp nhận trong kỳ 2");
                dt.Columns.Add("Số đơn");
                dt.Columns.Add("Số vụ việc");
                dt.Columns.Add("Tổng 2");
                dt.Columns.Add("Tố cáo 1");
                dt.Columns.Add("Chế độ chính sách");
                dt.Columns.Add("Đất đai nhà cửa");
                dt.Columns.Add("Khác");
                dt.Columns.Add("Lĩnh vực tư pháp");
                dt.Columns.Add("Lĩnh vực Đảng, đoàn thể");
                dt.Columns.Add("Lĩnh vực khác");
                dt.Columns.Add("Lần đầu 1");
                dt.Columns.Add("Lần 2");
                dt.Columns.Add("Đã có bản án của TAND");
                dt.Columns.Add("Chưa giải quyết xong");
                dt.Columns.Add("Tổng 3");
                dt.Columns.Add("Lần đầu 2");
                dt.Columns.Add("Lần 3");
                dt.Columns.Add("Tổng số 4");
                dt.Columns.Add("Hướng dẫn");
                dt.Columns.Add("Đôn đốc, giải quyết");
                dt.Columns.Add("Số văn bản phúc đáp nhận được do chuyển đơn");


                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_TongHopKetQuaXuLyDonThuKhieuNai(IdDonVi, TuNgay, DenNgay);
                int location = 8;
                foreach (BaoCao_TongHopKetQuaXuLyDonThuKhieuNai item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++,
                          item.TenDonVi,
                          item.TNTruocKy + item.TNTrongKy, item.TNTruocKy, item.TNTrongKy,
                          item.DonDaXuLyKyTruoc + item.DonDaXuLyTrongKy, item.DonDaXuLyKyTruoc, item.DonDaXuLyTrongKy,
                          item.DonDuDieuKien, item.VuDuDieuKien,
                          item.HanhChinhChinhSach + item.HanhChinhDatDai + item.HanhChinhKhac,
                          item.HanhChinhChinhSach, item.HanhChinhDatDai, item.HanhChinhKhac,
                          item.LinhVucTuPhap, item.LinhVucDangDoanThe, item.LinhVucKhac,
                          item.GiaiQuyetLanDau, item.GiaiQuyetLan2, item.BanAnTAND, item.ChuaGiaiQuyet,
                          item.KetQuaXuLyLanDau + item.KetQuaXuLyLan2,
                          item.KetQuaXuLyLanDau, item.KetQuaXuLyLan2,
                          item.DonHuongDan + item.DonDonDoc,
                          item.DonHuongDan, item.DonDonDoc,
                          item.DonChuyenPhucDap
                    );


                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_TongHopKetQuaXuLyDonKhieuNai.xls");
                string filename = "BC_TongHopKetQuaXuLyDonKhieuNai" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 8, 0);
                ws.Cells.DeleteRows(data.Count + 8, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult BC_TongHopKetQuaXuLyDonToCao(string IdDonVi, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Tên đơn vị");
                dt.Columns.Add("Tổng số 1");
                dt.Columns.Add("Kỳ trước chuyển sang 1");
                dt.Columns.Add("Tiếp nhận trong kỳ 1");
                dt.Columns.Add("Tổng 1");
                dt.Columns.Add("Kỳ trước chuyển sang 2");
                dt.Columns.Add("Tiếp nhận trong kỳ 2");
                dt.Columns.Add("Số đơn");
                dt.Columns.Add("Số vụ việc");
                dt.Columns.Add("Tổng cộng");
                dt.Columns.Add("Chế độ chính sách");
                dt.Columns.Add("Đất đai nhà cửa");
                dt.Columns.Add("Công chức công vụ");
                dt.Columns.Add("Khác");
                dt.Columns.Add("Tham nhũng");
                dt.Columns.Add("Lĩnh vực tư pháp");
                dt.Columns.Add("Lĩnh vực Đảng, đoàn thể");
                dt.Columns.Add("Lĩnh vực khác");
                dt.Columns.Add("Quá thời hạn chưa giải quyết");
                dt.Columns.Add("Đã có kết luận giải quyết");
                dt.Columns.Add("Chưa giải quyết xong");
                dt.Columns.Add("Tổng số 2");
                dt.Columns.Add("Tố cáo lần đầu");
                dt.Columns.Add("Tố cáo tiếp");
                dt.Columns.Add("Tổng số 3");
                dt.Columns.Add("Chuyển đơn");
                dt.Columns.Add("Đôn đốc, giải quyết");
                dt.Columns.Add("Số văn bản phúc đáp nhận được do chuyển đơn");


                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_TongHopKetQuaXuLyDonThuToCao(IdDonVi, TuNgay, DenNgay);
                int location = 8;
                foreach (BaoCao_TongHopKetQuaXuLyDonThuToCao item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++,
                          item.TenDonVi,
                          item.TNTruocKy + item.TNTrongKy,
                          item.TNTruocKy, item.TNTrongKy,
                          item.DonDaXuLyKyTruoc + item.DonDaXuLyTrongKy,
                          item.DonDaXuLyKyTruoc, item.DonDaXuLyTrongKy,
                          item.DonDuDieuKien,
                          item.HanhChinhChinhSach + item.HanhChinhDatDai + item.HanhChinhCongChucCongVu + item.HanhChinhKhac + item.ThamNhung + item.LinhVucTuPhap + item.LinhVucDangDoanThe + item.LinhVucKhac,
                          item.HanhChinhChinhSach + item.HanhChinhDatDai + item.HanhChinhCongChucCongVu + item.HanhChinhKhac,
                          item.HanhChinhChinhSach, item.HanhChinhDatDai, item.HanhChinhCongChucCongVu,
                          item.HanhChinhKhac, item.ThamNhung, item.LinhVucTuPhap,
                          item.LinhVucDangDoanThe, item.LinhVucKhac, item.QuaThoiHanChuaGiaiQuyet,
                          item.DaCoKetLuanGiaiQuyet, item.ChuaGiaiQuyet,
                          item.ToCaoLanDau + item.ToCaoTiep,
                          item.ToCaoLanDau, item.ToCaoTiep,
                          item.DonChuyenDon + item.DonDonDoc,
                          item.DonChuyenDon, item.DonDonDoc,
                          item.DonChuyenPhucDap
                    );

                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_TongHopKetQuaXuLyDonToCao.xls");
                string filename = "BC_TongHopKetQuaXuLyDonToCao" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 8, 0);
                ws.Cells.DeleteRows(data.Count + 8, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult BC_TongHopKetQuaXuLyDonKienNghiPhanAnh(string IdDonVi, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Tên đơn vị");
                dt.Columns.Add("Tổng số đơn");
                dt.Columns.Add("Số đơn kỳ trước chuyển sang");
                dt.Columns.Add("Số đơn tiếp nhận trong kỳ");
                dt.Columns.Add("Tổng số 1");
                dt.Columns.Add("Đơn kỳ trước chuyển sang");
                dt.Columns.Add("Đơn tiếp nhận trong kỳ");
                dt.Columns.Add("Số đơn");
                dt.Columns.Add("Số vụ việc");
                dt.Columns.Add("Chế độ chính sách");
                dt.Columns.Add("Đất đai");
                dt.Columns.Add("Tư pháp");
                dt.Columns.Add("Khác");
                dt.Columns.Add("Đã giải quyết");
                dt.Columns.Add("Chưa giải quyết xong");
                dt.Columns.Add("Vụ việc thuộc thẩm quyền");
                dt.Columns.Add("Tổng số 2");
                dt.Columns.Add("Chuyển đơn");
                dt.Columns.Add("Đôn đốc, giải quyết");
                dt.Columns.Add("Số vụ việc đã được giải quyết");
                dt.Columns.Add("Số vụ việc chưa đã được giải quyết");

                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh(IdDonVi, TuNgay, DenNgay);
                int location = 8;
                foreach (BaoCao_TongHopKetQuaXuLyDonThuKienNghiPhanAnh item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++,
                         item.TenDonVi,
                         item.TNTruocKy + item.TNTrongKy,
                         item.TNTruocKy, item.TNTrongKy,
                         item.DonDaXuLyKyTruoc + item.DonDaXuLyTrongKy,
                         item.DonDaXuLyKyTruoc, item.DonDaXuLyTrongKy,
                         item.DonDuDieuKien,
                         item.CheDoChinhSach + item.DatDai + item.TuPhap + item.Khac,
                         item.CheDoChinhSach, item.DatDai, item.TuPhap,
                         item.Khac, item.DaDuocGiaiQuyet, item.ChuaDuocGiaiQuyet,
                         item.VuViecThuocThamQuyen,
                         item.DonChuyenDon + item.DonDonDoc,
                         item.DonChuyenDon, item.DonDonDoc,
                         item.VuViecDaGiaiQuyet, item.VuViecChuaGiaiQuyet
                     );

                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_TongHopKetQuaXuLyDonKienNghiPhanAnh.xls");
                string filename = "BC_TongHopKetQuaXuLyDonKienNghiPhanAnh" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 8, 0);
                ws.Cells.DeleteRows(data.Count + 8, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult BC_TongHopKetQuaTiepCongDan01(string IdDonVi, string TuNgay, string DenNgay)
        {
            var session = Session["Users"];
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("STT");
                dt.Columns.Add("Tên đơn vị");
                dt.Columns.Add("Tổng số đơn phải xử lý");
                dt.Columns.Add("Số đơn đã xử lý");
                dt.Columns.Add("Đủ điều kiện xử lý");
                dt.Columns.Add("Số lượt tiếp");
                dt.Columns.Add("Số người được tiếp");
                dt.Columns.Add("Tiếp lần đầu 1");
                dt.Columns.Add("Tiếp nhiều lần 1");
                dt.Columns.Add("Số đoàn được tiếp 1");
                dt.Columns.Add("Số người được tiếp 1");
                dt.Columns.Add("Số vụ việc tiếp lần đầu 1");
                dt.Columns.Add("Số vụ việc tiếp nhiều lần 1");
                dt.Columns.Add("Số kỳ tiếp 1");
                dt.Columns.Add("Số lượt tiếp 1");
                dt.Columns.Add("Số người được tiếp 2");
                dt.Columns.Add("Tiếp lần đầu 2");
                dt.Columns.Add("Tiếp nhiều lần 2");
                dt.Columns.Add("Số đoàn được tiếp 2");
                dt.Columns.Add("Số người được tiếp 3");
                dt.Columns.Add("Số vụ việc tiếp lần đầu 2");
                dt.Columns.Add("Số vụ việc tiếp nhiều lần 2");
                dt.Columns.Add("Số kỳ tiếp 2");
                dt.Columns.Add("Số lượt tiếp 2");
                dt.Columns.Add("Số người được tiếp 4");
                dt.Columns.Add("Tiếp lần đầu 3");
                dt.Columns.Add("Tiếp nhiều lần 3");
                dt.Columns.Add("Số đoàn được tiếp 3");
                dt.Columns.Add("Số người được tiếp 5");
                dt.Columns.Add("Số vụ việc tiếp lần đầu 3");
                dt.Columns.Add("Số vụ việc tiếp nhiều lần 3");


                int i = 1;

                //if (TuNgay != "")
                //{
                //    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                //if (DenNgay != "")
                //{
                //    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                //}
                _obj = new BaoCaoRp();
                var data = _obj.BaoCao_TongHopKetQuaTiepCongDan01(IdDonVi, TuNgay, DenNgay);
                int location = 8;
                foreach (BaoCao_TongHopKetQuaTiepCongDan01 item in data)
                {
                    location = location + 1;
                    dt.Rows.Add(i++,
                        item.TenDonVi,

                        // Tổng cộng từ các nhóm
                        item.TTXSoLuot + item.TDKSoLuot + item.TDK_UQSoLuot,
                        item.TTXSoNguoi + item.TDKSoNguoi + item.TDK_UQSoNguoi,
                        item.TTXVuViecLanDau + item.TTXVuViecNhieuLan
                            + item.TDKVuViecLanDau + item.TDKVuViecNhieuLan
                            + item.TDK_UQVuViecLanDau + item.TDK_UQVuViecNhieuLan,

                        // TTX
                        item.TTXSoLuot, item.TTXSoNguoi, item.TTXVuViecLanDau,
                        item.TTXVuViecNhieuLan, item.TTXSoDoan, item.TTXSoNguoiTrongDoan,
                        item.TTXSoDoanLanDau, item.TTXSoDoanNhieuLan, item.TDKSoky,

                        // TDK
                        item.TDKSoLuot, item.TDKSoNguoi, item.TDKVuViecLanDau,
                        item.TDKVuViecNhieuLan, item.TDKSoDoan, item.TDKSoNguoiTrongDoan,
                        item.TDKSoDoanLanDau, item.TDKSoDoanNhieuLan, item.TDK_UQSoKy,

                        // TDK Ủy quyền
                        item.TDK_UQSoLuot, item.TDK_UQSoNguoi, item.TDK_UQVuViecLanDau,
                        item.TDK_UQVuViecNhieuLan, item.TDK_UQSoDoan, item.TDK_UQSoNguoiTrongDoan,
                        item.TDK_UQSoDoanLanDau, item.TDK_UQSoDoanNhieuLan
                    );

                }
                string tempfile = Server.MapPath("~/Exports/Templates/BC_TongHopKetQuaTiepCongDan01.xls");
                string filename = "BC_TongHopKetQuaTiepCongDan01" + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
                string foldername = "/Exports/Files/";
                DeleteFile(Server.MapPath(foldername));
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }
                string savefile = Server.MapPath(foldername) + filename;
                Workbook wb = new Workbook();
                wb.Open(tempfile);
                Worksheet ws = wb.Worksheets[0];

                if (TuNgay != "")
                {
                    TuNgay = DateTime.ParseExact(TuNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                if (DenNgay != "")
                {
                    DenNgay = DateTime.ParseExact(DenNgay, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString();
                }
                string tuNgayFormatted = string.IsNullOrEmpty(TuNgay) ? "" : DateTime.Parse(TuNgay).ToString("dd/MM/yyyy");
                string denNgayFormatted = string.IsNullOrEmpty(DenNgay) ? "" : DateTime.Parse(DenNgay).ToString("dd/MM/yyyy");
                ws.Cells["A4"].PutValue("Số liệu tính từ ngày " + tuNgayFormatted + " đến ngày " + denNgayFormatted);

                ws.Cells.ImportDataTable(dt, false, 9, 0);
                ws.Cells.DeleteRows(data.Count + 9, 100);
                wb.Save(savefile);

                return Json(foldername + filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        private void DeleteFile(string path)
        {
            if (!Directory.Exists(path)) return;
            string[] filename = Directory.GetFiles(path);
            foreach (string file in filename)
            {
                if (System.IO.File.Exists(file) == true)
                {
                    System.IO.File.Delete(file);
                }
            }
        }  
    }
}
