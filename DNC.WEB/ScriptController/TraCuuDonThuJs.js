
myApp.controller("TraCuuDonThuJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số  

        $scope.lstDonThu = [];

        $scope.KeyWord = '';
        $scope.NoiDung = '';
        $scope.IdNguonDon = -1;
        $scope.IdDoiTuong = -1;
        $scope.IdTinhThanh = -1;
        $scope.IdQuanHuyen = -1;
        $scope.IdPhuongXa = -1;
        $scope.IdLoaiDonThu = -1;
        $scope.IdLoaiDonThu = -1;
        $scope.IdLoaiKNTC = -1;
        $scope.IdLoaiKNTCChiTiet = -1;
        $scope.IdDonThuXuLy = -1;
        $scope.IdTrangThai = -1;
        $scope.IdHuongXuLy = -1;
        $scope.TuNgay = '';
        $scope.DenNgay = '';
        $scope.IdDonVi = $cookieStore.get("DeparmentId");
        //  $scope.IdDonVi = -1;
       

        $scope.DonThu = {};
        $scope.lstXuLyDonThu = [];

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

        var date = new Date();
        $scope.TuNgay = '01/' + '01' + '/' + date.getFullYear();
        $scope.DenNgay = '31/' + '12' + '/' + date.getFullYear();

        $scope.lstDepartments = [];

        //lấy danh phòng ban
        var getAllDepartment = function () {
            //crudService.getAll("/Departments/getAllDropDown_Search")
            //    .success(function (data) {
            //        $scope.lstDepartments = data;
            //    });

            return $http({
                url: "/Departments/getAllDropDown_ByParentId/",
                method: "GET",
                params: {
                    parentId: $cookieStore.get("DeparmentId")
                }
            }).success(function (data) {
                $scope.lstDepartments = data;
            });
        }
        getAllDepartment();

        // lấy danh sách hướng xử lý
        $scope.lstHuongXuLy = [];
        var getlstHuongXuLy = function () {
            crudService.getAll("/Constants/GetDmHuongXuLy")
                .success(function (data) {
                    $scope.lstHuongXuLy = data;
                });
        }
        getlstHuongXuLy();

        // lấy danh sách nguồn đơn
        $scope.lstNguonDon = [];
        var getlstNguonDon = function () {
            crudService.getAll("/Constants/GetDmNguonDon")
                .success(function (data) {
                    $scope.lstNguonDon = data;
                });
        }
        getlstNguonDon();

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
                    $scope.IdLoaiKNTC = -1;
                    $scope.IdLoaiKNTCChiTiet = -1;
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
                    $scope.IdLoaiKNTCChiTiet = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC chi tiết bị lỗi.", "error");
                });
        }

        // Lấy danh sách loại hồ sơ
        $scope.lstTrangThai = [];
        var getlstTrangThai = function () {
            crudService.getAll("/Constants/GetDmTrangThai")
                .success(function (data) {
                    $scope.lstTrangThai = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách trạng thái bị lỗi.", "error");
                });;
        }

        //Load danh sách data
        var getData = function () {
            return $http({
                url: "/DonThu/SearchAll",
                method: "GET",
                params: {
                    IdDonVi: $scope.IdDonVi, IdNguonDon: $scope.IdNguonDon, IdDoiTuong: $scope.IdDoiTuong, KeyWord: $scope.KeyWord,
                    IdTinhThanh: $scope.IdTinhThanh, IdQuanHuyen: $scope.IdQuanHuyen, IdPhuongXa: $scope.IdPhuongXa,
                    IdLoaiDonThu: $scope.IdLoaiDonThu, IdLoaiKNTC: $scope.IdLoaiKNTC, IdLoaiKNTCChiTiet: $scope.IdLoaiKNTCChiTiet,
                    IdDonThuXuLy: $scope.IdDonThuXuLy, IdTrangThai: $scope.IdTrangThai, IdHuongXuLy: $scope.IdHuongXuLy, NoiDung: $scope.NoiDung,
                    TuNgay: $scope.TuNgay, DenNgay: $scope.DenNgay,
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
        getlstNguonDon();
        getlstHuongXuLy();
        getlstLoaiDoiTuong();
        getlstTinhThanh();
        getlstLoaiDonThu();
        getlstTrangThai();

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
        $scope.ShowHuongXuLy = function (id) {
            $scope.DonThu = {};            
            getbyID(id);
            $scope.HuongXuLy = {}
            $scope.HuongXuLy.Id = id;
            $scope.HuongXuLy.IdHuongXuLy = -1;
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
                    $scope.KetLuan = {};
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


        $scope.Export_SoTiepNhanDonThu = function () {
            return $http({
                url: "/BaoCao/BC_SoTiepNhanDonThu",
                method: "GET",
                params: {
                    IdDonVi: $scope.IdDonVi, IdNguonDon: $scope.IdNguonDon, IdDoiTuong: $scope.IdDoiTuong, KeyWord: $scope.KeyWord,
                    IdTinhThanh: $scope.IdTinhThanh, IdQuanHuyen: $scope.IdQuanHuyen, IdPhuongXa: $scope.IdPhuongXa,
                    IdLoaiDonThu: $scope.IdLoaiDonThu, IdLoaiKNTC: $scope.IdLoaiKNTC, IdLoaiKNTCChiTiet: $scope.IdLoaiKNTCChiTiet,
                    IdDonThuXuLy: $scope.IdDonThuXuLy, IdTrangThai: $scope.IdTrangThai, IdHuongXuLy: $scope.IdHuongXuLy, NoiDung: $scope.NoiDung,
                    TuNgay: $scope.TuNgay, DenNgay: $scope.DenNgay,order: $scope.Order
                }
            }).success(function (data) {
                document.getElementById('BC_SoTiepNhanDonThu').src = data;
                $.notify("Xuất báo cáo thành công!", "success");
            }).error(function () {
                $.notify("Xuất báo cáo thất bại!", "error");
            });
        }
    });


