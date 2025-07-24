
myApp.controller("TiepNhanDonThuJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore) {
        // Tham số   
        $scope.namID = -1;      

        $scope.DonThu = {};
        $scope.DonThu.IdDonThu = null;
        $scope.DonThu.IdNguonDon = 1; // Mạc định là bưu chính chuyển phát
        $scope.DonThu.IdDoiTuong = 1; // Mạc định là cá nhân
        $scope.DonThu.GioiTinh = 0;
        $scope.DonThu.IdTinhThanh = 60; // Mạc định là Bắc Ninh
        $scope.DonThu.IdQuanHuyen = -1;
        $scope.DonThu.IdPhuongXa = -1;
        $scope.DonThu.IdQuocTich = 1; // Mạc định là Việt Nam
        $scope.DonThu.IdDanToc = 1; // Mạc định là Kinh       
        $scope.DonThu.IdLoaiDonThu = -1;
        $scope.DonThu.IdLoaiKNTC = -1;
        $scope.DonThu.IdLoaiKNTCChiTiet = -1;
        //$scope.DonThu.LanGiaiQuyet = 0;
        $scope.DonThu.LanTiepNhan = 0;
        $scope.DonThu.Pre_Status = 0;
        $scope.CheckDonThuXuLy = false;
        $scope.CheckThongTinDonThu = true;
        $scope.CheckThongTinNguoiBiKNTC = false;
        $scope.CheckThongTinNguoiDaiDien = false;

        $scope.HoTenSearch = '';
        $scope.NoiDungSearch = '';
        $scope.CMTNDSearch = '';
        $scope.IdTinhThanhSearch = -1; 
        $scope.IdQuanHuyenSearch = -1;
        $scope.IdPhuongXaSearch = -1;

        $scope.lstDonThu = [];

        $scope.today = new Date();       
        $scope.lstPage = [];
        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------
        var lblsohieuhs = document.getElementById("lblsohieuhs");
        var lbltenhs = document.getElementById("lbltenhs");
        var lblmahs = document.getElementById("lblmahs");
        var lblsohs = document.getElementById("lblsohs");
    
        //-------------------------------------------------------------------------------        

        var date = new Date();
        $scope.DonThu.NgayNhap = ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();

        // Convert datetime
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        //Lấy danh sách năm
        $scope.lstNam = [];
        var getListNam = function () {
            var currentTime = new Date();
            var nam = currentTime.getFullYear();
            $scope.namID = nam;
            for (i = nam; i > nam - 5; i--) {
                $scope.lstNam.push(i);
            }
        }          

        // lấy danh sách nguồn đơn
        $scope.lstNguonDon = [];
        var getlstNguonDon = function () {
            crudService.getAll("/DmNguonDon/GetAll")
                .success(function (data) {
                    $scope.lstNguonDon = data;
                    //console.log("lstNguonDon", data);
                });
        }   

        // lấy danh sách loại đối tượng
        $scope.lstLoaiDoiTuong = [];
        var getlstLoaiDoiTuong = function () {
            crudService.getAll("/DmLoaiDoiTuong/getAll")
                .success(function (data) {
                    $scope.lstLoaiDoiTuong = data;
                });
        }
       

        // lấy danh sách tỉnh thành
        $scope.lstTinhThanh = [];
        $scope.lstTinhThanhSearch = [];
        var getlstTinhThanh = function () {
            crudService.getAll("/DmTinhThanh/getAllDropDown")
                .success(function (data) {
                    $scope.lstTinhThanh = data.listTinhThanh;
                    $scope.lstTinhThanhSearch = data.listTinhThanh;

                    if ($scope.DonThu.IdTinhThanh != -1)
                    {
                        $scope.getlstQuanHuyen($scope.DonThu.IdTinhThanh);
                    }
                }).error(function (error) {
                    $.notify("Lấy danh sách tỉnh thành bị lỗi.", "error");
                });;
        }

        var getlstTinhThanhCookie = function () {
            crudService.getAll("/DmTinhThanh/getAllDropDown")
                .success(function (data) {
                    $scope.lstTinhThanh = data.listTinhThanh;
                    $scope.lstTinhThanhSearch = data.listTinhThanh;
                }).error(function (error) {
                    $.notify("Lấy danh sách tỉnh thành bị lỗi.", "error");
                });;
        }

        // Lấy danh sách quận huyện
        $scope.lstQuanHuyen = [];        
        $scope.getlstQuanHuyen = function (id) {
            crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", id)
                .success(function (data) {
                    $scope.lstQuanHuyen = data;
                    $scope.DonThu.IdQuanHuyen = -1;
                    $scope.lstPhuongXa = [];
                    $scope.DonThu.IdPhuongXa = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
                });
        }

        $scope.lstQuanHuyenSearch = [];
        $scope.getlstQuanHuyenSearch = function (id) {
            crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", id)
                .success(function (data) {
                    $scope.lstQuanHuyenSearch = data;
                    $scope.IdQuanHuyenSearch = -1;
                    $scope.lstPhuongXaSearch = [];
                    $scope.IdPhuongXaSearch = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
                });
        }

        var getlstQuanHuyenCookie = function (id) {
            crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", id)
                .success(function (data) {
                    $scope.lstQuanHuyen = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
                });
        }

        // Lấy danh sách Phường xã
        $scope.lstPhuongXa = [];
        $scope.getlstPhuongXa = function (id) {
            crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", id)
                .success(function (data) {
                    $scope.lstPhuongXa = data;
                    $scope.DonThu.IdPhuongXa = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                });
        }

        $scope.lstPhuongXaSearch = [];
        $scope.getlstPhuongXaSearch = function (id) {
            crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", id)
                .success(function (data) {
                    $scope.lstPhuongXaSearch = data;
                    $scope.IdPhuongXaSearch = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                });
        }

        var getlstPhuongXaCookie = function (id) {
            crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", id)
                .success(function (data) {
                    $scope.lstPhuongXa = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                });
        }

        // Lấy danh sách quốc tịch
        $scope.lstQuocTich = [];
        var getlstQuocTich = function () {
            crudService.getAll("/DmQuocTich/getAll")
                .success(function (data) {
                    $scope.lstQuocTich = data;
                });
        }

        // Lấy danh sách dân tộc
        $scope.lstDanToc = [];
        var getlstDanToc = function () {
            crudService.getAll("/DmDanToc/getAll")
                .success(function (data) {
                    $scope.lstDanToc = data;
                });
        }

        // Lấy danh sách loại hồ sơ
        $scope.lstLoaiDonThu = [];
        var getlstLoaiDonThu = function () {
            crudService.getAll("/DmLoaiDonThu/getAll")
                .success(function (data) {
                    $scope.lstLoaiDonThu = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách loại hồ sơ bị lỗi.", "error");
                });;
        }        

        // Lấy danh sách loại KNTC by loại hồ sơ
        $scope.lstLoaiKNTC = [];
        $scope.getlstLoaiKNTC = function (id) {
            crudService.get("/DmLoaiKNTC/GetLoaiKNTCByLoaiDonThu/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTC = data;
                    $scope.DonThu.IdLoaiKNTC = '-1';
                    $scope.DonThu.IdLoaiKNTCChiTiet = '-1';
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC bị lỗi.", "error");
                });
        }

        var getlstLoaiKNTCCookie = function (id) {
            crudService.get("/DmLoaiKNTC/GetLoaiKNTCByLoaiDonThu/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTC = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC bị lỗi.", "error");
                });
        }

        // Lấy danh sách loại KNTC chi tiết
        $scope.lstLoaiKNTCChiTiet = [];
        $scope.getlstLoaiKNTCChiTiet = function (id) {
            crudService.get("/DmLoaiKNTCCT/GetLoaiKNTCCTByLoaiKNTC/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTCChiTiet = data;
                    $scope.DonThu.IdLoaiKNTCChiTiet = '-1';
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC chi tiết bị lỗi.", "error");
                });
        }

        var getlstLoaiKNTCChiTietCookie = function (id) {
            crudService.get("/DmLoaiKNTCCT/GetLoaiKNTCCTByLoaiKNTC/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTCChiTiet = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC chi tiết bị lỗi.", "error");
                });
        }

        // Init data
        var init = function () {
            var IdDonThu = crudService.getCookie("IdDonThu");
            $("#divLoading").show();

            if (IdDonThu != '' && IdDonThu != null && IdDonThu != undefined) {                
                crudService.removeCookie("IdDonThu");
                crudService.get("/DonThu/GetInfoByID?id=", IdDonThu)
                   .success(function (data) {                       
                       data.NgayNhap = parseDateV(data.NgayNhap);
                       data.NgayCap = parseDateV(data.NgayCap);
                       data.NgayTao = parseDateV(data.NgayTao);
                       $scope.DonThu = data;
                       getFileByIdDonThu($scope.DonThu.Id);

                       
                       getlstNguonDon();
                       getlstLoaiDoiTuong();

                       getlstTinhThanhCookie();
                       getlstQuanHuyenCookie($scope.DonThu.IdTinhThanh);
                       getlstPhuongXaCookie($scope.DonThu.IdQuanHuyen);

                       getlstQuocTich();
                       getlstDanToc();

                       getlstLoaiDonThu();                       
                       getlstLoaiKNTCCookie($scope.DonThu.IdLoaiDonThu);
                       getlstLoaiKNTCChiTietCookie($scope.DonThu.IdLoaiKNTC);

                       $("#txtNgayNhap").datepicker('setDate', data.NgayNhap);

                       $("#divLoading").hide();
                   }).error(function (error) {
                       $.notify("Không tìm thấy dữ liệu!", "error");
                       $("#divLoading").hide();
                   });
            }
            else
            {
                
                getlstNguonDon();
                getlstLoaiDoiTuong();
                getlstTinhThanh();
                getlstQuocTich();
                getlstDanToc();
                getlstLoaiDonThu();
                $("#divLoading").hide();
            }
        }

        init();

        //get data doc id
        var getFileByIdDonThu = function (id) {
            GetAllFileByObjectId(id, $scope.FileType)
                .success(function (data) {
                    $scope.lstDataForPlugin = data;
                });
        }

        var GetAllFileByObjectId = function (Id, FileType) {
            return $http({
                url: "/Files/GetAllFileByObjectId",
                method: "GET",
                params: {
                    Id: Id,
                    FileType: FileType
                }
            });
        }

        $scope.ThemMoi = function () {
            //    $scope.lstPage = [];
            $scope.DonThu = {};
            $scope.DonThu.NgayNhap = ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();
            $scope.DonThu.IdDonThu = null;
            $scope.DonThu.IdNguonDon = 1;
            $scope.DonThu.IdDoiTuong = 1;
            $scope.DonThu.GioiTinh = 0;
            $scope.DonThu.IdTinhThanh = 6;
            $scope.DonThu.IdQuanHuyen = -1;
            $scope.DonThu.IdPhuongXa = -1;
            $scope.DonThu.IdQuocTich = 1;
            $scope.DonThu.IdDanToc = 1;
            $scope.DonThu.IdLoaiDonThu = -1;
            $scope.DonThu.IdLoaiKNTC = -1;
            $scope.DonThu.IdLoaiKNTCChiTiet = -1;
            $scope.CheckDonThuXuLy = false;
            $scope.CheckThongTinNguoiBiKNTC = false;
            $scope.CheckThongTinNguoiDaiDien = false;

            $scope.lstDataForPlugin = [];
            $scope.fileList = [];
        }      

        var parseDateV = function (value) {
            if (value) {
                var dateString = value.substr(6);
                var currentTime = new Date(parseInt(dateString));
                var month = currentTime.getMonth() + 1;
                var day = currentTime.getDate();
                var year = currentTime.getFullYear();
                if (day < 10) { day = "0" + day; }
                if (month < 10) { month = "0" + month; }
                var date = day + "/" + month + "/" + year;
                return date;
            }
            return null;
        }

        // Thực hiện lưu trên modal Insert Update
        $scope.preCreate = function (data) {
            if (data == null) {
                $("#txtNgayNhap").notify("Nhập ngày nhập đơn", "error");
                return;
            }

            if ($scope.CheckDonThuXuLy == false) {
                data.IdDonThuXuLy = 0;
                data.IdTrangThai = 2;
            }
            else {
                data.IdDonThuXuLy = 1;
                data.IdTrangThai = 99;
            }

            if (data.Id == null || data.Id == "") {
                if (validate(data) == false) {
                    return;
                }
                create(data);
            }
            else {
                if (validate(data) == false) {
                    return;
                }
                update(data);
            }
        }

        var validate = function (data) {

            if (data.IdNguonDon == null || data.IdNguonDon == -1) {
                $("#ddlNguonDon").notify("Chọn nguồn hồ sơ đến", "error");
                $("#ddlNguonDon").focus();
                return false;
            }

            if (data.NgayNhap == null || data.NgayNhap == "") {
                $("#txtNgayNhap").notify("Nhập ngày nhập đơn", "error");
                $("#txtNgayNhap").focus();
                return false;
            }

            if (data.IdDoiTuong == null || data.IdDoiTuong == -1) {
                $("#ddlLoaiDoiTuong").notify("Chọn loại đối tượng tiếp dân", "error");
                $("#ddlLoaiDoiTuong").focus();
                return false;
            }

            if (data.SoNguoi == null || data.SoNguoi == "") {
                $("#txtSoNguoi").notify("Nhập số người", "error");
                $("#txtSoNguoi").focus();
                return false;
            }

            if (data.HoTen == null || data.HoTen == "") {
                $("#txtHoTen").notify("Nhập họ tên", "error");
                $("#txtHoTen").focus();
                return false;
            }
            
            if (data.IdLoaiDonThu == null || data.IdLoaiDonThu == -1) {
                    $("#ddlLoaiDonThu").notify("Chọn loại hồ sơ", "error");
                    $("#ddlLoaiDonThu").focus();
                    return false;
            }
            
            //if (data.IdLoaiDonThu == 1 || data.IdLoaiDonThu == 2) {
            //    if (data.IdLoaiKNTC == null || data.IdLoaiKNTC == -1) {
            //        $("#ddlLoaiKNTC").notify("Chọn loại khiếu nại tố cáo", "error");
            //        $("#ddlLoaiKNTC").focus();
            //        return false;
            //    }
            //}

            if (data.IdLoaiKNTC == null || data.IdLoaiKNTC == -1) {
                $("#ddlLoaiKNTC").notify("Chọn loại khiếu nại tố cáo", "error");
                $("#ddlLoaiKNTC").focus();
                return false;
            }
            

            //if (data.LanTiepNhan == null) {
            //    $("#txtLanTiepNhan").notify("Nhập lần tiếp nhận", "error");
            //    $("#txtLanTiepNhan").focus();
            //    return false;
            //}
            
            if (data.NoiDungDonThu == null || data.NoiDungDonThu == "") {
                    $("#txtNoiDungDonThu").notify("Nhập nội dung hồ sơ", "error");
                    $("#txtNoiDungDonThu").focus();
                    return false;
            }
        }

        // Hàm thực hiện Create
        var create = function (data) {
            var dateSplit = "";
            if (data.NgayNhap != null && data.NgayNhap != "") {
                dateSplit = data.NgayNhap.split('/');
                data.NgayNhap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.NgayCap != null && data.NgayCap != "") {
                dateSplit = data.NgayCap.split('/');
                data.NgayCap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            data.IdDonViNhap = $cookieStore.get("DeparmentId");
            data.IdNguoiNhap = $cookieStore.get("UserID");
            data.IdDonViXuLy = $cookieStore.get("DeparmentId");

            crudService.create("/DonThu/CreateDonThu", data)
                .success(function (data) {
                    $.notify("Tạo mới thông tin hồ sơ thành công!", "success");
                    $scope.DonThu = data;
                    $scope.DonThu.NgayNhap = parseDateV(data.NgayNhap);
                    $scope.DonThu.NgayCap = parseDateV(data.NgayCap);

                    //Upload file sau khi tạo hồ sơ xong
                    if ($scope.fileList.length != 0) {
                        for (var i = 0; i < $scope.fileList.length; i++) {
                            fileUploadService.UploadedFile($scope.fileList[i].file, data.Id, $scope.FileType, data.IdDonThuGoc).then(function (data) {
                                $scope.lstDataForPlugin.push(data);

                            });
                        }
                        $scope.fileList = [];
                    }

                    $scope.XuLyDonThu = {};
                    $scope.XuLyDonThu.IdDonThu = data.Id;
                    $scope.XuLyDonThu.IdTrangThai = "1";
                    $scope.XuLyDonThu.IdDonVi = $cookieStore.get("DeparmentId");
                    $scope.XuLyDonThu.IdNguoiTao = $cookieStore.get("UserID");
                    $scope.XuLyDonThu.VuViecId = data.VuViecId;
                    $scope.XuLyDonThu.IdDonThuGoc = data.IdDonThuGoc;
                    crudService.create("/XuLyDonThu/Create", $scope.XuLyDonThu)
                    
                }).error(function (error) {
                    $.notify("Tạo mới thông tin hồ sơ thất bại!", "error");
                });
        }

        // Hàm thực hiện Update
        var update = function (data) {
            var dateSplit = "";
            if (data.NgayNhap != null && data.NgayNhap != "") {
                dateSplit = data.NgayNhap.split('/');
                data.NgayNhap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
                $scope.DonThu.NgayNhap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.NgayCap != null && data.NgayCap != "") {
                dateSplit = data.NgayCap.split('/');
                data.NgayCap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
                $scope.DonThu.NgayCap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }
            data.IdDonViNhap = $cookieStore.get("DeparmentId");
            data.IdNguoiNhap = $cookieStore.get("UserID");
            data.IdDonViXuLy = $cookieStore.get("DeparmentId");
            data.IdTrangThai = 2;
            crudService.update("/DonThu/Update", $scope.DonThu)
                .success(function (data) {
                    $.notify("Chỉnh sửa thông tin hồ sơ thành công !", "success");
                    $scope.DonThu = data;
                    $scope.DonThu.NgayNhap = parseDateV(data.NgayNhap);
                    $scope.DonThu.NgayCap = parseDateV(data.NgayCap);

                    $scope.DonThu.Id = data.Id;

                    //Upload file sau khi tạo hồ sơ xong
                    if ($scope.fileList.length != 0) {
                        for (var i = 0; i < $scope.fileList.length; i++) {
                            fileUploadService.UploadedFile($scope.fileList[i].file, data.Id, $scope.FileType, data.IdDonThuGoc).then(function (data) {
                                $scope.lstDataForPlugin.push(data);
                            });
                        }
                        $scope.fileList = [];
                    }
                }).error(function (error) {
                    $.notify("Chỉnh sửa thông tin hồ sơ thất bại !", "error");
                });
        }

        // Chuyển trang tiếp dân
        $scope.GoDonThuTiepNhan = function () {
            window.location.href = '../DonThu/DonThuTiepNhan';
        }

        //Load danh sách check hồ sơ
        var getData = function () {
            return $http({
                url: "/DonThu/SearchCheck",
                method: "GET",
                params: {
                    HoTen: $scope.HoTenSearch, CMTND: $scope.CMTNDSearch,
                    IdTinhThanh: $scope.IdTinhThanhSearch, IdQuanHuyen: $scope.IdQuanHuyenSearch, IdPhuongXa: $scope.IdPhuongXaSearch,
                    NoiDung: $scope.NoiDungSearch, order: $scope.Order, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex
                }
            });
        }

        // show modal check hồ sơ
        $scope.ShowModalCheckDonThu = function (data) {
            if (data.HoTen != null ) {
                $scope.HoTenSearch = data.HoTen;
            }
            else
            {
                $scope.HoTenSearch = '';
            }
            if (data.SoGiayTo != null ) {
                $scope.CMTNDSearch = data.SoGiayTo;
            }
            else {
                $scope.CMTNDSearch = '';
            }
            if (data.NoiDungDonThu != null ) {
                $scope.NoiDungSearch = data.NoiDungDonThu;
            }
            else {
                $scope.NoiDungSearch = '';
            }

            $scope.IdTinhThanhSearch = $scope.DonThu.IdTinhThanh;
            if ($scope.IdTinhThanhSearch != -1)
            {
                $scope.getlstQuanHuyenSearch($scope.IdTinhThanhSearch);
            }

            $scope.lstDonThu = [];
            $scope.totalRecords = 0;
            $scope.pageCount = 0;
        }

        // Hàm thực hiện check hồ sơ
        var InitCheck = function (data) {
            getData().success(function (data) {
               angular.forEach(data, function (item) {
                   item.NgayNhap = parseDate(item.NgayNhap);
               });
               $scope.lstDonThu = data;
               if (data.length > 0) {
                   $scope.totalRecords = data[0].TotalRecords;
               } else $scope.totalRecords = 0;
               $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
               for (i = 1; i <= $scope.pageCount && i <= 9; i++) {
                   $scope.lstPage.push(i);
               }
           })
           .error(function (error) {
               console.log(error);
           });
        }

        // Lấy về trang click vào trong phân trang
        $scope.selectPage = function (index) {
            if (index == 0) {
                index = 1;
            }
            $scope.pageIndex = index;
            $scope.lstPage.splice(0);
            var pageCount = $scope.pageCount;
            if (pageCount <= 9) {
                for (var i = 1; i <= pageCount; i++) {
                    $scope.lstPage.push(i);
                }
            } else {
                if (index >= 5) {
                    if (index <= pageCount - 4) {
                        for (i = index - 4; i <= index + 4 && i <= pageCount; i++) {
                            $scope.lstPage.push(i);
                        }
                    } else {
                        for (i = pageCount - 8; i <= pageCount; i++) {
                            $scope.lstPage.push(i);
                        }
                    }
                } else {
                    for (i = 1; i <= 9; i++) {
                        $scope.lstPage.push(i);
                    }
                }
            }

            if (!(index > pageCount)) {
                $scope.currentPage.stt = index;
            }

            if (!(index > pageCount)) {
                $scope.pageIndex = index;
                getData()
                .success(function (data) {
                    angular.forEach(data, function (item) {
                        item.NgayNhap = parseDate(item.NgayNhap);
                    });
                    $scope.lstDonThu = data;
                    $scope.totalRecords = data[0].TotalRecords;
                    $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
                })
                .error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
            }
        }

        var validateCheck = function () {
            if (($scope.HoTenSearch == null || $scope.HoTenSearch == '') && ($scope.CMTNDSearch == null || $scope.CMTNDSearch == '') && ($scope.NoiDungSearch == null || $scope.NoiDungSearch == '')){
                $("#txtHoTenSearch").notify("Nhập thông tin tìm kiếm", "warning");
                $("#txtHoTenSearch").focus();

                $("#txtCMTNDSearch").notify("Nhập thông tin tìm kiếm", "warning");

                $("#txtNoiDungSearch").notify("Nhập thông tin tìm kiếm", "warning");
                return false;
            }
            return true;
        }

        // Reload trang
        $scope.reloadCheck = function () {
            if (validateCheck() == true) {
                $scope.pageIndex = 1;
                $scope.currentPage.stt = 1;
                $scope.lstPage.splice(0);
                $scope.lstPage = [];
                InitCheck();
            }
        }

        // upload file
        $scope.fileList = [];

        $scope.ImageProperty = {
            file: ''
        }
        $scope.FileType = 1;
        $scope.lstDataForPlugin = [];

        var ValidateSize = function (file) {
            var FileSize = file.size / 1024 / 1024; // in MB
            if (FileSize > 10) {
                alert('Hãy chọn file có kích cỡ < 10mb');
                return false;
            }
            return true;
        }

        $scope.selectFileforUpload = function (element) {
            var fileupload = element.files;
            for (var i = 0; i < fileupload.length; i++) {
                $scope.ImageProperty.file = fileupload[i];
                if (ValidateSize($scope.ImageProperty.file)) {
                    $scope.fileList.push($scope.ImageProperty);
                }
                $scope.ImageProperty = {};
                $scope.$apply();
            }
        }

        $scope.removeFile = function (delitemfile) {
            $scope.fileList.splice($scope.fileList.indexOf(delitemfile), 1);
        }

        $scope.deleteFile = function (id) {
            crudService.remove("/Files/DeleteFiles", id)
            .success(function (result) {
                $.notify("Xóa file thành công!", "success");
                getFileByIdDonThu($scope.DonThu.Id);
            })
            .error(function (error) {
                $.notify("Xóa file thất bại!", "error");
            });

        }
    }).factory('fileUploadService', function ($http, $q) {
        var fac = {};
        fac.UploadedFile = function (file, docId, type, idDonThuGoc) {
            var deferred = $q.defer();
            var formData = new FormData();
            formData.append("file", file);
            formData.append("objectId", docId);
            formData.append("type", type);
            formData.append("idDonThuGoc", idDonThuGoc);
            $http.post("/Files/CreateFile", formData,
            {
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).success(function (data) {
                deferred.resolve(data);
            })
            .error(deferred.reject);
            return deferred.promise;
        }
        return fac;
    });



