
myApp.controller("DmLoaiKNTCCTJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số   
        $scope.thongtintimkiem = '';
        $scope.TrangThai = 0;
        $scope.lstLoaiKNTCCT = [];
        $scope.LoaiKNTCCT = {};
        $scope.today = new Date();
        $scope.lstPage = [];
        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.idTinhThanh = -1;
        $scope.idTinh = -1;
        $scope.idTinh2 = -1;
        $scope.TrangThai = -1
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------
        var lblmaLoaiKNTCCT = document.getElementById("lblmaLoaiKNTCCT");
        var lbltenLoaiKNTCCT = document.getElementById("lbltenLoaiKNTCCT");
        //-------------------------------------------------------------------------------

        // Convert datetime
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        //Load danh sách data
        var getData = function () {
            return $http({
                url: "/DmLoaiKNTCCT/GetIndex",
                method: "GET",
                params: {
                    thongtintimkiem: $scope.thongtintimkiem,
                    TrangThai: $scope.TrangThai,
                    order: $scope.Order,
                    pageSize: $scope.pageSize,
                    pageIndex: $scope.pageIndex,
                    idTinhThanh: $scope.idTinhThanh,
                    idTinh: $scope.idTinh
                }
            });
        }
        // lấy danh sách tỉnh thành
        $scope.lstTinhThanh = [];
        $scope.lstTinhThanh2 = [];
        var getAllDropdown = function () {
            crudService.getAll("/DmLoaiDonThu/getAllDropDown")
                .success(function (data) {
                    $scope.lstTinhThanh2 = data.listLoaiKNTC;
                    $scope.lstTinh = data.listLoaiDonThu;
                });


        }
        getAllDropdown();

        var getQuanHuyen = function () {
            return $http({
                url: "/DmLoaiKNTCCT/getQuanHuyen",
                method: "GET",
                params: {
                    id: $scope.idTinh,
                }
            });
        }
        var getQuanHuyen2 = function () {
            return $http({
                url: "/DmLoaiKNTCCT/getQuanHuyen",
                method: "GET",
                params: {
                    id: $scope.idTinh2,
                }
            });
        }
        // Init data
        var init = function () {
            getQuanHuyen()
                .success(function (data) {
                    $scope.lstTinhThanh = data.listLoaiKNTCById;
                })
                .error(function (error) {
                    console.log(error);
                });
            getData()
            .success(function (data) {
                angular.forEach(data, function (item) {
                    item.NgayTao = parseDate(item.NgayTao);
                });
                $scope.lstLoaiKNTCCT = data;
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
                            item.NgayTao = parseDate(item.NgayTao);
                        });
                        $scope.lstLoaiKNTCCT = data;
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
            $scope.lstPage.splice(0);
            $scope.lstPage = [];
            init();
        }
        $scope.reload1 = function () {
            if ($scope.idTinh == -1) {
                $scope.idTinhThanh = -1;
            }
            $scope.pageIndex = 1;
            $scope.lstPage.splice(0);
            $scope.lstPage = [];
            init();
        }
        $scope.reload2 = function () {
            getQuanHuyen2()
                .success(function (data) {
                    $scope.lstTinhThanh2 = data.listLoaiKNTCById;
                    $scope.idQuanHuyen2 = -1
                })
                .error(function (error) {
                    console.log(error);
                });
        }
        // Hiển thị thông tin modal Insert Update
        $scope.show = function (id) {
            $scope.LoaiKNTCCT = {};
            $('#lblmaLoaiKNTCCT').hide();
            $('#lbltenLoaiKNTCCT').hide();
            if (id != "") {
                $scope.currentId = id;
                $("#crud").val("update");
                $scope.idTinh2 = -1;
                $scope.reload2();
                $scope.getbyID(id);
            }
            else {
                $("#crud").val("create");
                $scope.idTinh2 = -1;
                $scope.idQuanHuyen2 = -1;
            }
        }

        // Get Data By ID
        $scope.getbyID = function (id) {
            crudService.get("/DmLoaiKNTCCT/GetById?id=", id)
                .success(function (data) {
                    data.NgayTao = parseDate(data.NgayTao);
                    $scope.idTinh2 = data.idTinh2;
                    $scope.LoaiKNTCCT = data;
                    $scope.idQuanHuyen2 = data.IdLoaiKNTC;

                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        // Thực hiện lưu trên modal Insert Update
        $scope.preCreate = function (data) {
            debugger
            data.IdLoaiKNTC = $scope.idQuanHuyen2;
            if (data == null) {
                $("#lblmaLoaiKNTCCT").show();
                lblmaLoaiKNTCCT.textContent = "Mã dự án không để trống";
                $("#lbltenLoaiKNTCCT").show();
                lbltenLoaiKNTCCT.textContent = "Tên dự án không để trống";
                return;
            }

            if ($("#crud").val() == "create") {
                if (validate(data) == false) {
                    return;
                }
                $scope.create(data);
                $("#myModal").modal("hide");
            }
            else if ($("#crud").val() == "update") {
                if (validate(data) == false) {
                    return;
                }
                $scope.update(data);
                $("#myModal").modal("hide");
            }
        }

        var validate = function (data) {
            //$('#lblmaLoaiKNTCCT').hide();
            //$('#lbltenLoaiKNTCCT').hide();
            if (data.IdLoaiKNTC == null || data.IdLoaiKNTC == -1 || data.IdLoaiKNTC == "") {
                $("#lblmaLoaiKNTCCT").show();
               // $("#lblmaLoaiKNTCCT").textContent = "Mã dự án không để trống!";
                return false;
            }
            //if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.LoaiKNTCCT_ma) == true) {
            //    $("#lblmaLoaiKNTCCT").show();
            //    $("#lblmaLoaiKNTCCT").textContent = "Mã dự án không chứa ký tự đặc biệt!";
            //    return false;
            //}
            //if (data.LoaiKNTCCT_ten == null || data.LoaiKNTCCT_ten == "") {
            //    $("#lbltenLoaiKNTCCT").show();
            //    $("#lbltenLoaiKNTCCT").textContent = "Tên dự án không để trống!";
            //    return false;
            //}
            //if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.LoaiKNTCCT_ten) == true) {
            //    $("#lbltenLoaiKNTCCT").show();
            //    $("#lbltenLoaiKNTCCT").textContent = "Tên dự án không chứa ký tự đặc biệt!";
            //    return false;
            //}
            return true;
        }

        // Hàm thực hiện Create
        $scope.create = function (data) {
            debugger
            crudService.create("/DmLoaiKNTCCT/Create", data)
                .success(function (data) {
                    $.notify("Tạo mới thành công!", "success");
                    clearData();
                    init();
                })
                .error(function (error) {
                    $.notify("Tạo mới thất bại!", "error");
                });
        }

        // Hàm thực hiện Update 
        $scope.update = function (data) {
            crudService.update("/DmLoaiKNTCCT/Update", data)
                .success(function () {
                    $.notify("Cập nhật thành công!", "success");
                    clearData();
                    init();
                })
                .error(function () {
                    $.notify("Cập nhật thất bại!", "error");
                });
        }

        // Hàm thực hiện Update Status
        $scope.UpdateStatus = function (data) {
            var trangThai = 0;
            if (data.TrangThai == 0) {
                trangThai = 1;
            } 
            UpdateIsLocked(data.id, trangThai)
                .success(function () {
                    $.notify("Cập nhật thành công!", "success");
                    clearData();
                    init();
                })
                .error(function () {
                    $.notify("Cập nhật thất bại!", "error");
                });
        }

        var UpdateIsLocked = function (Id, Status) {
            return $http({
                url: "/DmLoaiKNTCCT/updateStatus",
                method: "GET",
                params: {
                    Id: Id,
                    Status: Status
                }
            });
        }
        $scope.remove = function (id) {
            crudService.remove("/DmLoaiKNTCCT/Delete", id)
                .success(function () {
                    $("#confirm-delete").modal("hide");
                    $.notify("Xóa thành công!", "success");
                    $scope.reload();
                })
                .error(function (error) {
                    console.log(error);
                });
        }
        $scope.checkremove = function (id, tennhanvien) {
            $scope.ID = id;
            $scope.TenNhanVien = tennhanvien;
        }
    });

