myApp.controller("DmHuongXuLyJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số   
        $scope.thongtintimkiem = '';
        $scope.lstDmHuongXuLy = [];
        $scope.DmHuongXuLy = {};
        $scope.today = new Date();
        $scope.lstPage = [];

        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.Loai = -1;
        $scope.TrangThai = -1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------
        var lblTenHuongXuLy = document.getElementById("lblTenHuongXuLy");
        var lblTrangThai = document.getElementById("lblTrangThai");
        var lblLoai = document.getElementById("lblLoai");
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
                url: "/DmHuongXuLy/GetIndex",
                method: "GET",
                params: {
                    thongtintimkiem: $scope.thongtintimkiem,
                    TrangThai: $scope.TrangThai,
                    order: $scope.Order,
                    pageSize: $scope.pageSize,
                    pageIndex: $scope.pageIndex,
                    Loai: $scope.Loai
                }
            });
        }

        // Init data
        var init = function () {
            $("#divLoading").show();
            getData()
                .success(function (data) {
                    angular.forEach(data, function (item) {
                        item.NgayTao = parseDate(item.NgayTao);
                        item.ThoiDiemCapNhat = parseDate(item.ThoiDiemCapNhat);
                    });
                    $scope.lstDmHuongXuLy = data;
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
                            item.NgayTao = parseDate(item.NgayTao);
                            item.ThoiDiemCapNhat = parseDate(item.ThoiDiemCapNhat);
                        });
                        $scope.lstDmHuongXuLy = data;
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

        // Check for Enter key press
        $scope.checkEnter = function (event) {
            if (event.keyCode === 13) { // Enter key code
                $scope.reload();
            }
        }

        // Hiển thị thông tin modal Insert Update
        $scope.show = function (id) {
            $scope.DmHuongXuLy = {};
            if (id != "") {
                $scope.currentId = id;
                $("#crud").val("update");
                $scope.getbyID(id);
            }
            else {
                $("#crud").val("create");
                $scope.DmHuongXuLy = { Loai: 0, TrangThai: 0 }; // 🟢 Đặt giá trị mặc định cho Loai và TrangThai
            }
            if ($scope.addHuongXuLy) {
                $scope.addHuongXuLy.$setPristine();
                $scope.addHuongXuLy.$setUntouched();
            }
        }

        // Get Data By ID
        $scope.getbyID = function (id) {
            crudService.get("/DmHuongXuLy/GetById?id=", id)
                .success(function (data) {
                    data.NgayTao = parseDate(data.NgayTao);
                    data.ThoiDiemCapNhat = parseDate(data.ThoiDiemCapNhat);
                    $scope.DmHuongXuLy = data;
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        //kiểm tra validate form insert update
        $scope.submitForm = function (form) {
            if (form.$valid) {
                $scope.preCreate($scope.DmHuongXuLy); // Gửi dữ liệu khi form hợp lệ
            } else {
                angular.forEach(form.$error.required, function (field) {
                    field.$setTouched(); // Đánh dấu các trường bắt buộc để hiển thị lỗi
                });
            }
        };

        // Thực hiện lưu trên modal Insert Update
        $scope.preCreate = function (data) {
            if ($("#crud").val() === "create") {
                $scope.create(data);
            } else if ($("#crud").val() === "update") {
                $scope.update(data);
            }

            $("#myModal").modal("hide");
        };

        //var validate = function (data) {
        //    var isValid = true;
        //    if (data.TenHuongXuLy == null || data.TenHuongXuLy == "") {
        //        $("#lblTenHuongXuLy").show();
        //        lblTenHuongXuLy.textContent = "Tên hướng xử lý không để trống";
        //        isValid = false;
        //    }
        //    return isValid;
        //}

        // Hàm thực hiện Create
        $scope.create = function (data) {
            crudService.create("/DmHuongXuLy/Create", data)
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
            crudService.update("/DmHuongXuLy/Update", data)
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
            var trangthai = 0;
            if (data.TrangThai == 0) {
                trangthai = 1;
            }
            UpdateIsLocked(data.id, trangthai)
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
                url: "/DmHuongXuLy/UpdateStatus",
                method: "GET",
                params: {
                    Id: Id,
                    Status: Status
                }
            });
        }

        $scope.remove = function (id) {
            $("#confirm-delete").modal("hide");
            $("#divLoading").show();
            crudService.remove("/DmHuongXuLy/Delete", id)
                .success(function (data) {
                    if (data.Messeger == 1) {
                        $.notify("Danh mục đang được sử dụng, không xóa được !", "error");
                    } else if (data.Messeger == 2) {
                        $.notify("Xóa thành công !", "success");
                    }
                    $scope.reload();
                    $("#divLoading").hide();
                })
                .error(function (error) {
                    $.notify("Xóa thất bại !", "error");
                    console.log(error);
                    $("#divLoading").hide();
                });

        }

        $scope.checkremove = function (id) {
            var selectedItem = $scope.lstDmHuongXuLy.find(item => item.id === id);
            if (selectedItem) {
                $scope.DmHuongXuLy = selectedItem;
            }
        }
    });
