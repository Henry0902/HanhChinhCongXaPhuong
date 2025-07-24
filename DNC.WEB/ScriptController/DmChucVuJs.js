
myApp.controller("DmChucVuJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số   
        $scope.thongtintimkiem = '';
        $scope.TrangThai = 0;
        $scope.lstChucVu = [];
        $scope.ChucVu = {};
        $scope.today = new Date();
        $scope.lstPage = [];
        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------
        var lblmaChucVu = document.getElementById("lblmaChucVu");
        var lbltenChucVu = document.getElementById("lbltenChucVu");
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
                url: "/DmChucVu/GetIndex",
                method: "GET",
                params: { thongtintimkiem: $scope.thongtintimkiem, TrangThai: $scope.TrangThai, order: $scope.Order, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex }
            });
        }

        // Init data
        var init = function () {
            getData()
            .success(function (data) {
                angular.forEach(data, function (item) {
                    item.NgayTao = parseDate(item.NgayTao);
                });
                $scope.lstChucVu = data;
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
                        $scope.lstChucVu = data;
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

        // Hiển thị thông tin modal Insert Update
        $scope.show = function (id) {
            $scope.ChucVu = {};
            $('#lblmaChucVu').hide();
            $('#lbltenChucVu').hide();
            if (id != "") {
                $scope.currentId = id;
                $("#crud").val("update");
                $scope.getbyID(id);
            }
            else {
                $("#crud").val("create");
                $scope.ChucVu = null;
            }
        }

        // Get Data By ID
        $scope.getbyID = function (id) {
            crudService.get("/DmChucVu/GetById?id=", id)
                .success(function (data) {
                    data.NgayTao = parseDate(data.NgayTao);
                    //$scope.ChucVu_id = data.ChucVu_id;
                    $scope.ChucVu = data;

                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        // Thực hiện lưu trên modal Insert Update
        $scope.preCreate = function (data) {
            debugger
            if (data == null) {
                $("#lblmaChucVu").show();
                lblmaChucVu.textContent = "Mã dự án không để trống";
                $("#lbltenChucVu").show();
                lbltenChucVu.textContent = "Tên dự án không để trống";
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
            //$('#lblmaChucVu').hide();
            //$('#lbltenChucVu').hide();
            //if (data.ChucVu_ma == null || data.ChucVu_ma == "") {
            //    $("#lblmaChucVu").show();
            //    $("#lblmaChucVu").textContent = "Mã dự án không để trống!";
            //    return false;
            //}
            //if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.ChucVu_ma) == true) {
            //    $("#lblmaChucVu").show();
            //    $("#lblmaChucVu").textContent = "Mã dự án không chứa ký tự đặc biệt!";
            //    return false;
            //}
            //if (data.ChucVu_ten == null || data.ChucVu_ten == "") {
            //    $("#lbltenChucVu").show();
            //    $("#lbltenChucVu").textContent = "Tên dự án không để trống!";
            //    return false;
            //}
            //if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.ChucVu_ten) == true) {
            //    $("#lbltenChucVu").show();
            //    $("#lbltenChucVu").textContent = "Tên dự án không chứa ký tự đặc biệt!";
            //    return false;
            //}
            return true;
        }

        // Hàm thực hiện Create
        $scope.create = function (data) {
            debugger
            crudService.create("/DmChucVu/Create", data)
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
            crudService.update("/DmChucVu/Update", data)
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
                url: "/DmChucVu/updateStatus",
                method: "GET",
                params: {
                    Id: Id,
                    Status: Status
                }
            });
        }
        $scope.remove = function (id) {
            crudService.remove("/DmChucVu/Delete", id)
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

