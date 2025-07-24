myApp.controller("DmDiaDiemTiepDanJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số   
        
        $scope.thongtintimkiem = '';
        $scope.DiaChi = '';
        $scope.lstDmDiaDiemTiepDan = [];
        $scope.DmDiaDiemTiepDan = {};
        $scope.today = new Date();
        $scope.lstPage = [];

        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.TrangThai = -1
        $scope.IdDonVi = -1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------
        
        var lblDiaChi1 = document.getElementById("lblDiaChi1");
        var lblTen = document.getElementById("lblTen");
        var lblTrangThai = document.getElementById("lblTrangThai");
        var lblIdDonVi1 = document.getElementById("lblIdDonVi1");
        //-------------------------------------------------------------------------------

        // Convert datetime
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

       

        //lấy danh đơn vị
        $scope.lstDepartments = [];
        var getAllDepartment = function () {
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

        //lấy danh phòng ban và không có ký tự -
        $scope.lstDepartmentsNoSpecialCharacter = [];
        var getAllDropDown_ByParentId_NoSpecialCharacter = function () {
            return $http({
                url: "/Departments/getAllDropDown_ByParentId_NoSpecialCharacter/",
                method: "GET",
                params: {
                    parentId: $cookieStore.get("DeparmentId")
                }
            }).success(function (data) {
                $scope.lstDepartmentsNoSpecialCharacter = data;
            });
        }
        getAllDropDown_ByParentId_NoSpecialCharacter();

        //Load danh sách data
        var getData = function () {
            return $http({
                url: "/DmDiaDiemTiepDan/GetIndex",
                method: "GET",
                params: {
                    thongtintimkiem: $scope.thongtintimkiem,
                    IdDonVi: $scope.IdDonVi,
                    DiaChi: $scope.DiaChi,
                    TrangThai: $scope.TrangThai,
                    order: $scope.Order,
                    pageSize: $scope.pageSize,
                    pageIndex: $scope.pageIndex
                    
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
                    });
                    $scope.lstDmDiaDiemTiepDan = data;
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
                        });
                        $scope.lstDmDiaDiemTiepDan = data;
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
            console.log("getId", id);
            $scope.DmDiaDiemTiepDan = {};
            $('#lblTen, #lblDiaChi1,#lblIdDonVi1').hide();
            if (id != "") {
                $scope.currentId = id;
                $("#crud").val("update");
                $scope.getbyID(id);
            }
            else {
                $("#crud").val("create");
                $scope.DmDiaDiemTiepDan = { TrangThai: 0, IdDonVi: -1 }; // 🟢 Đặt giá trị mặc định cho TrangThai, IdDonVi
            }
        }

        // Get Data By ID
        $scope.getbyID = function (id) {
            console.log("getId", id);
            crudService.get("/DmDiaDiemTiepDan/GetById?id=", id)
                .success(function (data) {
                    data.NgayTao = parseDate(data.NgayTao);
                    $scope.DmDiaDiemTiepDan = data;
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        

        // Thực hiện lưu trên modal Insert Update
        $scope.preCreate = function (data) {
            if ($("#crud").val() == "create") {
                if (validate(data) == false) {
                    return;
                }
                $scope.create(data);
                $("#myModal").modal("hide");
            }
            else if ($("#crud").val() == "update") {
                debugger
                if (validate(data) == false) {
                    return;
                }
                $scope.update(data);
                $("#myModal").modal("hide");
            }
        }

        var validate = function (data) {
            var isValid = true;
            if (data.Ten == null || data.Ten == "") {
                $("#lblTen").show();
                lblTen.textContent = "Tên dự án không để trống";
                isValid = false;
            }
            if (data.IdDonVi == null || data.IdDonVi == -1 || data.IdDonVi == "") {
                $("#lblIdDonVi1").show();
                lblIdDonVi1.textContent = "Đơn vị không để trống!";
                isValid = false;
            }
            if (data.DiaChi == null || data.DiaChi == "") {
                $("#lblDiaChi1").show();
                lblDiaChi1.textContent = "Địa chỉ không để trống";
                isValid = false;
            }
            return isValid;
        }

        // Hàm thực hiện Create
        $scope.create = function (data) {
            debugger
            crudService.create("/DmDiaDiemTiepDan/Create", data)
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
            crudService.update("/DmDiaDiemTiepDan/Update", data)
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
            UpdateIsLocked(data.id, trangthai, data.DiaChi)
                .success(function () {
                    $.notify("Cập nhật thành công!", "success");
                    clearData();
                    init();
                })
                .error(function () {
                    $.notify("Cập nhật thất bại!", "error");
                });
        }

        var UpdateIsLocked = function (Id, Status, DiaChi) {
            return $http({
                url: "/DmDiaDiemTiepDan/updateStatus",
                method: "GET",
                params: {
                    Id: Id,
                    Status: Status,
                    DiaChi: DiaChi
                }
            });
        }
        $scope.remove = function (id) {
            $("#confirm-delete").modal("hide");
            $("#divLoading").show();
            crudService.remove("/DmDiaDiemTiepDan/Delete", id)
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
            var selectedItem = $scope.lstDmDiaDiemTiepDan.find(item => item.id === id);
            if (selectedItem) {
                $scope.DmDiaDiemTiepDan = selectedItem;
            }
            
        }
    });