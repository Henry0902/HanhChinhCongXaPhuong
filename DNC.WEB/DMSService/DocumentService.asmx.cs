using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Xml;
using DNC.WEB.Models;

namespace DNC.WEB.DMSService
{
    /// <summary>
    /// Summary description for DocumentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DocumentService : System.Web.Services.WebService
    {
        private DbConnectContext _db = new DbConnectContext();

        [WebMethod]
        public string AddDoc(string usernameService, string passwordService, string xmlOrJsonData, int dataType)
        {

            string sql = "";
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    int result = _db.Database.ExecuteSqlCommand(sql);
                    trans.Commit();
                    return "OK: " + result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "ERR:3 " + ex.ToString();
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }

        [WebMethod]
        public string AddListDocs(string usernameService, string passwordService, string xmlOrJsonData, int dataType)
        {
            UserService userService =
                _db.UserService.Where(x => x.userservice_name == usernameService && x.userservice_password == passwordService).OrderBy(x => x.ngay_tao).FirstOrDefault();
            if (userService == null)
            {
                return "ERR:1";
            }
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    string sqlInsert = "INSERT INTO Docs(doc_ten, doc_sohieu, doc_soto, doc_kyhieu, doc_trichdan, ngay_ban_hanh, ngay_hieuluc_from, " +
                                       "ngay_hieuluc_to, ngay_luutru_from, ngay_luutru_to, ngay_baoquan_from, ngay_baoquan_to, kho_id, kegia_id, hopcap_id, " +
                                       "hoso_id, cqbh_id, ttvl_id, loaivb_id, linhvuc_id, donvi_id, thoihan_id, trang_thai, user_created_id, ghichu, ngay_tao) ";
                    string sqlValue = " VALUES ";
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlOrJsonData);
                    XmlNodeList parentNode = xmlDoc.GetElementsByTagName("Doc");
                    int length = parentNode.Count;
                    int count = 1;
                    DateTime ngayTao = DateTime.Now;
                    foreach (XmlNode childrenNode in parentNode)
                    {
                        string fKey = childrenNode.SelectSingleNode("Key") == null ? "" : childrenNode.SelectSingleNode("Key").InnerText;
                        string docTen = childrenNode.SelectSingleNode("DocTen") == null ? "" : childrenNode.SelectSingleNode("DocTen").InnerText;
                        string docSohieu = childrenNode.SelectSingleNode("DocSohieu") == null ? "" : childrenNode.SelectSingleNode("DocSohieu").InnerText;
                        string docSoto = childrenNode.SelectSingleNode("DocSoto") == null ? "0" : childrenNode.SelectSingleNode("DocSoto").InnerText;
                        string docKyhieu = childrenNode.SelectSingleNode("DocKyhieu") == null ? "" : childrenNode.SelectSingleNode("DocKyhieu").InnerText;
                        string docTrichyeu = childrenNode.SelectSingleNode("DocTrichyeu") == null ? "" : childrenNode.SelectSingleNode("DocTrichyeu").InnerText;
                        string ngayBanhanh = childrenNode.SelectSingleNode("NgayBanhanh") == null ? "" : childrenNode.SelectSingleNode("NgayBanhanh").InnerText;
                        string ngayHieulucFrom = childrenNode.SelectSingleNode("NgayHieulucFrom") == null ? "" : childrenNode.SelectSingleNode("NgayHieulucFrom").InnerText;
                        string ngayHieulucTo = childrenNode.SelectSingleNode("NgayHieulucTo") == null ? "" : childrenNode.SelectSingleNode("NgayHieulucTo").InnerText;
                        string ngayLuutruFrom = childrenNode.SelectSingleNode("NgayLuutruFrom") == null ? "" : childrenNode.SelectSingleNode("NgayLuutruFrom").InnerText;
                        string ngayLuutruTo = childrenNode.SelectSingleNode("NgayLuutruTo") == null ? "" : childrenNode.SelectSingleNode("NgayLuutruTo").InnerText;
                        string ngayBaoquanFrom = childrenNode.SelectSingleNode("NgayBaoquanFrom") == null ? "" : childrenNode.SelectSingleNode("NgayBaoquanFrom").InnerText;
                        string ngayBaoquanTo = childrenNode.SelectSingleNode("NgayBaoquanTo") == null ? "" : childrenNode.SelectSingleNode("NgayBaoquanTo").InnerText;
                        string khoId = childrenNode.SelectSingleNode("KhoId") == null ? "0": childrenNode.SelectSingleNode("KhoId").InnerText;
                        string kegiaId = childrenNode.SelectSingleNode("KegiaId") == null ? "0" : childrenNode.SelectSingleNode("KegiaId").InnerText;
                        string hopcapId = childrenNode.SelectSingleNode("HopcapId") == null ? "0" : childrenNode.SelectSingleNode("HopcapId").InnerText;
                        string hosoId = childrenNode.SelectSingleNode("HosoId") == null ? "0" : childrenNode.SelectSingleNode("HosoId").InnerText;
                        string cqbhId = childrenNode.SelectSingleNode("CoquanbhId") == null ? "0" : childrenNode.SelectSingleNode("CoquanbhId").InnerText;
                        string ttvlId = childrenNode.SelectSingleNode("TinhtrangId") == null ? "0" : childrenNode.SelectSingleNode("TinhtrangId").InnerText;
                        string loaivbId = childrenNode.SelectSingleNode("LoaivbId") == null ? "0" : childrenNode.SelectSingleNode("LoaivbId").InnerText;
                        string linhvucId = childrenNode.SelectSingleNode("LinhvucId") == null ? "0" : childrenNode.SelectSingleNode("LinhvucId").InnerText;

                        DateTime? dayBanhanh = null;
                        DateTime? dayHieulucFrom = null;
                        DateTime? dayHieulucTo = null;
                        DateTime? dayLuutruFrom = null;
                        DateTime? dayLuutruTo = null;
                        DateTime? dayBaoquanFrom = null;
                        DateTime? dayBaoquanTo = null;

                        sqlValue += "(N'" + docTen + "', N'" + docSohieu + "', " + docSoto + ", N'" + docKyhieu + "', N'" + docTrichyeu + "'";

                        if (!string.IsNullOrEmpty(ngayBanhanh))
                        {
                            dayBanhanh = DateTime.ParseExact(ngayBanhanh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlValue += ", '" + dayBanhanh + "'";
                        }
                        else
                        {
                            sqlValue += ", null";
                        }
                        if (!string.IsNullOrEmpty(ngayHieulucFrom))
                        {
                            dayHieulucFrom = DateTime.ParseExact(ngayHieulucFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlValue += ", '" + dayHieulucFrom + "'";
                        }
                        else
                        {
                            sqlValue += ", null";
                        }
                        if (!string.IsNullOrEmpty(ngayHieulucTo))
                        {
                            dayHieulucTo = DateTime.ParseExact(ngayHieulucTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlValue += ", '" + dayHieulucTo + "'";

                        }
                        else
                        {
                            sqlValue += ", null";
                        }
                        if (!string.IsNullOrEmpty(ngayLuutruFrom))
                        {
                            dayLuutruFrom = DateTime.ParseExact(ngayLuutruFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlValue += ", '" + dayLuutruFrom + "'";
                        }
                        else
                        {
                            sqlValue += ", null";
                        }
                        if (!string.IsNullOrEmpty(ngayLuutruTo))
                        {
                            dayLuutruTo = DateTime.ParseExact(ngayLuutruTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlValue += ", '" + dayLuutruTo + "'";
                        }
                        else
                        {
                            sqlValue += ", null";
                        }
                        if (!string.IsNullOrEmpty(ngayBaoquanFrom))
                        {
                            dayBaoquanFrom = DateTime.ParseExact(ngayBaoquanFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlValue += ", '" + dayBaoquanFrom + "'";
                        }
                        else
                        {
                            sqlValue += ", null";
                        }
                        if (!string.IsNullOrEmpty(ngayBaoquanTo))
                        {
                            dayBaoquanTo = DateTime.ParseExact(ngayBaoquanTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            sqlValue += ", '" + dayBaoquanTo + "'";
                        }
                        else
                        {
                            sqlValue += ", null";
                        }

                        sqlValue += ", " + khoId + ", " + kegiaId + ", " + hopcapId + ", " + hosoId + "" +
                                    ", " + cqbhId + ", " + ttvlId + ", " + loaivbId + ", " + linhvucId + ", 0, 0, 0, 0, '', '" + ngayTao + "')";
                        if (count < length)
                        {
                            sqlValue += " , ";
                        }
                        
                        count++;
                    }
                    string sql = sqlInsert + sqlValue;
                    int result = _db.Database.ExecuteSqlCommand(sql);
                    trans.Commit();
                    return "OK:" + result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "ERR:3" + ex.ToString();
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }

        [WebMethod]
        public string UpdateLinhvuc(string usernameService, string passwordService, string xmlData)
        {

            string sql = "";
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    string sqlInsert = "";
                    string sqlValue = " VALUES ";
                    int result = _db.Database.ExecuteSqlCommand(sql);
                    trans.Commit();
                    return "OK: " + result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "ERR:3 " + ex.ToString();
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }

        [WebMethod]
        public string UpdateLoaiVanban(string usernameService, string passwordService, string xmlData)
        {

            string sql = "";
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    string sqlInsert = "";
                    string sqlValue = " VALUES ";
                    int result = _db.Database.ExecuteSqlCommand(sql);
                    trans.Commit();
                    return "OK: " + result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "ERR:3 " + ex.ToString();
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }

        [WebMethod]
        public string UpdateCQBanhanh(string usernameService, string passwordService, string xmlData)
        {

            string sql = "";
            using (var trans = _db.Database.BeginTransaction())
            {
                try
                {
                    string sqlInsert = "";
                    string sqlValue = " VALUES ";
                    int result = _db.Database.ExecuteSqlCommand(sql);
                    trans.Commit();
                    return "OK: " + result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return "ERR:3 " + ex.ToString();
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }
    }
}
