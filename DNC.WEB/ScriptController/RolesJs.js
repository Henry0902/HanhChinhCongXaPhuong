
myApp.controller("RoleJs",
function ($location, $scope, $routeParams, crudService, $http, $cookieStore) {
    $scope.name = "";
    $scope.trangthai = -1;
    $scope.isdelete = -1;
    $scope.currentPage = 1;
    $scope.pageSize = 20;
    $scope.pageCount = 0;
    $scope.lstPage = [];
    $scope.current = 0;
    $scope.total = 0;
    $scope.lstAllRole = [];
    $scope.lstMenuFunction = [];
    // crud
    $scope.Ma = "";
    $scope.ID;
    $scope.Name;
    $scope.Role = {};
    $scope.UserInfo = [];
    var lblName = document.getElementById("lblName");
    var ten = document.getElementById("Name");

    // validate tên
    $scope.checkNameInput = function () {
        debugger;
        if (ten.value === "") {
            $("#lblName").show();
            lblName.textContent = "Trường bắt buộc không được để trống!";
            ten.focus();
            return;
        }
        if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\]/gi.test(ten.value) == true) {
            $("#lblName").show();
            lblName.textContent = "Tên nhóm quyền không chứa ký tự đặc biệt!";
            ten.focus();
        }
        else {
            $("#lblName").hide();
        }
    }

    var getData = function (index, pageSize) {
        return $http({
            url: "/Roles/GetIndex",
            method: "GET",
            params: { pageIndex: $scope.currentPage, pageSize: $scope.pageSize, keyword: $scope.name, status: $scope.trangthai }
        });
    }
    // Convert datetime
    var parseDate = function (value) {
        if (value) {
            return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
        }
        return null;
    }
    // Clear Data
    var ClearData = function () {
        $scope.lstPage = [];
    }
    // Hàm Init để đổ dữ liệu
    var init = function () {
        $scope.lstRole = [];
        getData($scope.currentPage, $scope.pageSize)
       .success(function (data) {
           angular.forEach(data, function (item) {
               item.CreatedDate = parseDate(item.CreatedDate);
           });
           $scope.lstRole = data;
           //$scope.total = data[0].CountTotal;
           if (data.length > 0) {
               $scope.total = data[0].CountTotal;
           }
           else {
               $scope.total = 0;
           }
           $scope.pageCount = Math.ceil($scope.total / $scope.pageSize);
           for (i = 1; i <= $scope.pageCount && i <= 9; i++) {
               $scope.lstPage.push(i);
           }
       })
       .error(function (error) {
           console.log(error);
       });
    }
    init();
    // Hàm check tên trong hệ thống
    var checkName = function (Name) {
        return $http({
            url: "/Roles/checkRoleName",
            method: "GET",
            params: { name: Name }
        });
    }
    // Lấy về trang click vào trong phân trang
    $scope.selectPage = function (index) {
        if (index == 0) {
            return;
        }
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
            $scope.currentPage = index;
            getData($scope.currentPage, $scope.pageSize)
            .success(function (data) {
                angular.forEach(data, function (item) {
                    item.CreatedDate = parseDate(item.CreatedDate);
                });
                $scope.lstRole = data;
                $scope.total = data[0].CountTotal;
                $scope.pageCount = Math.ceil($scope.total / $scope.pageSize);
            })
            .error(function (error) {
                $.notify("Không tìm thấy dữ liệu!", "error");
            });
        }
    }
    // Xử Lý intenr Hàm Tìm Kiếm
    $('#txtFilter').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $scope.Search();
        }
    });
    // Xử Lý Hàm Tìm Kiếm
    $scope.Search = function () {
        $scope.name = $("#txtFilter").val();
        ClearData();
        init();
    }
    // display index
    $scope.reload = function () {
        $scope.currentPage = 1;
        $scope.lstPage.splice(0);
        init();
    }
    // Hàm Create
    $scope.create = function (data) {
        crudService.create("/Roles/Create", data)
        .success(function (data) {
            $.notify("Tạo mới thành công!", "success");
            ClearData();
            init();
        })
        .error(function (error) {
            $.notify("Tạo mới thất bại!", "error");
        });
    }
    // Show Modal
    $scope.show = function (id) {
        $("#lblName").hide();
        $scope.Role = {};
        if (id != "") {
            $("#crud").val("update");
            $scope.getbyID(id);

        } else {
            $("#crud").val("create");
            $("#ID").val('');
            $("#Name").val('');
            $("#Description").val('');
        }
        $(".myModal").modal("show");
        ten.focus();
        window.setTimeout(function () {
            ten.focus();
        }, 800);
    }
    // Get Data By ID
    $scope.getbyID = function (id) {
        crudService.get("/Roles/GetById?id=", id)
     .success(function (data) {
         data.CreatedDate = parseDate(data.CreatedDate);
         $scope.Role = data;
         $scope.Name = data.Name;
     })
        .error(function (error) {
            $.notify("Không tìm thấy dữ liệu!", "error");
        });
    }
    // Hàm Update 
    $scope.update = function (data) {
        crudService.update("/Roles/Update", data)
    .success(function (result) {
        $("#myModal").modal("hide");
        $.notify("Cập nhật thành công!", "success");
        ClearData();
        init();
    })
    .error(function (error) {
        $.notify("Cập nhật thất bại!", "error");
    });
    }


    var checkExistNhomQuyen = function () {
        var nhomquyen = $('#Name').val();
        for (var i = 0; i < $scope.lstAllRole.length; i++) {

            if (nhomquyen.toUpperCase() == $scope.lstAllRole[i].Name.toUpperCase()) {
                $("#lblName").show();
                lblName.textContent = "Nhóm quyền đã tồn tại. Vui lòng nhập lại!";
                ten.focus();
                return true;
            }
        }
    };

    var checkExistNhomQuyenUpdate = function () {
        var nhomquyen = $('#Name').val();
        var nhomquyensua = $scope.Name;

        for (var i = 0; i < $scope.lstAllRole.length; i++) {
            if (nhomquyensua.toUpperCase() == nhomquyen.toUpperCase()) {
                return false;
            } else
                if (nhomquyen.toUpperCase() == $scope.lstAllRole[i].Name.toUpperCase()) {
                    $("#lblName").show();
                    lblName.textContent = "Nhóm quyền đã tồn tại. Vui lòng nhập lại!";
                    ten.focus();
                    return true;
                }
        }
    };

    var validate = function (data) {
        if (data == null) {
            $("#lblName").show();
            lblName.textContent = "Trường bắt buộc không được để trống!";
            ten.focus();
            return true;
        }
        else {

            if (data.Name == null || data.Name == "") {
                $("#lblName").show();
                lblName.textContent = "Trường bắt buộc không được để trống!";
                ten.focus();
                return true;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.Name) == true) {
                $("#lblName").show();
                lblName.textContent = "Tên nhóm quyền không chứa ký tự đặc biệt!";
                ten.focus();
                return true;
            }

        }
        return false;
    }
    var validateUpdate = function (data) {
        if (data == null) {
            $("#lblName").show();
            lblName.textContent = "Trường bắt buộc không được để trống!";
            ten.focus();
            return true;
        }
        else {

            if (data.Name == null || data.Name == "") {
                $("#lblName").show();
                lblName.textContent = "Trường bắt buộc không được để trống!";
                ten.focus();
                return true;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.Name) == true) {
                $("#lblName").show();
                lblName.textContent = "Tên nhóm quyền không chứa ký tự đặc biệt!";
                ten.focus();
                return true;
            }
        }
        return false;
    }

    $scope.preCreate = function (data) {
        if ($("#crud").val() == "create") {
            if (validate(data) == true)
            { return; }
            checkName(data.Name)
            .success(function (result) {
                if (result > 0) {
                    $("#lblName").html("Tên nhóm quyền đã tồn tại trong hệ thống!");
                    $("#lblName").show();
                    $("#Name").focus();
                }
                if (result == 0) {
                    data.Status = true;
                    $scope.create(data);
                    $("#myModal").modal("hide");
                }
            }).error(function () {

            });
        }
        else if ($("#crud").val() == "update") {
            if (validateUpdate(data) == true)
            { return; }
            if (data.Name == $scope.Name) {
                $scope.update(data);
                $("#myModal").modal("hide");
            }
            else {
                checkName(data.Name)
                .success(function (result) {
                    if (result > 0) {
                        $("#lblName").html("Tên nhóm quyền đã tồn tại trong hệ thống!");
                        $("#lblName").show();
                    }
                    if (result == 0) {
                        $scope.update(data);
                        $("#myModal").modal("hide");
                    }
                }).error(function () {

                });
            }
        }
    }

    // Hàm Delete
    $scope.remove = function (id) {
        crudService.get("/Roles/countUserPagebyRoles?id=", id)
            .success(function(result) {
                if (result.CountUserRoles > 0 || result.CountPageRoles > 0) { 
                    if (result.CountUserRoles > 0) {
                        $.notify("Nhóm quyền còn người dùng!", "error");
                    }
                    if (result.CountPageRoles > 0) {
                        $.notify("Nhóm quyền còn thiết lập các chức năng!", "error");
                    } 
                    $("#confirm-delete").modal("hide");
                    return;
                } else {
                    crudService.remove("/Roles/Delete", id)
                        .success(function(data) {
                            if (data == 'True') {
                                $("#confirm-delete").modal("hide");
                                $.notify("Xóa thành công!", "success");
                                ClearData();
                                init();
                            } else {
                                ClearData();
                                $("#confirm-delete").modal("hide");
                                $.notify("Xóa thất bại!", "error");

                            }
                        })
                        .error(function(error) {
                            ClearData();
                            $("#confirm-delete").modal("hide");
                            $.notify("Xóa thất bại!", "error");
                        });
                }
            });
    }
    // Destroy record
    $scope.destroy = function (data) {
        crudService.update("/Role/Update", data)
        .success(function (result) {
            $.notify("Cập nhật thành công!", "success");
            $('#confirm-destroy').modal('hide');
            init();
        })
        .error(function (error) {
            $.notify("Cập nhật thất bại!", "error");
        });
    }

    //Check delete
    $scope.checkremove = function (data) {
        $scope.Role = data;
        $("#confirm-delete").modal("Show");
    }
    // Show modal cập nhật trạng thái
    $scope.checkStatus = function (data) {
        $scope.dataStatus = data;
        $scope.ID = data.Id;
        $scope.Name = data.Name;
        if (data.Status == true) {
            $scope.actionStatus = "vô hiệu hóa";
        }
        else {
            $scope.actionStatus = "sử dụng";
        }
        $("#confirm-status").modal("show");
    }
    // Hàm Update Status
    $scope.UpdateStatus = function (data) {
        if (data.Status == true) {
            data.Status = false;
        }
        else {
            data.Status = true;
        }
        crudService.update("/Roles/Update", data)
        .success(function (result) {
            $("#confirm-status").modal("hide");
            $.notify("Cập nhật thành công!", "success");
            ClearData();
            init();
        })
        .error(function (error) {
            $("#confirm-status").modal("hide");
            $.notify("Cập nhật thất bại!", "error");
        });
    }

    $scope.SearchUserbyRole = function (id) {
        $cookieStore.put("RoleIDSearch", id);
        window.location.href = '/Users/Index';
    }

    $scope.SetFunctionRole = function (data) {
        $("#SetFunc").modal("show");
        $scope.quyen = data;
        crudService.get("/Roles/getPageFuncRole?id=", data.Id)
           .success(function (result) {
               $scope.lstMenuFunction = result;
           });
    }
    // Get all Function 
    var getallFunction = function () {
        crudService.getAll("/Roles/getFunction")
        .success(function (result) {
            $scope.lstallFunction = result;
        });
    };

    getallFunction();

    var getallMenuLink = function () {
        crudService.getAll("/Pages/getForRole")
            .success(function(result) {
                var items = [];
                var source = [];
                for (var i = 0; i < result.length; i++) {
                    //set gia tri cho item
                    var itemdata = result[i];
                    var name = itemdata["Name"];
                    var parentid = itemdata["ParentId"];
                    var id = itemdata["PageId"];
                    var functions = "";
                    if (itemdata["Functions"] != null || itemdata["Functions"] !=undefined) {
                        functions = itemdata["Functions"].split(';');
                    }
                    if (items[parentid]) {
                        var item = { id: id, parentid: parentid, functions: functions, name: name, items: [] };
                        if (!items[parentid].items) {
                            items[parentid].items = [];
                        }
                        items[parentid].items[items[parentid].items.length] = item;
                        items[id] = item;

                    } else {
                        items[id] = { id: id, parentid: parentid, functions: functions, name: name, items: [] };
                        source.push(items[id]);
                    }
                }
                $scope.lstallMenuLink = source;
            });
    }
    getallMenuLink();

    $scope.showSub = function (id) {
        var icon = $("#links" + id).html();
        if (icon == '<i class=' + '"' + 'fa fa-plus' + '"' + '></i>') {
            $("#links" + id).html('<i class=' + '"' + 'fa fa-minus' + '"' + '></i>')
            $("#submenu" + id).collapse('show');
        }
        else {
            $("#links" + id).html('<i class=' + '"' + 'fa fa-plus' + '"' + '></i>')
            $("#submenu" + id).collapse('hide');
        }
    };

    $scope.showRole = function (id) {
        var icon = $("#sub" + id).html();
        if (icon == '<i class=' + '"' + 'fa fa-plus' + '"' + '></i>') {
            $("#sub" + id).html('<i class=' + '"' + 'fa fa-minus' + '"' + '></i>')
            $("#menurole" + id).collapse('show');
        }
        else {
            $("#sub" + id).html('<i class=' + '"' + 'fa fa-plus' + '"' + '></i>')
            $("#menurole" + id).collapse('hide');
        }
    };

    $scope.setalllinkcheck = function (id) {
        var check = document.getElementById("linkselect" + id);
        if (check.checked == true) {

            for (var j = 0; j < $scope.lstallMenuLink.length; j++) {
                if ($scope.lstallMenuLink[j].id == id && $scope.lstallMenuLink[j].items.length > 0) {
                    for (var k = 0; k < $scope.lstallMenuLink[j].items.length; k++) {
                        $scope.lstMenuFunction.push({ "PageId": $scope.lstallMenuLink[j].items[k].id, "FunctionId": 1 });
                        $scope.lstMenuFunction.push({ "PageId": $scope.lstallMenuLink[j].items[k].id, "FunctionId": 2 });
                        $scope.lstMenuFunction.push({ "PageId": $scope.lstallMenuLink[j].items[k].id, "FunctionId": 3 });
                        $scope.lstMenuFunction.push({ "PageId": $scope.lstallMenuLink[j].items[k].id, "FunctionId": 4 });
                    }
                }
            }

            $scope.lstMenuFunction.push({ "PageId": id, "FunctionId": 1 });
            $scope.lstMenuFunction.push({ "PageId": id, "FunctionId": 2 });
            $scope.lstMenuFunction.push({ "PageId": id, "FunctionId": 3 });
            $scope.lstMenuFunction.push({ "PageId": id, "FunctionId": 4 });
        }
        else {
            var lsttemp = [];
            var lstMenuFunction = $scope.lstMenuFunction;
            var lstallMenuLink = $scope.lstallMenuLink;
            for (var i = 0; i < lstallMenuLink.length ; i++) {
                if (lstallMenuLink[i].id == id && lstallMenuLink[i].items.length > 0) {
                    
                    for (var j = 0; j < lstallMenuLink[i].items.length ; j++) {
                        for (var k = 0; k < lstMenuFunction.length ; k++) {
                            if (lstallMenuLink[i].items[j].id == lstMenuFunction[k].PageId || lstallMenuLink[i].items[j].parentid == lstMenuFunction[k].PageId) {
                                lsttemp.push(lstMenuFunction[k]);
                            }
                        }
                    }
                    
                }
            }
            for (var i = lstMenuFunction.length - 1; i >= 0; i--) {
                for (var j = 0; j < lsttemp.length; j++) {
                    if (lstMenuFunction[i] && (lstMenuFunction[i].PageId == lsttemp[j].PageId)) {
                        lstMenuFunction.splice(i, 1);
                    }
                }
            }

            for (var j = 0; j < $scope.lstMenuFunction.length; j++) {
                if ($scope.lstMenuFunction[j].FunctionId == 1 && $scope.lstMenuFunction[j].PageId == id) {
                    $scope.lstMenuFunction.splice(j, 1);
                }
                if ($scope.lstMenuFunction[j].FunctionId == 2 && $scope.lstMenuFunction[j].PageId == id) {
                    $scope.lstMenuFunction.splice(j, 1);
                }
                if ($scope.lstMenuFunction[j].FunctionId == 3 && $scope.lstMenuFunction[j].PageId == id) {
                    $scope.lstMenuFunction.splice(j, 1);
                }
                if ($scope.lstMenuFunction[j].FunctionId == 4 && $scope.lstMenuFunction[j].PageId == id) {
                    $scope.lstMenuFunction.splice(j, 1);
                }
            }
        }
    };

    $scope.CheckShowFunction = function(Idfunction,page)
    {
        for(var i = 0 ; i <  $scope.lstallMenuLink.length ; i++)
        {
            if ($scope.lstallMenuLink[i].id == page.parentid && $scope.lstallMenuLink[i].items.length > 0)
            {
                for(var j = 0 ; j < $scope.lstallMenuLink[i].items.length;j++)
                {
                    if($scope.lstallMenuLink[i].items[j].id == page.id)
                    {
                        for(var f = 0; f < $scope.lstallMenuLink[i].items[j].functions.length;f++)
                        {
                            if($scope.lstallMenuLink[i].items[j].functions[f] == Idfunction)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }
        }
    }

    $scope.CheckShowLinkFunction = function (Idfunction, page) {
        for (var i = 0 ; i < $scope.lstallMenuLink.length ; i++) {
            if ($scope.lstallMenuLink[i].id == page.id) {
                for (var f = 0; f < $scope.lstallMenuLink[i].functions.length; f++) {
                    if ($scope.lstallMenuLink[i].functions[f] == Idfunction) {
                        return true;
                    }
                }
                return false;
            }
        }
    }
    $scope.setLinkselect = function (item) {
        var func = $("#menurole" + item.id).find("div").find("input");
        var check = document.getElementById("subselect" + item.id);
        if (check.checked) {
            for (var i = 0; i < func.length; i++) {
                var cb = func[i];
                var fucid = func[i].id;
                if (fucid.length > 1) {
                    fucid = parseInt(fucid.substr(2, 1));
                }
                $(cb).prop('checked', true);
                $scope.lstMenuFunction.push({ "PageId": item.id, "FunctionId": fucid });
            }

            $("#links" + item.parentid).attr("checked", "checked");
            var checkparent = false;
            for (var j = 0; j < $scope.lstMenuFunction.length; j++) {
                if ($scope.lstMenuFunction[j].PageId == item.parentid) {
                    checkparent = true;
                }
            }
            if (!checkparent) {
                $scope.lstMenuFunction.push({ "PageId": item.parentid, "FunctionId": 1 });
            }
        }
        else {
            for (var i = 0; i < func.length; i++) {
                var cb = func[i];
                var fucid = func[i].id;
                if (fucid.length > 1) {
                    fucid = parseInt(fucid.substr(2, 1));
                }
                $(cb).prop('checked', false);
                for (var k = 0; j < $scope.lstMenuFunction.length; k++) {
                    if ($scope.lstMenuFunction[k].FunctionId == fucid && $scope.lstMenuFunction[k].PageId == id) {
                        $scope.lstMenuFunction.splice(k, 1);
                        break;
                    }
                }

                for (var l = 0 ; l < $scope.lstallMenuLink.length; l++) {
                    if ($scope.lstallMenuLink[l].id == item.parentid && $scope.lstallMenuLink[l].items.length > 0) {
                        var Uncheck = true;
                        for (var t = 0; t < $scope.lstallMenuLink[l].items; t++) {
                            for (var g = 0 ; g < $scope.lstMenuFunction.length ; g++) {
                                if ($scope.lstallMenuLink[l].items[t] == $scope.lstMenuFunction[g].PageId) {
                                    Uncheck = true;
                                }
                            }

                        }
                        if (Uncheck) {
                            for (var c = 0 ; c < $scope.lstMenuFunction.length ; c++) {
                                if ($scope.lstallMenuLink[c].PageId == item.parentid) {
                                    $scope.lstallMenuLink.splice(c, 1);
                                }
                            }
                        }
                    }
                }

            }
        }
    };

    $scope.setFunction = function (linkid, funcid) {
        var inputchild = $("#menurole" + linkid).find("div").find("input");
        var chk = document.getElementById('subselect' + linkid);
        var chkfc = document.getElementById('fc' + funcid);
        var tt = false;
        var chkval = false;
        for (var i = 0; i < inputchild.length; i++) {
            var check = inputchild[i].checked;
            //
            if (check) {
                tt = true;
                break;
            }
        }
        if (tt) {
            chk.checked = true;
        } else {
            chk.checked = false;
        }

        for (var i = 0; i < inputchild.length; i++) {
            var cck = "input#" + inputchild[i].id;
            var cckval = inputchild[i].checked;
            var fcidval = "input#fc" + funcid;
            if (cck == fcidval) {
                chkval = cckval;
                break;
            }
        }
        //push list
        if (chkval) {
            $scope.lstMenuFunction.push({ 'PageId': linkid, 'FunctionId': funcid });
        } else {
            for (var j = 0; j < $scope.lstMenuFunction.length; j++) {
                if ($scope.lstMenuFunction[j].FunctionId == funcid && $scope.lstMenuFunction[j].PageId == linkid) {
                    $scope.lstMenuFunction.splice(j, 1);
                    break;
                }
            }
        }
    };

    $scope.setLinkFunction = function (linkid, funcid) {
        var inputchild = $("#menulinkrole" + linkid).find("span").find("input");
        var chk = document.getElementById('linkselect' + linkid);
        var chkfc = document.getElementById('fc' + funcid);
        var tt = false;
        var chkval = false;
        for (var i = 0; i < inputchild.length; i++) {
            var check = inputchild[i].checked;
            //
            if (check) {
                tt = true;
                break;
            }
        }
        if (tt) {
            chk.checked = true;
        } else {
            chk.checked = false;
        }

        for (var i = 0; i < inputchild.length; i++) {
            var cck = "input#" + inputchild[i].id;
            var cckval = inputchild[i].checked;
            var fcidval = "input#fc" + funcid;
            if (cck == fcidval) {
                chkval = cckval;
                break;
            }
        }
        //push list
        if (chkval) {
            $scope.lstMenuFunction.push({ 'PageId': linkid, 'FunctionId': funcid });
        } else {
            for (var j = 0; j < $scope.lstMenuFunction.length; j++) {
                if ($scope.lstMenuFunction[j].FunctionId == funcid && $scope.lstMenuFunction[j].PageId == linkid) {
                    $scope.lstMenuFunction.splice(j, 1);
                    break;
                }
            }
        }
    };

    $scope.setLinkchecked = function (id) {
        if ($scope.lstMenuFunction == null) {
            return;
        }
        var i = 0;
        var checked = false;
        for (i ; i < $scope.lstMenuFunction.length ; i++) {
            if (id == $scope.lstMenuFunction[i].PageId) {
                checked = true;
                return checked;
            }
        }
        return checked;
    };

    $scope.setFunctionchecked = function (menuid, funcid) {
        if ($scope.lstMenuFunction == null) {
            return;
        }
        var i = 0;
        var checked = false;
        for (i ; i < $scope.lstMenuFunction.length ; i++) {
            if (funcid == $scope.lstMenuFunction[i].FunctionId && $scope.lstMenuFunction[i].PageId == menuid) {
                checked = true;
                return checked;
            }
        }
        return checked;
    };

    $scope.SaveMenuLinkFunction = function (id) {

        if ($scope.lstMenuFunction != undefined) {

            $http({
                url: "/Roles/addPageFunction",
                method: "POST",
                data: { data: $scope.lstMenuFunction, roleId: id },
                dataType: "json"
            })
           .success(function (result) {
               if (result == true) {
                   $("#SetFunc").modal("hide");
                   $.notify("Phân quyền thành công!", "success");
                   init();
               } else {
                   $.notify("Phân quyền thất bại!", "error");
               }
           });
        }
        else {
            $("#SetFunc").modal("hide");
            $.notify("Dữ liệu trống!", "error");
        }
    };

    $scope.countUserPages = function (id) {
        crudService.get("/Roles/countUserPagebyRoles?id=", id)
            .success(function(result) {
                $("#sochucnang" + id).html(result.CountPageRoles);
                $("#songuoi" + id).html(result.CountUserRoles);
            });
    };
    $scope.CheckvalidateNullthis = function (infothis) {
        var idcheck = "#" + event.target.id;
        var idnoti = "#lbl" + event.target.id;
        if ($(idcheck).val() == "") {
            $(idnoti).html('Trường bắt buộc không được để trống!');
            $(idnoti).show();
        }
        else {
            $(idnoti).hide();
        }
        if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\]/gi.test(ten.value) == true) {
            $("#lblName").show();
            lblName.textContent = "Tên nhóm quyền không chứa ký tự đặc biệt!";
            ten.focus();
        }

    };

    $scope.activepage = function (page) {
        if (page == $scope.currentPage) {
            return { color: "white", background: "#f07030" }
        }
    }
    $scope.EntnkyNQ = function (id) {
        $("#" + id + "").focus();
    };
});