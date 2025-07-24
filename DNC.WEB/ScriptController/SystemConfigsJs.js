
myApp.controller("SystemConfigsJs",
function ($location, $scope, $routeParams, crudService, $http,$cookieStore) {
    // tham so phan trang
    $scope.name = "";
    $scope.pageIndex = 1;
    $scope.pageSize = 20;
    $scope.totalRecords = 0;
    $scope.pageCount = 0;
    $scope.currentPage = 1;
    $scope.lstSystemConfigs = [];
    $scope.lstAllSystemConfigs = [];
    $scope.lstPage = [];
    // tham so xu ly du lieu
    $scope.Key = "";
    $scope.ID = null;
    $scope.Name = "";
    $scope.KeyExist = "";
    $scope.SystemConfigs = {};

    // bien xu ly tren form
    var lblKey = document.getElementById("lblKey");
    var key = document.getElementById("Key");

    // Convert datetime
    var parseDate = function (value) {
        if (value) {
            return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
        }
        return null;
    }
    //Get all data
    var getAllData = function () {
        crudService.getAll("/SystemConfigs/GetAll")
        .success(function (data) {
            $scope.lstAllSystemConfigs = data;
        })
        .error(function (error) {
            $.notify("Lỗi khi lấy dữ liệu!", "error");
        });
    }
    getAllData();
    //Load data
    var getData = function (index, pageSize) {
        return $http({
            url: "/SystemConfigs/GetIndex",
            method: "GET",
            params: { keyword: $scope.name, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex }
        });
    }
    // Init data
    var init = function () {
        crudService.getAll("/SystemConfigs/GetAll")
     .success(function (data) {
         $scope.lstAllSystemConfigs = data;
     })
        getData($scope.currentPage, $scope.pageSize)
        .success(function (data) {
            $scope.lstSystemConfigs = data;
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

    // Clear data
    var clearData = function () {
        $scope.lstPage = [];
    }
    // Lấy về trang click vào trong phân trang
    $scope.selectPage = function (index) {
        if (index == 0) {
            index = 1;
        }
        $scope.currentPage = index;
        $scope.pageIndex = index;
        $scope.lstPage.splice(0);
        var pageCount = $scope.pageCount;
        // phân theo chuỗi 9 trang
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
            getData(index, $scope.pageSize)
            .success(function (data) {
                angular.forEach(data, function (item) {
                    item.CreatedDate = parseDate(item.CreatedDate);
                });
                $scope.lstSystemConfigs = data;
                if (data.length > 0) {
                    $scope.totalRecords = data[0].TotalRecords;
                } else $scope.totalRecords = 0;
                $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
            })
            .error(function (error) {
                $.notify("Không tìm thấy dữ liệu!", "error");
            });
        }
    }
    // Tìm kiếm
    $scope.Search = function () {
        $scope.name = $("#txtKeyword").val();
        clearData();
        $scope.reload();
    }
    // Reload
    $scope.reload = function () {
        $scope.lstPage = [];
        $scope.currentPage = 1;
        $scope.lstPage.splice(0);
        clearData();
        init();
    }
    // Show popup
    $scope.showData = function (id) {
        $("#lblKey").hide();
        $scope.KeyExist = "";
        $scope.SystemConfigs = {};
        if (id != "") {
            $('#Key').attr('readonly', true);
            $("#crud").val("update");
            $scope.getDataById(id);
            $(".myModal").modal("show");
            document.getElementById("Name").focus();
            window.setTimeout(function () {
                document.getElementById("Name").focus();
            }, 600);
        } else {
            $("#crud").val("create");
            $('#Key').attr('readonly', false);
            $scope.SystemConfigs = null;

            $(".myModal").modal("show");
            key.focus();
            window.setTimeout(function () {
                key.focus();
            }, 600);
        }
    }
    // Get data by ID
    $scope.getDataById = function (id) {
        crudService.get("/SystemConfigs/GetById?id=", id)
     .success(function (data) {
         $scope.SystemConfigs = data;
         $scope.KeyExist = data.Key;
     })
        .error(function (error) {
            $.notify("Không tìm thấy dữ liệu!", "error");
        });
    } 
    // Create
    $scope.create = function (data) {
        crudService.create("/SystemConfigs/Create", data)
        .success(function (data) {
            $.notify("Tạo mới thành công!", "success");
            clearData();
            $scope.reload();
        })
        .error(function (error) {
            $.notify("Tạo mới thất bại!", "error");
        });
    }
    // Update
    $scope.update = function (data) {
        crudService.update("/SystemConfigs/Update", data)
        .success(function (result) {
            $scope.reload();
            $("#myModal").modal("hide");
            $.notify("Cập nhật thành công!", "success");
            clearData();
            
            })
        .error(function (error) {
            $.notify("Cập nhật thất bại!", "error");
        });
    }
    
    // validate
    var validate = function (data) {
        if (data == null || data == "") {
            $("#lblKey").show();
            lblKey.textContent = "Trường bắt buộc không được để trống!";
            key.focus();
            return false;
        }
        else {
            if (data.Key == null || data.Key == "") {
                $("#lblKey").show();
                lblKey.textContent = "Trường bắt buộc không được để trống!";
                key.focus();
                return false;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\]/gi.test(data.Key) == true) {
                $("#lblKey").show();
                lblKey.textContent = "Không cho phép sử dụng ký tự đặc biệt!";
                key.focus();
                return false;
            }
            else {
                return true;
            }
        }
    }
    var validateUpdate = function (data) {
        if (data.Key == null || data.Key == "") {
            $("#lblKey").show();
            lblKey.textContent = "Trường bắt buộc không được để trống!";
            key.focus();
            return false;
        }
        else {
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\]/gi.test(data.Key) == true) {
                $("#lblKey").show();
                lblKey.textContent = "Không cho phép sử dụng ký tự đặc biệt!";
                key.focus();
                return false;
            }
            else {
                return true;
            }
        }
    }
    //check exist
    var checkKeyInput = function () {
        var key = $('#Key').val();
        if (key.toUpperCase() == $scope.KeyExist.toUpperCase()) {
            return true;
        } else {
            for (var i = 0; i < $scope.lstAllSystemConfigs.length; i++) {
                if (key.toUpperCase() == $scope.lstAllSystemConfigs[i].Key.toUpperCase()) {
                    $("#lblKey").show();
                    lblKey.textContent = "Thông tin đã tồn tại. Vui lòng nhập lại!";
                    $('#Key').focus();
                    return false;
                }
                else {
                    $("#lblKey").hide();
                }
            }
            return true;
        }
    }
    // create & update
    $scope.preCreate = function (data) {
        if ($("#crud").val() == "create") {
            if (validate(data) == true && checkKeyInput() == true) {
                $scope.create(data);
            } else return false;
            $("#myModal").modal("hide");
        }
        else if ($("#crud").val() == "update") {
            if (validateUpdate(data) == true && checkKeyInput() == true) {
                $scope.update(data);
            } else return false;
            $("#myModal").modal("hide");
        }
    }

    $scope.activepage = function (page) {
        if (page == $scope.currentPage) {
            return { color: "white", background: "#f07030" }
        } else return "";
    }
    $scope.EntnkyCH = function (id) {
        $("#" + id + "").focus();
    }
    $scope.checkNull = function (infothis) {
        var idcheck = "#" + event.target.id;
        var idnoti = "#lbl" + event.target.id;
        if ($(idcheck).val() == "") {
            $(idnoti).html('Trường bắt buộc không được để trống!');
            $(idnoti).show();
        } else {
            $(idnoti).hide();
        }
    };
    // Enter tìm kiếm
    $('#txtKeyword').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $scope.Search();
        }
    });
});