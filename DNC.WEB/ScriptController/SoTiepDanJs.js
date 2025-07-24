
myApp.controller("SoTiepDanJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số   
        $scope.KieuTiepDan = -1;
        $scope.IdPhienTCD = -1;
        $scope.IdDoiTuong = -1;
        $scope.KeyWord = '';
        $scope.IdTinhThanh = -1;
        $scope.IdQuanHuyen = -1;
        $scope.IdPhuongXa = -1;
        $scope.IdLoaiVu = -1;
        $scope.IdLoaiVuKNTC = -1;
        $scope.IdLoaiVuKNTCChiTiet = -1;
        $scope.NoiDung = '';
        $scope.TuNgay = '';
        $scope.DenNgay = '';

        $scope.lstNam = [];
        $scope.lstTiepDan = [];
        $scope.lstXuLyDonThu = [];
        $scope.IdDonVi = $cookieStore.get("DeparmentId");

        $scope.TiepDan = {};
        $scope.today = new Date();
        $scope.lstPage = [];
        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------        

        var date = new Date();
        $scope.TuNgay = '01/' + '01' + '/' + date.getFullYear();
        $scope.DenNgay = '31/' + '12' + '/' + date.getFullYear();

        // Convert datetime
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        // lấy danh sách phiên tiếp công dân
        $scope.lstPhienTCD = [];
        var getlstPhienTCD = function () {
            crudService.getAll("/Constants/GetDmPhienTiepCongDan")
                .success(function (data) {
                    $scope.lstPhienTCD = data;
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
        var getlstTinhThanh = function () {
            crudService.getAll("/DmTinhThanh/getAllDropDown")
                .success(function (data) {
                    $scope.lstTinhThanh = data.listTinhThanh;
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
                    $scope.lstPhuongXa = [];
                    $scope.IdQuanHuyen = -1;
                    $scope.IdPhuongXa = -1;
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
                    $scope.IdPhuongXa = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
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

        // Lấy danh sách loại KNTC
        $scope.lstLoaiKNTC = [];
        $scope.getlstLoaiKNTC = function (id) {
            crudService.get("/DmLoaiKNTC/GetLoaiKNTCByLoaiDonThu/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTC = data;
                    $scope.lstLoaiKNTCChiTiet = [];
                    $scope.IdLoaiVuKNTC = '-1';
                    $scope.IdLoaiVuKNTCChiTiet = '-1';
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
                    $scope.IdLoaiVuKNTCChiTiet = '-1';
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC chi tiết bị lỗi.", "error");
                });
        }

        //Load danh sách data
        var getData = function () {
            return $http({
                url: "/TiepCongDan/GetIndex",
                method: "GET",
                params: {
                    IdDonVi: $scope.IdDonVi, KieuTiepDan: $scope.KieuTiepDan, IdPhienTCD: $scope.IdPhienTCD, IdDoiTuong: $scope.IdDoiTuong, KeyWord: $scope.KeyWord,
                    IdTinhThanh: $scope.IdTinhThanh, IdQuanHuyen: $scope.IdQuanHuyen, IdPhuongXa: $scope.IdPhuongXa,
                    IdLoaiVu: $scope.IdLoaiVu, IdLoaiVuKNTC: $scope.IdLoaiVuKNTC, IdLoaiVuKNTCChiTiet: $scope.IdLoaiVuKNTCChiTiet,
                    NoiDung: $scope.NoiDung,TuNgay: $scope.TuNgay, DenNgay: $scope.DenNgay,
                    order: $scope.Order, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex
                }
            });
        }

        // Init data
        var init = function () {
            $("#divLoading").show();
            getData()
            .success(function (data) {
                angular.forEach(data, function (item) {
                    item.NgayTiep = parseDate(item.NgayTiep);
                });
                $scope.lstTiepDan = data;
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
        getlstPhienTCD();
        getlstLoaiDoiTuong();
        getlstTinhThanh();
        getlstLoaiDonThu();

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
                        item.NgayTiep = parseDate(item.NgayTiep);
                    });
                    $scope.lstTiepDan = data;
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

        // Hiển thị thông tin modal Insert Update
        $scope.show = function (id) {
            $scope.TiepDan = {};
            getbyID(id);
            $('#TabThongTin').addClass("active");
            $('#ThongTinTiepDan').addClass("active in");
            $('#TabXuLy').removeClass("active");
            $('#XuLyDonThu').removeClass("active in");
        }

        // Get Data By ID
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

        // Get Data By ID
        var getbyID = function (id) {
            crudService.get("/TiepCongDan/GetInfoViewByID?id=", id)
                .success(function (data) {
                    data.NgayTiep = parseDateV(data.NgayTiep);
                    data.NgayCap = parseDateV(data.NgayCap);
                    data.NgayTao = parseDateV(data.NgayTao);

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
                    $scope.TiepDan = data;

                    if (data.IdDonThu != null && data.IdDonThu != "") {
                        getXuLyDonThubyIdDonThu(data.IdDonThu);
                    }

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

        // Hiển thị thông tin modal Insert Update
        $scope.update = function (id) {
            crudService.setCookie('IdTiepCongDan', id);
            window.location.href = '../TiepCongDan/TiepCongDan';
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

        $scope.Export_SoTiepCongDan = function () {
            return $http({
                url: "/BaoCao/BC_SoTiepCongDan",
                method: "GET",
                params: {
                    IdDonVi: $scope.IdDonVi, KieuTiepDan: $scope.KieuTiepDan, IdPhienTCD: $scope.IdPhienTCD, IdDoiTuong: $scope.IdDoiTuong, KeyWord: $scope.KeyWord,
                    IdTinhThanh: $scope.IdTinhThanh, IdQuanHuyen: $scope.IdQuanHuyen, IdPhuongXa: $scope.IdPhuongXa,
                    IdLoaiVu: $scope.IdLoaiVu, IdLoaiVuKNTC: $scope.IdLoaiVuKNTC, IdLoaiVuKNTCChiTiet: $scope.IdLoaiVuKNTCChiTiet,
                    NoiDung: $scope.NoiDung, TuNgay: $scope.TuNgay, DenNgay: $scope.DenNgay, order: $scope.Order
                }
            }).success(function (data) {
                console.log(data);
                document.getElementById('BC_SoTiepCongDan').src = data;
                $.notify("Xuất báo cáo thành công!", "success");
            }).error(function () {
                $.notify("Xuất báo cáo thất bại!", "error");
            });
        }
    });


