
myApp.controller("DuAnJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số   
        $scope.thongtintimkiem = '';
        $scope.trang_thai = 0;
        $scope.lstDuAn = [];
        $scope.DuAn = {};
        $scope.today = new Date();
        $scope.lstPage = [];
        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------
        var lblmaduan = document.getElementById("lblmaduan");
        var lbltenduan = document.getElementById("lbltenduan");
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
                url: "/DuAn/GetIndex",
                method: "GET",
                params: { thongtintimkiem: $scope.thongtintimkiem, trang_thai: $scope.trang_thai, order: $scope.Order, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex }
            });
        }

        // Init data
        var init = function () {
            getData()
            .success(function (data) {
                angular.forEach(data, function (item) {
                    item.ngay_tao = parseDate(item.ngay_tao);
                });
                $scope.lstDuAn = data;
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
                        item.ngay_tao = parseDate(item.ngay_tao);
                    });
                    $scope.lstDonVi = data;
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
            $scope.DuAn = {};
            $('#lblmaduan').hide();
            $('#lbltenduan').hide();
            if (id != "") {
                $scope.currentId = id;
                $("#crud").val("update");
                $scope.getbyID(id);
            }
            else {
                $("#crud").val("create");
                $scope.DuAn = null;
            }
        }

        // Get Data By ID
        $scope.getbyID = function (id) {
            crudService.get("/DuAn/GetById?id=", id)
                .success(function (data) {
                    data.ngay_tao = parseDate(data.ngay_tao);
                    $scope.duan_id = data.duan_id;
                    $scope.DuAn = data;

                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        // Thực hiện lưu trên modal Insert Update
        $scope.preCreate = function (data) {
            if (data == null) {
                $("#lblmaduan").show();
                lblmaduan.textContent = "Mã dự án không để trống";
                $("#lbltenduan").show();
                lbltenduan.textContent = "Tên dự án không để trống";
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
            $('#lblmaduan').hide();
            $('#lbltenduan').hide();
            if (data.duan_ma == null || data.duan_ma == "") {
                $("#lblmaduan").show();
                $("#lblmaduan").textContent = "Mã dự án không để trống!";
                return false;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.duan_ma) == true) {
                $("#lblmaduan").show();
                $("#lblmaduan").textContent = "Mã dự án không chứa ký tự đặc biệt!";
                return false;
            }
            if (data.duan_ten == null || data.duan_ten == "") {
                $("#lbltenduan").show();
                $("#lbltenduan").textContent = "Tên dự án không để trống!";
                return false;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.duan_ten) == true) {
                $("#lbltenduan").show();
                $("#lbltenduan").textContent = "Tên dự án không chứa ký tự đặc biệt!";
                return false;
            }
            return true;
        }

        // Hàm thực hiện Create
        $scope.create = function (data) {
            crudService.create("/DuAn/Create", data)
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
            crudService.update("/DuAn/Update", data)
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
            if (data.trang_thai == 0) {
                trangThai = 1;
            } 
            UpdateIsLocked(data.duan_id, trangThai)
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
                url: "/DuAn/updateStatus",
                method: "GET",
                params: {
                    Id: Id,
                    Status: Status
                }
            });
        }
    });

