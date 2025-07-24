using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DNC.WEB.Controllers;
using DNC.WEB.Models;

namespace DNC.WEB.Repository
{
    public class XuLyDonThuRp
    {
        private GenericRepository<XuLyDonThu> _context = null;
        private DbConnectContext _obj;
        //private Ultils logs = new Ultils();
        public XuLyDonThuRp()
        {
            _obj = new DbConnectContext();
        }

        public List<XuLyDonThuSearch> GetByIdTiepCongDan(string id)
        {
            var data = _obj.Database.SqlQuery<XuLyDonThuSearch>("XuLyDonThu_GetByIdTiepCongDan @id", new SqlParameter("@id", id)).ToList();
            return data;
        }

        public List<XuLyDonThuSearch> GetByIdDonThu(string id)
        {
            var data = _obj.Database.SqlQuery<XuLyDonThuSearch>("XuLyDonThu_GetByIdDonThu @id", new SqlParameter("@id", id)).ToList();
            return data;
        }

        public List<XuLyDonThuSearchView> GetViewByIdDonThu(string id)
        {
            var data = _obj.Database.SqlQuery<XuLyDonThuSearchView>("XuLyDonThu_GetByIdDonThu @id", new SqlParameter("@id", id)).ToList();
            return data;
        }

        public List<XuLyDonThuSearchView> GetViewByIdDonThuGoc(string idDonThuGoc)
        {
            var data = _obj.Database.SqlQuery<XuLyDonThuSearchView>("XuLyDonThu_GetByIdDonThuGoc @id", new SqlParameter("@id", idDonThuGoc)).ToList();
            return data;
        }
    }
}