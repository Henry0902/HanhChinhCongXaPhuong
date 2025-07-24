
myApp.controller("DonThuDuyetGiaiQuyetJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore) {
        // Tham số  
        
        $scope.namID = -1;
        $scope.NguonDonId = -1;
        $scope.loaihsID = -1;
        $scope.TuNgay = '';
        $scope.DenNgay = '';
        $scope.Keyword = '';
        $scope.DeparmentId = $cookieStore.get("DeparmentId");
        

        $scope.lstNam = [];
        $scope.lstDonThu = [];

        $scope.lstXuLyDonThu = [];
        $scope.IdTrangThai = 6;
        $scope.YKienDuyet = 0;

        $scope.DonThu = {};
        $scope.XuLyDonThu = {};
        $scope.KetLuan = {};
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

        //Lấy danh sách năm
        var getListNam = function () {
            var currentTime = new Date();
            var nam = currentTime.getFullYear();
            $scope.namID = nam;
            for (i = nam; i > nam - 5; i--) {
                $scope.lstNam.push(i);
            }
        }
        getListNam();

        // lấy danh sách nguồn đơn
        $scope.lstNguonDon = [];
        var getlstNguonDon = function () {
            crudService.getAll("/DmNguonDon/GetAll")
                .success(function (data) {
                    $scope.lstNguonDon = data;
                });
        }
        getlstNguonDon();

        // lấy danh sách hướng xử lý
        $scope.lstHuongXuLy = [];
        var getlstHuongXuLy = function () {
            crudService.getAll("/DmHuongXuLy/getAll")
                .success(function (data) {
                    $scope.lstHuongXuLy = data;
                });
        }
        getlstHuongXuLy();

        $scope.lstDepartments = [];
        //lấy danh phòng ban
        var getAllDepartment = function () {
            crudService.getAll("/Departments/getAllDropDown")
                .success(function (data) {
                    $scope.lstDepartments = data;
                });
        }
        getAllDepartment();

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
        //    getKetLuanbyIdDonThu(id);
        //}

        // Hiển thị thông tin modal Insert Update
        $scope.show = function (item) {
            $scope.DonThu = {};
            getbyID(item.Id);
            getXuLyDonThubyIdDonThu(item.IdDonThuGoc);
            getKetLuanbyIdDonThu(item.Id);
        }

        // Hiển thị thông tin modal Insert Update
        $scope.ShowPheDuyet = function (id) {
            $scope.DonThu = {};            
            getbyID(id);            
            $scope.fileList = [];
            $scope.NoiDung = $("#ddlYKienDuyet option:selected").text();
        }

        $scope.ChangeYKienDuyet = function () {
            $scope.NoiDung = $("#ddlYKienDuyet option:selected").text();
        }

        $scope.HideHuongXuLy = function (id) {
            $scope.YKienDuyet = 0;
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
                    data.NgayTao = parseDateV(data.NgayTao);
                    data.NgayBanHanh = parseDateV(data.NgayBanHanh);

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

                    $scope.KetLuan = data;
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
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
        $scope.preCreateDuyetGiaiQuyet= function (data) {
            if ($scope.YKienDuyet == 0) { // Đồng ý duyệt
                if (data == null) {
                    $("#ddlHuongXuLy").notify("Chọn hướng xử lý", "error");
                    return;
                }

                if ($scope.NoiDung == null || $scope.NoiDung == "") {
                    $("#txtNoiDung").notify("Nhập nội dung xử lý", "error");
                    $("#txtNoiDung").focus();
                    return false;
                }
                data.IdTrangThai = 7;
            }
            else  { // Không đồng ý duyệt trả lại 
                data.IdTrangThai = 5;
            }

            createDuyetGiaiQuyet(data);
            $("#myModal").modal("hide");
        }

        // Hàm thực hiện create HuongXuLy
        var createDuyetGiaiQuyet = function (data) {
            $http({
                url: "/DonThu/UpdateTrangThai",
                method: "GET",
                params: {
                    Id: data.Id,
                    IdTrangThai: data.IdTrangThai
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
            $("#myModal").modal("hide");
        }

        var createXuLyDonThu = function (data) {
            $scope.XuLyDonThu = {};
            $scope.XuLyDonThu.IdDonThu = data.Id;
            $scope.XuLyDonThu.IdTrangThai = 6; // Id trạng thái phê duyệt kết quả giải quyết
            $scope.XuLyDonThu.IdDonVi = $cookieStore.get("DeparmentId");
            $scope.XuLyDonThu.IdNguoiTao = $cookieStore.get("UserID");
            $scope.XuLyDonThu.ThaoTac = $("#ddlHuongXuLy option:selected").text();
            $scope.XuLyDonThu.NoiDung = $scope.NoiDung;
            $scope.XuLyDonThu.VuViecId = data.VuViecId;
            $scope.XuLyDonThu.IdDonThuGoc = data.IdDonThuGoc;
            crudService.create("/XuLyDonThu/Create", $scope.XuLyDonThu)
                .success(function (result) {
                    //Upload file sau khi tạo bước xử lý xong
                    if ($scope.fileList.length != 0) {
                        var count = 0;
                        for (var i = 0; i < $scope.fileList.length; i++) {
                            fileUploadService.UploadedFile($scope.fileList[i].file, result.Id, $scope.FileType, result.IdDonThuGoc);
                            count++;
                        }
                        if (count == $scope.fileList.length) {
                            $scope.fileList = [];
                        }
                    }
                })
                .error(function () {
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

        // upload file
        $scope.fileList = [];

        $scope.ImageProperty = {
            file: ''
        }
        $scope.FileType = 2;
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
    }).factory('fileUploadService', function ($http, $q) {
        var fac = {};
        fac.UploadedFile = function (file, docId, type,idDonThuGoc) {
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


