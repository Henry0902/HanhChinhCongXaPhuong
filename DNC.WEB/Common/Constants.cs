using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNC.WEB.Common
{
    public class Constants
    {
        public const int STATUS_ACTIVE = 0;
        public const int STATUS_NOT_ACTIVE = 1;

        public const int PHIEU_TAO_MOI = 1;
        public const int PHIEU_DA_DUYET = 2;
        public const int PHIEU_DA_TRA = 3;

        public const string ITEMS = "Items";
        public const string PAGE_SIZE = "pageSize";
        public const string PAGE_INDEX = "pageIndex";
        public const string TOTAL_RECORDS = "TotalRecords";

        public const string PATH_FOLDER_FILESUPLOAD= "/FilesUpload/";

        public const string TIME_DAY_MIN = " 00:00:00";
        public const string TIME_DAY_MAX = " 23:59:59";
        public const string FORMAT_DATE_AND_TIME = "dd/MM/yyyy HH:mm:ss";

        public const int FileType_DonThu = 1;
        public const int FileType_XuLy = 2;
        public const int FileType_KetLuan = 3;

        public const int TrangThaiTiepNhan = 1;
        public const int TrangThaiXuLy = 2;
        public const int TrangThaiDuyetXuLy = 3;
        public const int TrangThaiThuLyGiaiQuyet = 4;
        public const int TrangThaiKetLuan = 5;
        public const int TrangThaiDuyetGiaiQuyet = 6;
        public const int TrangThaiTraKetQua = 7;
        public const int TrangThaiDaKetThuc = 99;

        //API
        public const string DATA = "DATA";

        public const string TYPE = "type";
        public const string CODE = "code";
        public const string STATUS = "status";
        public const string MESSAGE = "message";
        public const string SUCCESS_MESSAGE = "Successfully";
        public const string ERROR_MESSAGE = "Error try ... catch";
        public const int SUCCESS_CODE = 200;
        public const int ERROR = 400;
        public const int ERR_PASS = 100;
        public const int ERR_USERNAME = 101;
        public const int ERR_LOCK = 102;
        public const int ERR_DELETE = 103;
        public const int ERR_KHOAHOP = 104;
        public const int ERR_EXPIRE = 403;
        public const int ERR_END = 402;

        //Authen API
        public const int AUTH_NULL = 410;
        public const int AUTH_TOKEN_EMPTY = 411;
        public const int AUTH_TOKEN_INVALID = 412;

        //Huong Xu Ly
        public const int HXL_CHUYEN_DON = 2;
        public const int STATUS_DUYET_CHUYEN_DON = 5;
    }
}