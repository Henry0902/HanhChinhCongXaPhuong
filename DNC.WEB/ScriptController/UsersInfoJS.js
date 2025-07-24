myApp.controller("UsersInfoJs",
function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore, $rootScope, $timeout) { 
    $scope.User = {};
    $scope.du = 0;
    $scope.fdr = 0;
    $scope.layoutUID = 0;
    $scope.depName = "";
    $scope.lstLoadRole = [];

    //khai báo dữ liệu
    var lblpass = document.getElementById("lblpass");
    var lblopass = document.getElementById("lbloldpass");
    var lblpassconfirm = document.getElementById("lblpassconfirm");
    // Convert datetime
    var parseDate = function (value) {
        if (value) {
            return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
        }
        return null;
    }    

    $scope.getRolebyDepartment = function (id) {
        $scope.lstRoles = [];
        $http({
            url: "/Roles/getRolebyUserandDep",
            method: "GET",
            params: {
                id:0,
                DepId: id
            }
        }).success(function (data) {
            $scope.lstRoles = data;
        });
    };
    var getDepartmentNameById = function (id) {
        crudService.get("/Departments/GetById?id=", id)
           .success(function (data) {
               $scope.depName = data.Name;
           });
    }
    //Load Roles
    var listCheckRole = function (id) {
        return $http({
            url: "/Roles/GetRoleByUserId",
            method: "GET",
            params: {
                id: id
            }
        })
         .success(function (data) {
             $scope.lstLoadRole = data;
         })
         .error(function (error) {
             $.notify("Không tìm thấy dữ liệu!", "error");
         });
    }
    
    $scope.showUserInfo = function (uId) {
        $("#FirstName").focus();
        $scope.User = {};
        $scope.layoutUID = uId; 
        crudService.get("/Users/GetById?id=", uId)
        .success(function(data) {
            $scope.User = data;
            getDepartmentNameById(data.DepartmentId);
            listCheckRole(data.Id);            
        });

        window.setTimeout(function () {
            $("#FirstName").focus();
        }, 800);
    };  
    $scope.updateProfile = function (data) {
        var user = data;
        crudService.update("/Users/Update", data)
        .success(function() {
            $.notify("Cập nhật thành công!", "success");
            $("#modalProfile").modal("hide");
            clearData();
            init();            
        })
        .error(function() {
            $.notify("Cập nhật thất bại!", "error");
        });
    }
    
    // Đổi mật khẩu
    $scope.showModalChangePw = function (uId) {
        $scope.layoutUID = uId;
        $("#modalChangePassword").modal("show");
        $("#oldpassword").val('');
        $("#password").val('');
        $("#repeatPassword").val('');
        $("#lbloldpass").hide();
        $("#lblpass").hide();
        $("#lblpassconfirm").hide();
        $("#password").focus();
        window.setTimeout(function () {
            $("#oldpassword").focus();
        }, 800);
    }
    // Hàm đổi mật khẩu 
    $scope.checkConfirmPw = function () {
        var pass = $("#password").val();
        var rpass = $("#repeatPassword").val();
        if (rpass != null && pass != null) {
            if (rpass != pass) {
                $("#lblpassconfirm").show();
                lblpassconfirm.textContent = "Xác nhận mật khẩu không chính xác!";
                return false;
            }
            else {
                $("#lblpassconfirm").hide();
            }
        }
    }
    var validateChangePw = function () {
        var opass=$('#oldpassword').val();
        var pass = $("#password").val();
        var rpass = $("#repeatPassword").val();
        if (pass == "" && rpass == "" && opass == "") {
            $("#lbloldpass").show();
            lblopass.textContent = "Mật khẩu không để trống!";
            $("#lblpass").show();
            lblpass.textContent = "Mật khẩu không để trống!";
            $("#lblpassconfirm").show();
            lblpassconfirm.textContent = "Xác nhận mật khẩu không để trống!";
            $("#oldpassword").focus();
            return false;
        } else {
            if (pass == null || pass == "") {
                $("#lblpass").show();
                lblpass.textContent = "Mật khẩu không để trống!";
                $("#password").focus();
                return false;
            } else {
                $("#lblpass").hide();
            }
            if (pass.length < 6) {
                $("#lblpass").show();
                lblpass.textContent = "Mật khẩu phải có ít nhất 6 ký tự!";
                $("#password").focus();
                return false;
            } else {
                $("#lblpass").hide();
            }
            if (rpass == null || rpass == "") {
                $("#lblpassconfirm").show();
                lblpassconfirm.textContent = "Xác nhận mật khẩu không để trống!";
                $("#repeatPassword").focus();
                return false;
            } else {
                $("#lblpassconfirm").hide();
            }
            if (rpass != null && pass != null) {
                if (rpass != pass) {
                    $("#lblpassconfirm").show();
                    lblpassconfirm.textContent = "Xác nhận mật khẩu không chính xác!";
                    $("#repeatPassword").focus();
                    return false;
                }
                else {
                    $("#lblpassconfirm").hide();
                }
            }
        }
    }

    $scope.passwordData = {};
    $scope.showChangePasswordModal = function () {
        // Xóa dữ liệu cũ
        $scope.passwordData = {};

        if ($scope.passwordForm) {
            // Reset trạng thái validate của form
            $scope.passwordForm.$setPristine();
            $scope.passwordForm.$setUntouched();
        }

        // Hiển thị modal
        $('#changePasswordModal2').modal('show');
    };

    $scope.savePass = function () {
        if ($scope.passwordForm.$invalid) {
            // Đánh dấu các trường là "touched" để hiển thị lỗi
            angular.forEach($scope.passwordForm.$error, function (field) {
                angular.forEach(field, function (errorField) {
                    errorField.$setTouched();
                });
            });
            return; // Dừng lại nếu form chưa hợp lệ
        }

        // Nếu hợp lệ, thực hiện đổi mật khẩu
        crudService.get("/Users/GetById?id=", $cookieStore.get("UserID"))
            .success(function (data) {
                data.CreatedDate = parseDate(data.CreatedDate);
                data.DateOfBirth = parseDate(data.DateOfBirth);
                data.IssuanceDate = parseDate(data.IssuanceDate);
                var databyid = data;
                databyid.Password = $scope.passwordData.newPassword;
                databyid.LastChangePassword = new Date();
                crudService.update("/Users/ChangePassword", databyid)
                    .success(function () {
                        $("#changePasswordModal2").modal("hide");
                        $.notify("Thay đổi mật khẩu thành công!", "success");
                    });
            })
            .error(function () {
                $("#changePasswordModal2").modal("hide");
                $.notify("Không tìm thấy dữ liệu!", "error");
            });
    };

    $scope.changePassword = function (data1) {
        var uid = $("#uID").val();
        if (validateChangePw() == false) {
            return false;
        } else {
            var oldPassword = $('#oldpassword').val();
            crudService.get("/Users/CheckPassword?pass=", oldPassword).success(function (data) {
                if (data == 'False') {
                    $("#lbloldpass").show();
                    lblopass.textContent = "Mật khẩu không đúng, vui lòng thử lại!";
                    return false;
                } else if (data == 'True') {
                    $("#lbloldpass").hide();
                    crudService.get("/Users/GetById?id=", $scope.layoutUID)
                    .success(function (data) {
                        data.CreatedDate = parseDate(data.CreatedDate);
                        var databyid = data;
                        databyid.Password = data1.password;
                        databyid.LastChangePassword = new Date();
                        crudService.update("/Users/ChangePassword", databyid)
                            .success(function () {
                                $("#modalChangePassword").modal("hide");
                                $.notify("Thay đổi mật khẩu thành công!", "success");
                            });
                    })
                    .error(function () {
                        $.notify("Không tìm thấy dữ liệu!", "error");
                    });
                }
            });
        } 
    }
});