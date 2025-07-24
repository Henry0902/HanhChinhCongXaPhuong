myApp.controller("UserServiceJs",
function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
    $scope.TenController = 'UserService';

    $scope.strSearch = '';
    $scope.trang_thai = 0;
    $scope.lstUserService = [];
    $scope.UserService = {};
    $scope.today = new Date();
    $scope.lstPage = [];

    $scope.Order = 'desc';
    $scope.pageIndex = 1;
    $scope.pageSize = 10;
    $scope.totalRecords = 0;
    $scope.pageCount = 0;

    var lblMa = document.getElementById("lblMa");
    var lblPassword = document.getElementById("lblPassword");
    var lblRetypePass = document.getElementById("lblRetypePass");
    var lblUEmail = document.getElementById("lblUEmail");
    var lblTrangthai = document.getElementById("lblTrangthai");

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
            url: "/" + $scope.TenController + "/GetIndex",
            method: "GET",
            params: { strSearch: $scope.strSearch, trang_thai: $scope.trang_thai, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex }
        });
    }

    // Init data
    var init = function () {
        getData()
        .success(function (data) {
            angular.forEach(data.Items, function (item) {
                item.ngay_tao = parseDate(item.ngay_tao);
            });
            $scope.lstUserService = data.Items;
            $scope.totalRecords = data.TotalRecords;
            $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
            for (var i = 1; i <= $scope.pageCount && i <= 9; i++) {
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
                angular.forEach(data.Items, function (item) {
                    item.ngay_tao = parseDate(item.ngay_tao);
                });
                $scope.lstUserService = data.Items;
                $scope.totalRecords = data.TotalRecords;
                $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
            })
            .error(function (error) {
                $.notify("Không tìm thấy dữ liệu!", "error");
            });
        }
    }

    $scope.reload = function () {
        $scope.pageIndex = 1;
        $scope.lstPage.splice(0);
        $scope.lstPage = [];
        init();
    }


    $scope.show = function (id) {
        $scope.User = {};
        $("#lblMa").hide();
        $("#lblPassword").hide();
        $("#lblRetypePass").hide();
        $("#lblUEmail").hide();
        if (id != "") {
            $scope.currentId = id;
            $("#crud").val("update");
            $scope.getbyID(id);
        }
        else {
            $("#crud").val("create");
            $scope.UserService = null;
        }
    }

    // Get Data By ID
    $scope.getbyID = function (id) {
        crudService.get("/" + $scope.TenController + "/GetById?id=", id)
            .success(function (data) {
                data.ngay_tao = parseDate(data.ngay_tao);
                $scope.kho_id = data.kho_id;
                $scope.UserService = data;

            }).error(function (error) {
                $.notify("Không tìm thấy dữ liệu!", "error");
            });
    };

    $scope.preCreate = function (data) {
        
        if (data == null) {
            $("#lblMa").show();
            lblMa.textContent = "Tài khoản service không để trống";
            $("#lblPassword").show();
            lblPassword.textContent = "Mật khẩu không để trống";
            $("#lblRetypePass").show();
            lblRetypePass.textContent = "Mật khẩu nhập lại không để trống";
            $("#lblUEmail").show();
            lblUEmail.textContent = "Email không để trống";
            $("#lblTrangthai").show();
            lblTrangthai.textContent = "Vui lòng chọn trạng thái sử dụng";
            return;
        }

        if ($("#crud").val() == "create") {
            if (validate(data) == false) {
                return;
            }
            $scope.create(data);
        } else if ($("#crud").val() == "update") {
            if (validate(data) == false) {
                return;
            }
            $scope.update(data);
        }
    }

    //Kiểm tra đầu vào trước khi tạo mới hoặc cập nhật user service
    var validate = function (data) {
        $("#lblMa").hide();
        $("#lblPassword").hide();
        $("#lblRetypePass").hide();
        $("#lblUEmail").hide();
        $("#lblTrangthai").hide();

        var pass = document.getElementById("txtPass").value;
        var retypePass = document.getElementById("txtRetypePass").value;

        var isError = false;
        if (data.userservice_name == null || data.userservice_name == "") {
            alert(data.userservice_name);
            $("#lblMa").show();
            lblMa.textContent = "Tài khoản service không để trống!";
            isError = true;
        }
        else if (/[`~!@#$%^&*()|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.userservice_name) == true) {
            $("#lblMa").show();
            lblMa.textContent = "Tài khoản không chứa ký tự đặc biệt!";
            isError = true;
        }
        if (data.userservice_password == null || data.userservice_password == "") {
            $("#lblPassword").show();
            lblPassword.textContent = "Mật khẩu không để trống!";
            isError = true;
        }

        if (pass != retypePass) {
            $("#lblRetypePass").show();
            lblRetypePass.textContent = "Mật khẩu nhập lại không giống mật khẩu ở trên!";
            isError = true;
        }

        if (data.userservice_email == null || data.userservice_email == "") {
            $("#lblUEmail").show();
            lblUEmail.textContent = "Email không để trống!";
            isError = true;
        } else {
            var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (!regex.test(data.userservice_email)) {
                $("#lblUEmail").show();
                lblUEmail.textContent = "Email sai định dạng!";
                isError = true;
            }
        }

        if (data.trang_thai == 0 || data.trang_thai == 1) {
            $("#lblTrangthai").hide();
        } else {
            $("#lblTrangthai").show();
            lblTrangthai.textContent = "Vui lòng chọn trạng thái sử dụng";
            isError = true;
        }
        
        if (isError) {
            return false;
        }

        return true;
    }

    //Tạo mới
    $scope.create = function (data) {
        crudService.create("/" + $scope.TenController + "/Create", data)
            .success(function (result) {
                if (result != null && result.userservice_id == 0) {
                    $("#lblMa").show();
                    lblMa.textContent = "Tài khoản service đã tồn tại. Hãy tạo tài khoản khác!";
                } else {
                    $("#myModal").modal("hide");
                    $.notify("Tạo mới thành công.", "success");
                    clearData();
                    init();
                }
                
            })
            .error(function (error) {
                $.notify("Tạo mới không thành công. Hãy thử lại!", "error");
            });
    }

    //Cập nhật
    $scope.update = function (data) {
        crudService.update("/" + $scope.TenController + "/Update", data)
            .success(function () {
                $("#myModal").modal("hide");
                $.notify("Cập nhật thành công!", "success");
                clearData();
                init();
            })
            .error(function () {
                $.notify("Cập nhật không thành công. Hãy thử lại!", "error");
            });
    }

    $scope.UpdateStatus = function (data) {
        var trangThai = 0;
        if (data.trang_thai == 0) {
            trangThai = 1;
        }
        $http({
            url: "/" + $scope.TenController + "/UpdateStatus",
            method: "GET",
            params: {
                userServiceId: data.userservice_id,
                status: trangThai
            }
        }).success(function () {
            data.trang_thai = trangthai;
            $.notify("Xóa thành công!", "success");
            clearData();
            init();
        }).error(function () {
            $.notify("Xóa không thành công. Hãy thử lại!", "error");
        });
    }

});