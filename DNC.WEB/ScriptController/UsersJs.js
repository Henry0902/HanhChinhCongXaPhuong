myApp.controller("UsersJs",
    function($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        $scope.name = "";
        $scope.islocked = 0;
        $scope.isdelete = 0;
        $scope.pageIndex = 1;
        $scope.pageSize = 20;
        $scope.pageCount = 0;
        $scope.lstUser = [];
        $scope.lstDepartments = [];
        $scope.lstRoles = [];
        $scope.lstPage = [];
        $scope.lstSelect = [];
        $scope.lstLoadRole = [];
        $scope.lstLoadDepartment = [];
        $scope.lstPosition = [];
        $scope.total = 0;
        $scope.lstAllUser = [];
        $scope.totalUsers = 0;
        $scope.maxNewsID = '';
        $scope.Ma = "";
        $scope.ID = null;
        $scope.UserInfo = [];
        $scope.User = {};
        $scope.roleID = -1;
        $scope.depName = "";

        $scope.RoleIDSearch = $cookieStore.get("RoleIDSearch");
        // user
        var passcp = document.getElementById("passwordcp");
        var lblpwcp = document.getElementById("lblmatkhaucp");
        var pass = document.getElementById("Password");
        var us = document.getElementById("UserName");
        var email = document.getElementById("EmailUser");
        var mobile1 = document.getElementById("Mobile1");
        var lblus = document.getElementById("lbltaikhoan");
        var lblpw = document.getElementById("lblmatkhau");
        var lblemail = document.getElementById("lblemail");
        var lblsdt = document.getElementById("lblsdt");
        var lblquyen = document.getElementById("lblquyen");
        var department = document.getElementsByName("department");
        var quyen = document.getElementsByName("quyen");
        var lblpass = document.getElementById("lblpass");
        var lblpassconfirm = document.getElementById("lblpassconfirm");
        $('#Password').change(function() {
            if (pass.value === "") {
                $("#lblmatkhau").show();
                lblpw.textContent = "Mật khẩu không để trống!";
                pass.focus();
                return false;
            } else {
                $("#lblmatkhau").hide();
                return true;
            }
        });
        $("#quyen").change(function() {
            var field = $("input[name='quyen']").serializeArray();
            if (field.length == 0) {
                $("#lblquyen").show();
                lblquyen.textContent = "Vui lòng chọn nhóm quyền!";
                return true;
            } else {
                $("#lblquyen").hide();
            }
        });
        $('#EmailUser').change(function() {
            var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            var x = document.forms["createForm"]["EmailUser"].value;
            if (x == null || x == "") {
                $("#lblemail").hide();
            } else if (!regex.test(x)) {
                $("#lblemail").show();
                lblemail.textContent = "Email sai định dạng!";
                email.focus();
                return false;
            } else {
                $("#lblemail").hide();
            }
        });
        $('#Mobile1').change(function() {
            var sdt = document.forms["createForm"]["Mobile1"].value;
            if (sdt == null || sdt == "") {
                $("#lblsdt").hide();
            } else if (sdt.length < 10 || sdt.length > 11) {
                $("#lblsdt").show();
                lblsdt.textContent = "Số điện thoại không đúng!";
                return false;
            } else {
                $("#lblsdt").hide();
            }
        }); 
        // Clear Data
        var clearData = function() {
            $scope.lstPage = [];
        }
        var getData = function () { 
            return $http({
                url: "/Users/GetIndex",
                method: "GET",
                params: {
                    pageIndex: $scope.pageIndex,
                    pageSize: $scope.pageSize,
                    name: $scope.name,
                    islocked: $scope.islocked,
                    isdeleted: $scope.isdelete,
                    roleID: $scope.roleID
                }
            });
        }
        // getAllUser
        var getAllUsers = function() {
            return $http({
                url: "/Users/GetAll",
                method: "GET"
            }).success(function(data) {
                $scope.lstAllUser = data;
            });
        }
        getAllUsers();
        // Convert datetime
        var parseDate = function(value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }
        //lấy về Quyền 
        var getAllRole = function() {
            //crudService.getAll("/Roles/GetAll")
            //    .success(function(data) {
            //        $scope.lstRoles = data;
            //    });
            return $http({
                url: "/Roles/GetIndex",
                method: "GET",
                params: { pageIndex: 1, pageSize: 100, keyword: "", status: 1}
            }).success(function(data) {
                $scope.lstRoles = data;
            });
        }
        getAllRole();
        //lấy về Quyền 
        var getAllDepartment = function() {
            crudService.getAll("/Departments/getAllDropDown")
                .success(function(data) {
                    $scope.lstDepartments = data;
                });
        }
        getAllDepartment();
        var getAllDep= function () {
            crudService.getAll("/Departments/GetAll")
            .success(function (data) {
                $scope.lstLoadDepartment = data;
            });
        }
        getAllDep();
        var getDepartmentNameById = function(id) {
            crudService.get("/Departments/GetById?id=",id)
               .success(function (data) {
                   $scope.depName = data.Name;
               });
        }
        // Hàm Init để đổ dữ liệu
        var init = function () {
            if ($scope.RoleIDSearch != undefined) {
                $scope.roleID = $scope.RoleIDSearch;
            }
            getData()
            .success(function(data) {
                angular.forEach(data, function(item) {
                    item.CreatedDate = parseDate(item.CreatedDate);
                });
                $scope.lstUser = data;
                if (data.length > 0) {
                    $scope.total = data[0].CountTotal;
                } else $scope.total = 0;
                
                $scope.pageCount = Math.ceil($scope.total / $scope.pageSize);
                for (i = 1; i <= $scope.pageCount && i <= 9; i++) {
                    $scope.lstPage.push(i);
                } 
            })
            .error(function(error) {
                console.log(error);
            });
        }
        init();
        // Lấy về trang click vào trong phân trang
        $scope.selectPage = function(index) {
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
                $scope.pageIndex = index;
                getData()
                .success(function (data) {
                    angular.forEach(data, function (item) {
                        item.CreatedDate = parseDate(item.CreatedDate);
                    });
                    $scope.lstUser = data;
                    if (data.length > 0) {
                        $scope.total = data[0].CountTotal;
                    } else $scope.total = 0;
                    $scope.pageCount = Math.ceil($scope.total / $scope.pageSize);
                })
                .error(function () {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
            }
        }
        // Xử lý tìm kiếm
        $("#txtSearchByName").keypress(function(event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                $scope.Search();
            }

        });
        $scope.Search = function() {
            $scope.name = $("#txtSearchByName").val();
            $scope.roleID = parseInt($('#ddlRoles').val());
            init();
            clearData();
        }
        $scope.reload = function () {
            $scope.RoleIDSearch = undefined;
            $scope.roleID = parseInt($('#ddlRoles').val());
            $scope.pageIndex = 1;
            $scope.lstPage.splice(0);
            init();
        }
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
        //Load Roles
        var listCheckRole = function (id) {
            return $http({
                url: "/UsersRoles/UserRole_GetByUserId",
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
        // Load ROLE checkbox by ID
        $scope.SetRole = function (id) {
            var i = 0;
            var checked = false;
            for (i; i < $scope.lstLoadRole.length; i++) {
                if (id == $scope.lstLoadRole[i].RoleId) {
                    checked = true;
                    return checked;
                }
            }
            return checked;
        }
        // hàm select ROLE checkbox
        var selectCBRole = function () {
            var dt = $scope.lstRoles;
            $scope.lstSelectRole = [];
            for (var i = 0; i < dt.length; i++) {
                var id = dt[i].Id;
                var chk = $("#role" + id).is(":checked");
                if (chk) {
                    $scope.lstSelectRole.push(dt[i].Id);
                }
            }
        }
        // hàm select DEPARTMENT checkbox
        var delUR = function (id) {
            crudService.remove("/UsersRoles/Delete", id);
        }
        // Xong thì Insert
        var addUR = function (data) {
            crudService.create("/UsersRoles/Create", data);
        }
        // Hàm Create
        $scope.create = function(data) {
          //  data.DateOfBirth = $('#DateOfBirth').val();

            var dateSplit = "";
            if (data.DateOfBirth != null && data.DateOfBirth != "") {
                dateSplit = data.DateOfBirth.split('/');
                data.DateOfBirth = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            crudService.create("/Users/Create", data)
                .success(function(data) {
                    var uid = data.Id;
                    var lstdatacreate = [];
                    // Save dữ liệu vào các bảng phụ
                    if ($scope.lstSelectRole.length > 0) {
                        var list = $scope.lstSelectRole;
                        for (var i = 0; i < list.length; i++) {
                            var datacreate = {DepartmentId: 0, UserId: uid, RoleId: list[i] }
                            addUR(datacreate);
                        } 
                    }
                    $.notify("Tạo mới thành công!", "success");
                    clearData();
                    init();
                })
                .error(function(error) {
                    $.notify("Tạo mới thất bại!", "error");
                });
        }
        // Show Modal
        $scope.show = function(id) {
            $scope.User = {};
            $('#lblmatkhau').hide();
            $('#lblemail').hide();
            $('#autofocus').hide();
            $('#lbltaikhoanfalse').hide();
            $('#lbltaikhoan').hide();
            $('#lbltaikhoansuccess').hide();
            if (id != "") {
                $scope.currentId = id;
                $('#UserName').attr('readonly', true);
                $('#Password').attr('readonly', true);
                $("#crud").val("update");
                $scope.getbyID(id);

                $(".myModal").modal("show");
                $("#DisplayName").focus();
                window.setTimeout(function() {
                    $("#DisplayName").focus();
                }, 800);

            } else {
                for (var i = 0; i < quyen.length; i++) {
                    if (quyen[i].type == "checkbox") {
                        quyen[i].checked = false;
                    }
                }
                for (var f = 0; f < department.length; f++) {
                    if (department[f].type == "checkbox") {
                        department[f].checked = false;
                    }
                }
                $("#crud").val("create");
                $('#UserName').attr('readonly', false);
                $('#Password').attr('readonly', false);
                $('#checkUser').attr('style', 'display:block');
                document.getElementById("Sex").checked = true;
                $scope.User.Gender = true;
                $scope.User = null;

            //    $(".myModal").modal("show");
                us.focus();
                window.setTimeout(function() {
                    us.focus();
                }, 800);
            }
        }
        // Get Data By ID
        $scope.getbyID = function(id) {
            crudService.get("/Users/GetById?id=", id)
                .success(function(data) {
                    $scope.uid = data.Id;
                    $scope.User = data;
                    $scope.User.DateOfBirth = parseDateV($scope.User.DateOfBirth);
                    getDepartmentNameById(data.DepartmentId);
                    listCheckRole(id, $scope.du);
                }).error(function(error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };
        // Hàm Update 
        $scope.update = function(data) {
            $("#lbltaikhoan").hide();

            //Format date dd/mm/yyyy -> mm/dd/yyyy
            var dateSplit = data.DateOfBirth.split('/');
            data.DateOfBirth = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];

            crudService.update("/Users/Update", data)
                .success(function() {
                    var uid = data.Id;
                    // Save dữ liệu vào các bảng phụ
                    var list = $scope.lstSelectRole;
                    for (var i = 0; i < list.length; i++) {
                        var datacreate = { RoleId: list[i], UserId: uid }
                        addUR(datacreate);
                    }
                    $.notify("Cập nhật thành công!", "success");
                    clearData();
                    init();
                })
                .error(function() {
                    $.notify("Cập nhật thất bại!", "error");
                });
        }
        // Click Save
        $scope.preCreate = function (data) {
            if (data == null) {
                $("#lbltaikhoan").show();
                lblus.textContent = "Tài khoản không để trống!";
                $("#lblmatkhau").show();
                lblpw.textContent = "Mật khẩu không để trống!";
                $("#lbltaikhoansuccess").hide();
                $("#lbltaikhoanfalse").hide();
                $("#lblngaysinh").hide();
                us.focus();
                return;
            }
            //var currentYear = new Date().getFullYear();
            //var Yearofbirth = new Date($('#DateOfBirth').val()).getFullYear();
            //if (currentYear - Yearofbirth < 18) {
            //    $("#lblngaysinh").show();
            //    $("#DateOfBirth").focus();
            //    return;
            //} else {
            //    $("#lblngaysinh").hide();
            //}
            data.DateOfBirth = $('#DateOfBirth').val();
            if ($("#crud").val() == "create") {
                if (validate(data) == true) {
                    return;
                }
                selectCBRole();
                if ($("#Sex").is(":checked")) {
                    data.Gender = true;
                } else {
                    data.Gender = false;
                }
                data.DepartmentId = $('#Department').val();
                data.Status = 2;
                data.IsSuper = 0;
                $scope.create(data);
                $("#myModal").modal("hide");
            } else if ($("#crud").val() == "update") {
                // Load dữ liệu từ checkbox - dropdown xuống
                if (validateupdate(data) == true) {
                    return;
                }
                selectCBRole();
                delUR(data.Id);
                data.DateOfBirth = $('#DateOfBirth').val();
                data.DepartmentId = $('#Department').val();
                $scope.update(data);
                $("#lbltaikhoan").hide();
                $("#lbltaikhoansuccess").hide();
                $("#lbltaikhoanfalse").hide();
                $("#myModal").modal("hide");
            }
        }
        // Hàm đổi mật khẩu 
        $scope.checkConfirmPw = function() {
            var pass = $("#password").val();
            var rpass = $("#repeatPassword").val();
            if (rpass != null && pass != null) {
                if (rpass != pass) {
                    $("#lblpassconfirm").show();
                    lblpassconfirm.textContent = "Xác nhận mật khẩu không chính xác!";
                    return false;
                } else {
                    $("#lblpassconfirm").hide();
                }
            }
        }
        var validateChangePw = function() {
            var pass = $("#password").val();
            var rpass = $("#repeatPassword").val();
            if (pass == "" && rpass == "") {
                $("#lblpass").show();
                lblpass.textContent = "Mật khẩu không để trống!";
                $("#lblpassconfirm").show();
                lblpassconfirm.textContent = "Xác nhận mật khẩu không để trống!";
                $("#password").focus();
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
                    } else {
                        $("#lblpassconfirm").hide();
                    }
                }
            }
        }
        $scope.changePassword = function(data1) {
            var uid = data1.Id;
            if (validateChangePw() == false) {
                return;
            }
            crudService.get("/Users/GetById?id=", uid)
                .success(function(data) {
                    data.CreatedDate = parseDate(data.CreatedDate);
                    var databyid = data;
                    databyid.Password = data1.password;
                    databyid.LastChangePassword = new Date();
                    crudService.update("/Users/ChangePassword", databyid)
                        .success(function(result) {
                            $("#modalChangePassword").modal("hide");
                            $.notify("Thay đổi mật khẩu thành công!", "success");
                        });
                })
                .error(function(error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });

        }
        //Xoa du lieu bang trung gian // UserRole
        var deleteUserRoleByUserId = function (id) {
            crudService.remove("/UsersRoles/Delete", id);
        }
        $scope.checkExistUserName = function() {
            var username = $('#UserName').val();
            if (username == "" || username == null) {
                $("#lbltaikhoan").show();
                $("#lbltaikhoansuccess").hide();
                $("#lbltaikhoanfalse").hide();
                lblus.textContent = "Vui lòng nhập tên tài khoản!";
                us.focus();
                return;
            } else if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(username) == true) {
                $("#lbltaikhoan").show();
                lblus.textContent = "Tài khoản không chứa ký tự đặc biệt!";
                $("#lbltaikhoansuccess").hide();
                $("#lbltaikhoanfalse").hide();
                us.focus();
                return;
            }
            //else if (/^[a-zA-Z0-9]+$/.test(username) == false) {
            //    $("#lbltaikhoan").show();
            //    $("#lbltaikhoansuccess").hide();
            //    $("#lbltaikhoanfalse").hide();
            //    lblus.textContent = "Tài khoản chỉ chứa ký tự số và chữ!";
            //    us.focus();
            //    return;
            //}
            else {
                for (var i = 0; i < $scope.lstAllUser.length; i++) {

                    if (username.toUpperCase() == $scope.lstAllUser[i].UserName.toUpperCase()) {
                        $("#lbltaikhoanfalse").show();
                        $("#lbltaikhoan").hide();
                        $("#lbltaikhoansuccess").hide();
                        us.focus();
                        return;
                    } else {
                        $("#lbltaikhoansuccess").show();
                        $("#lbltaikhoan").hide();
                        $("#lbltaikhoanfalse").hide();
                    }
                }
            }

        }
        // Set Position Name
        $scope.setName = function(posid) {
            var dtname = $scope.lstGetAllPosition;
            for (var i = 0; i < dtname.length; i++) {
                var idname = dtname[i].ID;
                var name = dtname[i].Name;
                if (posid == idname) {
                    $('div#department').find('select').children("option[value=" + posid + "]").text(name);
                }
            }
        }
        var validate = function(data) {
            if (data == null) {
                $("#lbltaikhoan").show();
                lblus.textContent = "Tài khoản không để trống!";
                $("#lbltaikhoansuccess").hide();
                $("#lbltaikhoanfalse").hide();
                $("#lblmatkhau").show();
                lblpw.textContent = "Mật khẩu không để trống!";
                us.focus();
                return true;
            }
            else {
                //check validate cho usename
                if (data.UserName == null || data.UserName == "") {
                    $("#lbltaikhoan").show();
                    lblus.textContent = "Tài khoản không để trống!";
                    $("#lbltaikhoansuccess").hide();
                    $("#lbltaikhoanfalse").hide();
                    us.focus();
                    return true;
                } else {
                    //check exist
                    if ($scope.checkExistUserName() == true) {
                        return false;
                    }
                }
                if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.UserName) == true) {
                    $("#lbltaikhoan").show();
                    lblus.textContent = "Tài khoản không chứa ký tự đặc biệt!";
                    $("#lbltaikhoansuccess").hide();
                    $("#lbltaikhoanfalse").hide();
                    us.focus();
                    return true;
                }
                //if (/^[a-zA-Z0-9\t]+$/i.test(data.UserName) == false) {
                //    $("#lbltaikhoan").show();
                //    $("#lbltaikhoansuccess").hide();
                //    $("#lbltaikhoanfalse").hide();
                //    lblus.textContent = "Tài khoản chỉ chứa ký tự số và chữ!";
                //    us.focus();
                //    return true;
                //}
                 //check validate cho password
                if (data.Password == null || data.Password == "") {
                    $("#lblmatkhau").show();
                    lblpw.textContent = "Mật khẩu không để trống!";
                    pass.focus();
                    return true;
                }
                if (data.Password.length < 6) {
                    $("#lblmatkhau").show();
                    lblpw.textContent = "Mật khẩu phải có ít nhất 6 ký tự!";
                    pass.focus();
                    return true;
                }
                var sdt = document.forms["createForm"]["Mobile1"].value;
                if (data.Mobile == null || data.Mobile == "") {
                    $("#lblsdt").hide();
                }
                else if (sdt.length < 10 || sdt.length > 11) {
                    $("#lblsdt").show();
                    lblsdt.textContent = "Số điện thoại không đúng!";
                    mobile1.focus();
                    return true;
                }
                var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                var x = document.forms["createForm"]["EmailUser"].value;
                if (data.Email == null || data.Email == "") {
                    $("#lblemail").hide();
                }
                else if (!regex.test(x)) {
                    $("#lblemail").show();
                    lblemail.textContent = "Email sai định dạng!";
                    email.focus();
                    return true;
                }

                if (data.DisplayName == null || data.DisplayName == "") {
                    $("#lblDisplayName").show();
                    $("#lblDisplayName").html("Tên hiển thị không được để trống!");
                    $("#DisplayName").focus();
                    return true;
                } else {
                    $("#lblDisplayName").hide();
                }
                if (data.DepartmentId == null || data.DepartmentId == "") {
                    $("#lblphongban").show();
                    $("#lblphongban").html("Trường bắt buộc không được để trống!");
                    $("#DepartmentId").focus();
                    return true;
                } else {
                    $("#lblphongban").hide();
                }

                var field = $("input[name='quyen']").serializeArray();
                if (field.length == 0) {
                    $("#lblquyen").show();
                    lblquyen.textContent = "Vui lòng chọn nhóm quyền!";
                    return true;
                } else {
                    $("#lblquyen").hide();
                }
            }
            return false;
        }
        var validateupdate = function(data) {
            if (data == null) {
                $("#lblmatkhau").show();
                lblpw.textContent = "Mật khẩu không để trống!";
                pass.focus();
                return true;
            } else {
                // check validate cho password
                if (data.Password == null || data.Password == "") {
                    $("#lblmatkhau").show();
                    lblpw.textContent = "Mật khẩu không để trống!";
                    pass.focus();
                    return true;
                }
                if (data.Password.length < 6) {
                    $("#lblmatkhau").show();
                    lblpw.textContent = "Mật khẩu phải có ít nhất 6 ký tự!";
                    pass.focus();
                    return true;
                }
                var regex = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                var x = document.forms["createForm"]["EmailUser"].value;
                if (data.Email == null || data.Email == "") {
                    $("#lblemail").hide();
                } else if (!regex.test(x)) {
                    $("#lblemail").show();
                    lblemail.textContent = "Email sai định dạng!";
                    email.focus();
                    return true;
                }
                var sdt = document.forms["createForm"]["Mobile1"].value;
                if (data.Mobile == null || data.Mobile == "") {
                    $("#lblsdt").hide();
                } else if (sdt.length < 10 || sdt.length > 11) {
                    $("#lblsdt").show();
                    lblsdt.textContent = "Số điện thoại không đúng!";
                    mobile1.focus();
                    return true;
                }
                var field = $("input[name='quyen']").serializeArray();
                if (field.length == 0) {
                    $("#lblquyen").show();
                    lblquyen.textContent = "Vui lòng chọn nhóm quyền!";
                    return true;
                } else {
                    $("#lblquyen").hide();
                }
            }
            return false;
        }
        $scope.updateProfile = function(data) {
            if ($("#crud").val() == "update") {
                var DisplayName = document.getElementById('DisplayName').innerHTML;
                var add = document.getElementById('add').innerHTML;
                var email = document.getElementById('email').innerHTML;
                var dateofbirth = document.getElementById('dateofbirth').innerHTML;
                data.DisplayName = DisplayName;
                data.Address = add;
                data.Email = email;
                data.DateOfBirth = dateofbirth;
                $scope.update(data).success(function () {
                    $.notify("Cập nhật thành công !", "success");
                })
                .error(function () {
                    $.notify("Cập nhật thất bại !", "error");
                });
            }
        }

        // Hàm Delete
        $scope.remove = function(data) {
            //deleteUserRoleByUserId(data.Id);
            data.IsDeleted = true;
            crudService.update("/Users/Update", data)
                .success(function() {
                    $("#confirm-delete").modal("hide");
                    $.notify("Xóa thành công!", "success");
                    clearData();
                    init();
                })
                .error(function(error) {
                    console.log(error);
                });
        }

        // Hàm khôi phục
        $scope.repaired = function (data) {
            data.IsDeleted = false;
            crudService.update("/Users/Update", data)
                .success(function() {
                    $("#confirm-delete").modal("hide");
                    $.notify("Khôi phục thành công!", "success");
                    clearData();
                    init();
                })
                .error(function(error) {
                    console.log(error);
                });
        }

        //Check delete
        $scope.checkremove = function(data) {
            $scope.User = data;
        }

        // Hàm Update Status
        $scope.UpdateStatus = function(data) {
            if (data.IsLocked == true) {
                data.IsLocked = false;
            } else {
                data.IsLocked = true;
            }
            //crudService.update("/Users/Update", data)
            UpdateIsLocked(data.Id,data.IsLocked).success(function (result) {
                    $.notify("Cập nhật thành công!", "success");
                    clearData();
                    init();
                })
                .error(function(error) {
                    $.notify("Cập nhật thất bại!", "error");
                });
        }

        var UpdateIsLocked = function (Id, Status) {
            return $http({
                url: "/Users/updateStatus",
                method: "GET",
                params: {
                    Id: Id,
                    Status: Status
                }
            });
        }


        $scope.popupChangePass = function() {
            $("#modalProfile").modal("hide");
            $("#modalChangePassword").modal("show");
        }
        //check change password
        $scope.checkChangePassword = function(data) {
            $scope.userCP = data;
            $("#lblmatkhaucp").hide();
            $("confirm-changepassword").modal("show");
            $("#passwordcp").focus();
            window.setTimeout(function() {
                $("#passwordcp").focus();
            }, 800);
        }
        $scope.showModalChangePw = function() {
            $("#modalChangePassword").modal("show");
            $("#password").val('');
            $("#repeatPassword").val('');
            $("#lblpass").hide();
            $("#lblpassconfirm").hide();
            $("#password").focus();
            window.setTimeout(function() {
                $("#password").focus();
            }, 800);
        }
        //change password in main table
        $scope.changePasswordForUser = function(data1) {
            if (data1.password == null || data1.password == "") {
                $("#lblmatkhaucp").show();
                lblpwcp.textContent = "Mật khẩu không để trống!";
                passcp.focus();
                return true;
            }
            if (data1.password.length < 6) {
                $("#lblmatkhaucp").show();
                lblpwcp.textContent = "Mật khẩu phải có ít nhất 6 ký tự!";
                passcp.focus();
                return true;
            }
            var uid = data1.Id;
            crudService.get("/Users/GetById?id=", uid)
                .success(function(data) {
                    data.CreatedDate = parseDate(data.CreatedDate);
                    var databyid = data;
                    databyid.Password = data1.password;
                    databyid.UpdatedDate = new Date();
                    crudService.update("/Users/ChangePassword", databyid)
                        .success(function() {
                            $("#confirm-changepassword").modal("hide");
                            $.notify("Thay đổi mật khẩu thành công!", "success");
                        });
                })
                .error(function() {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        }
        $scope.EntnkyND = function(id) {
            $("#" + id + "").focus();
        }
        $scope.activepage = function (page) {
            if (page == $scope.pageIndex) {
                return { color: "white", background: "#f07030" }
            } else return "";
        }
        
    });