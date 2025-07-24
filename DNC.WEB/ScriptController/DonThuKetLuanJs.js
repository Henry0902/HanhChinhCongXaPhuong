
myApp.controller("DonThuKetLuanJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore) {
        // Tham số  
        
        $scope.namID = -1;
        $scope.NguonDonId = -1;
        $scope.loaihsID = -1;
        $scope.TuNgay = '';
        $scope.DenNgay = '';
        $scope.Keyword = '';     

        $scope.lstNam = [];
        $scope.lstDonThu = [];

        $scope.lstXuLyDonThu = [];
        $scope.IdTrangThai = 5;
        $scope.DeparmentId = $cookieStore.get("DeparmentId");

        $scope.DonThu = {};
        $scope.KetLuan = {};
        $scope.XuLyDonThu = {};
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

        // Convert datetime
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        // lấy danh sách nguồn đơn
        $scope.lstNguonDon = [];
        var getlstNguonDon = function () {
            crudService.getAll("/Constants/GetDmNguonDon")
                .success(function (data) {
                    $scope.lstNguonDon = data;
                });
        }
        getlstNguonDon();

        // lấy danh sách nguồn đơn
        $scope.lstLoaiKetQua = [];
        var getlstLoaiKetQua = function () {
            crudService.getAll("/Constants/GetDmLoaiKetQua")
                .success(function (data) {
                    $scope.lstLoaiKetQua = data;
                });
        }
        getlstLoaiKetQua();

        //Load danh sách data
        var getData = function () {
            return $http({
                url: "/DonThu/GetXuLy",
                method: "GET",
                params: {
                    IdTrangThai: $scope.IdTrangThai, IdDonViXuLy: $scope.DeparmentId, IdNguonDon: $scope.NguonDonId, TuNgay: $scope.TuNgay, DenNgay: $scope.DenNgay,
                    Keyword: $scope.Keyword, order: $scope.Order, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex
                }
            });
        }

        // Init data
        var init = function () {
            $("#divLoading").show();
            getData()
            .success(function (data) {
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
                $("#divLoading").hide();
            })
            .error(function (error) {
                console.log(error);
                $("#divLoading").hide();
            });
        }

        init();        

        // Clear Data
        var clearData = function () {
            $scope.lstPage = [];
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

        // Reload trang
        $scope.reload = function () {
            $scope.pageIndex = 1;
            $scope.currentPage.stt = 1;
            $scope.lstPage.splice(0);
            $scope.lstPage = [];
            init();
        }

        //// Hiển thị thông tin modal Insert Update
        //$scope.show = function (id) {
        //    $scope.DonThu = {};
        //    getbyID(id);
        //    getXuLyDonThubyIdDonThu(id);            
        //}

        // Hiển thị thông tin modal Insert Update
        $scope.show = function (item) {
            $scope.DonThu = {};
            getbyID(item.Id);
            getXuLyDonThubyIdDonThu(item.IdDonThuGoc);
        }

        // Hiển thị thông tin modal Insert Update
        $scope.ShowKetLuan = function (id) {
            $scope.DonThu = {};            
            getbyID(id);
            $scope.KetLuan = {}
            $scope.KetLuan.IdDonThu = id;
            $scope.KetLuan.IdLoaiKetQua = -1;
            $scope.KetLuan.NoiDung = '';
            $scope.lstDataForPlugin = [];
            $scope.fileList = [];
            getKetLuanbyIdDonThu(id);
        }

        var getXuLyDonThubyIdDonThu = function (id) {
            crudService.get("/XuLyDonThu/GetViewByIdDonThu?id=", id)
                .success(function (data) {
                    angular.forEach(data, function (item) {
                        item.NgayTao = parseDate(item.NgayTao);
                        angular.forEach(item.FileUpload, function (items) {
                            if (items.file_extension == ".ppt" || items.file_extension == ".pptx" || items.file_extension == ".PPT" || items.file_extension == ".PPTX") {
                                items.icon = '/Content/assets/images/icon/ppt.png';
                            } else if (items.file_extension == ".doc" || items.file_extension == ".docx" || items.file_extension == ".DOC" || items.file_extension == ".DOCX") {
                                items.icon = '/Content/assets/images/icon/doc.png';
                            } else if (items.file_extension == ".xlsx" || items.file_extension == ".xls" || items.file_extension == ".XLSX" || items.file_extension == ".XLS") {
                                items.icon = '/Content/assets/images/icon/xlsx.png';
                            } else if (items.file_extension == ".pdf" || items.file_extension == ".PDF") {
                                items.icon = '/Content/assets/images/icon/pdf.png';
                            } else if (items.file_extension == ".rar" || items.file_extension == ".RAR" || items.file_extension == ".zip" || items.file_extension == ".ZIP") {
                                items.icon = '/Content/assets/images/icon/rar.png';
                            } else if (items.file_extension == ".jpeg" || items.file_extension == ".jpg" || items.file_extension == ".png" || items.file_extension == ".tif" || items.file_extension == ".gif" || items.file_extension == ".JPEG" || items.file_extension == ".JPG" || items.file_extension == ".PNG" || items.file_extension == ".TIF" || items.file_extension == ".GIF") {
                                items.icon = '/Content/assets/images/icon/jpg.png';
                            }
                        });
                    });
                    $scope.lstXuLyDonThu = data;
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        var getbyID = function (id) {
            crudService.get("/DonThu/GetInfoViewByID?id=", id)
                .success(function (data) {
                    data.NgayNhap = parseDateV(data.NgayNhap);
                    data.NgayCap = parseDateV(data.NgayCap);
                    data.NgayTao = parseDateV(data.NgayTao);
                    data.NgayThoiHanThuLy = parseDateV(data.NgayThoiHanThuLy);
                    angular.forEach(data.FileUpload, function (items) {
                        if (items.file_extension == ".ppt" || items.file_extension == ".pptx" || items.file_extension == ".PPT" || items.file_extension == ".PPTX") {
                            items.icon = '/Content/assets/images/icon/ppt.png';
                        } else if (items.file_extension == ".doc" || items.file_extension == ".docx" || items.file_extension == ".DOC" || items.file_extension == ".DOCX") {
                            items.icon = '/Content/assets/images/icon/doc.png';
                        } else if (items.file_extension == ".xlsx" || items.file_extension == ".xls" || items.file_extension == ".XLSX" || items.file_extension == ".XLS") {
                            items.icon = '/Content/assets/images/icon/xlsx.png';
                        } else if (items.file_extension == ".pdf" || items.file_extension == ".PDF") {
                            items.icon = '/Content/assets/images/icon/pdf.png';
                        } else if (items.file_extension == ".rar" || items.file_extension == ".RAR" || items.file_extension == ".zip" || items.file_extension == ".ZIP") {
                            items.icon = '/Content/assets/images/icon/rar.png';
                        } else if (items.file_extension == ".jpeg" || items.file_extension == ".jpg" || items.file_extension == ".png" || items.file_extension == ".tif" || items.file_extension == ".gif" || items.file_extension == ".JPEG" || items.file_extension == ".JPG" || items.file_extension == ".PNG" || items.file_extension == ".TIF" || items.file_extension == ".GIF") {
                            items.icon = '/Content/assets/images/icon/jpg.png';
                        }
                    });

                    $scope.DonThu = data;
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        var getKetLuanbyIdDonThu = function (id) {
            crudService.get("/KetLuanGiaiQuyet/GetInfoViewByID?id=", id)
                .success(function (data) {
                    console.log(data)
                    data.NgayTao = parseDateV(data.NgayTao);
                    data.NgayBanHanh = parseDateV(data.NgayBanHanh);

                    $scope.KetLuan = data;
                    $scope.lstDataForPlugin = data.FileUpload;
                }).error(function (error) {
                  //  $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

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
        $scope.preCreateKetLuan = function (data) {
            if (data == null) {
                $("#txtNgayBanHanh").notify("Chọn ngày ban hành kết luận", "error");
                return;
            }

            if (data.NgayBanHanh == null || data.NgayBanHanh == "") {
                $("#txtNgayBanHanh").notify("Chọn ngày ban hành kết luận", "error");
                $("#txtNgayBanHanh").focus();
                return false;
            }

            if (data.IdLoaiKetQua == null || data.IdLoaiKetQua == -1) {
                $("#ddlLoaiKetQua").notify("Chọn loại kết quả", "error");
                $("#ddlLoaiKetQua").focus();
                return false;
            }

            if (data.NoiDung == null || data.NoiDung == "") {
                $("#txtNoiDung").notify("Nhập nội dung xử lý", "error");
                $("#txtNoiDung").focus();
                return false;
            }

            if (data.Id == null || data.Id == "") {
                createKetLuan(data);
                $("#myModal").modal("hide");
            }
            else {
                updateKetLuan(data);
                $("#myModal").modal("hide");
            }
        }

        // Hàm thực hiện create kết luận
        var createKetLuan = function (data) {
            if (data.NgayBanHanh != null && data.NgayBanHanh != "") {
                dateSplit = data.NgayBanHanh.split('/');
                data.NgayBanHanh = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }
          

            crudService.create("/KetLuanGiaiQuyet/Create", data)
                .success(function (data) {
                    $.notify("Cập nhật kết luận giải quyết thành công !", "success");

                    //Upload file sau khi tạo kết luận xong
                    if ($scope.fileList.length != 0) {
                        for (var i = 0; i < $scope.fileList.length; i++) {
                            console.log("Đang tải lên tệp:", $scope.fileList[i]);
                            fileUploadService.UploadedFile($scope.fileList[i].file, data.Id, $scope.FileType, data.IdDonThuGoc).then(function (data) {
                                $scope.lstDataForPlugin.push(data);
                            });
                        }
                        $scope.fileList = [];
                    } else {
                        console.log("Danh sách fileList rỗng, không có tệp nào để tải lên.");
                    }

                    //Cập nhật trạng thái cho hồ sơ
                    $http({
                        url: "/DonThu/UpdateTrangThai",
                        method: "GET",
                        params: {
                            Id: data.IdDonThu,
                            IdTrangThai: 6 //trạng thái phê duyệt kết quả giải quyết
                        }
                        }).success(function () {
                            createXuLyDonThu(data);
                            $.notify("Cập nhật thành công !", "success");
                            clearData();
                            init();
                        })
                    .error(function () {
                        $.notify("Cập nhật thất bại !", "error");
                    });
                })
                .error(function (error) {
                    $.notify("Tạo mới thất bại!", "error");
                });
        }        

        // Hàm thực hiện create kết luận
        var updateKetLuan = function (data) {
            if (data.NgayBanHanh != null && data.NgayBanHanh != "") {
                dateSplit = data.NgayBanHanh.split('/');
                data.NgayBanHanh = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            crudService.create("/KetLuanGiaiQuyet/Update", data)
                .success(function (data) {
                    $.notify("Cập nhật kết luận giải quyết thành công !", "success");

                    //Upload file sau khi tạo kết luận xong
                    if ($scope.fileList.length != 0) {
                        for (var i = 0; i < $scope.fileList.length; i++) {
                            fileUploadService.UploadedFile($scope.fileList[i].file, data.Id, $scope.FileType, data.IdDonThuGoc).then(function (data) {
                                $scope.lstDataForPlugin.push(data);
                            });
                        }
                        $scope.fileList = [];
                    }

                    //Cập nhật trạng thái cho hồ sơ
                    $http({
                        url: "/DonThu/UpdateTrangThai",
                        method: "GET",
                        params: {
                            Id: data.IdDonThu,
                            IdTrangThai: 6 //trạng thái phê duyệt kết quả giải quyết

                        }
                    }).success(function () {
                        createXuLyDonThu(data);
                        $.notify("Cập nhật thành công !", "success");
                        clearData();
                        init();
                    })
                    .error(function () {
                        $.notify("Cập nhật thất bại !", "error");
                    });
                })
                .error(function (error) {
                    $.notify("Tạo mới thất bại!", "error");
                });
        }

        // Hàm thực hiện create xử lý hồ sơ
        var createXuLyDonThu = function (data) {
            $scope.XuLyDonThu = {};
            $scope.XuLyDonThu.IdDonThu = data.IdDonThu;
            $scope.XuLyDonThu.IdTrangThai = 5; // Id trạng thái kết luận giải quyết
            $scope.XuLyDonThu.IdDonVi = $cookieStore.get("DeparmentId");
            $scope.XuLyDonThu.IdNguoiTao = $cookieStore.get("UserID");
            $scope.XuLyDonThu.NoiDung = "";
            $scope.XuLyDonThu.VuViecId = data.VuViecId;
            $scope.XuLyDonThu.IdDonThuGoc = data.IdDonThuGoc;
            crudService.create("/XuLyDonThu/Create", $scope.XuLyDonThu)
                 .success(function (result) {

                 })
                .error(function () {
                });
        }

        //get data doc id
        var getFileByIdKetLuan = function (id) {
            GetAllFileByObjectId(id, $scope.FileType)
                .success(function (data) {
                    $scope.lstDataForPlugin = data;
                });
            console.log("GetAllFileByObjectId", data)
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

        $scope.FileView = {};
        $scope.viewFileByDocId = function (fileExtension, fileUrl, fileOriginal) {
            $scope.content = "";
            $scope.FileView.file_original = fileOriginal;
            var port = $location.port();

            if (port != '') {
                $scope.content = $location.protocol() + "://" + $location.host() + ':' + $location.port() + fileUrl;
            }
            else {
                $scope.content = $location.protocol() + "://" + $location.host() + fileUrl;
            }

            if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".tif" || fileExtension == ".gif") {
                document.getElementById('IframeViewFile').src = $scope.content;
            }
            else {
                document.getElementById('IframeViewFile').src = " http://docs.google.com/gview?url=" + $scope.content + "&embedded=true";
            }
        }

        //lấy danh sách file theo IdDonThuGoc thì sẽ lấy được đơn của đơn vị gốc và đơn của đơn vị mới(trong trường hợp chuyển đơn)

        

        // upload file
        $scope.fileList = [];

        $scope.ImageProperty = {
            file: ''
        }
        $scope.FileType = 3;
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
            console.log("Danh sách fileList sau khi chọn tệp:", $scope.fileList);
        }
        $scope.removeFile = function (delitemfile) {
            $scope.fileList.splice($scope.fileList.indexOf(delitemfile), 1);
        }

        $scope.deleteFile = function (id) {
            crudService.remove("/Files/DeleteFiles", id)
            .success(function (result) {
                $.notify("Xóa file thành công!", "success");
                getFileByIdKetLuan($scope.KetLuan.Id);
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


