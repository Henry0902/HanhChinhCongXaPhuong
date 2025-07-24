
myApp.controller("DmLoaiKNTCJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số   
        $scope.thongtintimkiem = '';
        $scope.Code = '';
        $scope.TrangThai = 0;
        $scope.lstLoaiKNTC = [];
        $scope.LoaiKNTC = {};
        $scope.today = new Date();
        $scope.lstPage = [];
        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.IdLoaiDonThu = -1;
        $scope.Loai = -1;
        $scope.IdNguonDonThu = -1;
        $scope.IdLoaiKNTC = -1;
        $scope.TrangThai = -1;
        $scope.Activated = -1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------
        //var lblmaLoaiKNTC = document.getElementById("lblmaLoaiKNTC");
        //var lbltenLoaiKNTC = document.getElementById("lbltenLoaiKNTC");
        //var lblidLoaiDonThu = document.getElementById("lblidLoaiDonThu");
        //var lblidNguonDonThu = document.getElementById("lblidNguonDonThu");
        //var lblmoTa = document.getElementById("lblmoTa");
        //var lblsoThuTu = document.getElementById("lblsoThuTu");
        //var lbltrangThai = document.getElementById("lbltrangThai");
        //var lblloai = document.getElementById("lblloai");
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
                url: "/DmLoaiKNTC/GetIndex",
                method: "GET",
                params: {
                    thongtintimkiem: $scope.thongtintimkiem,
                    Code: $scope.Code,
                    TrangThai: $scope.TrangThai,
                    order: $scope.Order,
                    pageSize: $scope.pageSize,
                    pageIndex: $scope.pageIndex,
                    IdLoaiDonThu: $scope.IdLoaiDonThu,
                    IdNguonDonThu: $scope.IdNguonDonThu,
                    Loai: $scope.Loai
                }
            });
        }
        // lấy danh sách loại hồ sơ
        $scope.lstLoaiDonThu = [];
        var getAllDropdown = function () {
            crudService.getAll("/DmLoaiDonThu/getAllDropDown")
                .success(function (data) {
                    $scope.lstLoaiDonThu = data.listLoaiDonThu;
                    //console.log("lstLoaiDonThu", data.listLoaiDonThu);
                });
        }
        getAllDropdown();

        // Lấy danh sách nguồn hồ sơ
        $scope.lstNguonDon = [];
        var getNguonDon = function () {
            crudService.getAll("/DmNguonDon/GetAll")
                .success(function (data) {
                    $scope.lstNguonDon = data;
                    console.log("danh sach don thu", data);
                }).error(function (error) {
                    $.notify("Lấy danh sách nguồn hồ sơ bị lỗi.", "error");
                });;
        }
        getNguonDon();
        

        var init = function () {
            getData().success(function (data) {
                //console.log("getData", data);
                angular.forEach(data, function (item) {
                    item.NgayTao = parseDate(item.NgayTao);
                    item.ThoiDiemCapNhat = parseDate(item.ThoiDiemCapNhat);
                });
                $scope.lstLoaiKNTC = data;
                console.log("lstLoaiKNTC", data);
                if (data.length > 0) {
                    $scope.totalRecords = data[0].TotalRecords;
                } else $scope.totalRecords = 0;
                $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
                for (i = 1; i <= $scope.pageCount && i <= 9; i++) {
                    $scope.lstPage.push(i);
                }
            }).error(function (error) {

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
                        $scope.lstLoaiKNTC = data;
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
                $scope.thongtintimkiem = $scope.thongtintimkiem.trim();
                $scope.Code = $scope.Code.trim();
                $scope.reload();
            }
        }

        // Hiển thị thông tin modal Insert Update
        $scope.show = function (id) {
            debugger
            $scope.LoaiKNTC = {};
            //$('#lblmaLoaiKNTC').hide();
            //$('#lbltenLoaiKNTC').hide();
            //$('#lblidNguonDonThu').hide();
            //$('#lblidLoaiDonThu').hide();
            //$('#lblmoTa').hide();
            //$('#lblsoThuTu').hide();
            //$('#lbltrangThai').hide();
            //$('#lblloai').hide();
            if (id != "") {
                $scope.currentId = id;
                $("#crud").val("update");
                $scope.getbyID(id);
            }
            else {
                $("#crud").val("create");
                $scope.LoaiKNTC = { Loai: 0, TrangThai: 0}; // 🟢 Đặt giá trị mặc định cho Loai và trạng thái
            }
            if ($scope.addNguonDonThu) {
                $scope.addNguonDonThu.$setPristine();
                $scope.addNguonDonThu.$setUntouched();
            }
        }

        // Get Data By ID
        $scope.getbyID = function (id) {
            crudService.get("/DmLoaiKNTC/GetById?id=", id)
                .success(function (data) {
                    data.NgayTao = parseDate(data.NgayTao);
                    data.ThoiDiemCapNhat = parseDate(data.ThoiDiemCapNhat);
                    console.log("getbyId", data);
                    //$scope.LoaiKNTC_id = data.LoaiKNTC_id;
                    $scope.LoaiKNTC = data;

                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        //kiểm tra validate form insert update
        $scope.submitForm = function (form) {
            if (form.$valid) {
                $scope.preCreate($scope.LoaiKNTC); // Gửi dữ liệu khi form hợp lệ
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



        //// Thực hiện lưu trên modal Insert Update
        //$scope.preCreate = function (data) {
        //    console.log($scope.addNguonDonThu);
        //    return
        //    if ($("#crud").val() == "create") {
        //        if (validate(data) == false) {
        //            return;
        //        }
        //        $scope.create(data);
        //        $("#myModal").modal("hide");
        //    }
        //    else if ($("#crud").val() == "update") {
        //        debugger
        //        if (validate(data) == false) {
        //            return;
        //        }
        //        $scope.update(data);
        //        $("#myModal").modal("hide");
        //    }
        //}

        //var validate = function (data) {
        //    var isValid = true;
        //    if (data.IdNguonDonThu == null || data.IdNguonDonThu == -1 || data.IdNguonDonThu == "") {
        //        $("#lblidNguonDonThu").show();
        //        lblidNguonDonThu.textContent = "Nguồn hồ sơ không để trống";
        //        isValid = false;
        //    }
        //    if (data.IdLoaiDonThu == null || data.IdLoaiDonThu == -1 || data.IdLoaiDonThu == "") {
        //        $("#lblidLoaiDonThu").show();
        //        lblidLoaiDonThu.textContent = "Loại hồ sơ không để trống";
        //        isValid = false;
        //    }
        //    if (data.Code == null || data.Code == "") {
        //        $("#lblmaLoaiKNTC").show();
        //        lblmaLoaiKNTC.textContent = "Mã dự án không để trống!";
        //        isValid = false;
        //    }
        //    if (data.TenLoaiKNTC == null || data.TenLoaiKNTC == "") {
        //        $("#lbltenLoaiKNTC").show();
        //        lbltenLoaiKNTC.textContent = "Tên dự án không để trống";
        //        isValid = false;
        //    }

        //    if (data.MoTa == null || data.Description == "") {
        //        $("#lblmoTa").show();
        //        lblmoTa.textContent = "Mô tả không để trống";
        //        isValid = false;
        //    }
        //    if (data.SoThuTu == null || data.Order == "") {
        //        $("#lblsoThuTu").show();
        //        lblsoThuTu.textContent = "Số thứ tự trong danh mục không để trống";
        //        isValid = false;

        //    }
        //    if (data.TrangThai == null || data.Order == "") {
        //        $("#lbltrangThai").show();
        //        lbltrangThai.textContent = "Trạng thái trong danh mục không để trống";
        //        isValid = false;

        //    }

        //    return isValid;
        //    //if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.LoaiKNTC_ma) == true) {
        //    //    $("#lblmaLoaiKNTC").show();
        //    //    $("#lblmaLoaiKNTC").textContent = "Mã dự án không chứa ký tự đặc biệt!";
        //    //    return false;
        //    //}
        //    //if (data.LoaiKNTC_ten == null || data.LoaiKNTC_ten == "") {
        //    //    $("#lbltenLoaiKNTC").show();
        //    //    $("#lbltenLoaiKNTC").textContent = "Tên dự án không để trống!";
        //    //    return false;
        //    //}
        //    //if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.LoaiKNTC_ten) == true) {
        //    //    $("#lbltenLoaiKNTC").show();
        //    //    $("#lbltenLoaiKNTC").textContent = "Tên dự án không chứa ký tự đặc biệt!";
        //    //    return false;
        //    //}
        //}
    

        // Hàm thực hiện Create
        $scope.create = function (data) {
            debugger
            crudService.create("/DmLoaiKNTC/Create", data)
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
            crudService.update("/DmLoaiKNTC/Update", data)
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
                url: "/DmLoaiKNTC/updateStatus",
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
            crudService.remove("/DmLoaiKNTC/Delete", id)
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
            var selectedItem = $scope.lstLoaiKNTC.find(item => item.id === id);
            if (selectedItem) {
                $scope.LoaiKNTC = selectedItem;
            }

        }
    });

