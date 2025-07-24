angular.module('myApp').requires.push('kendo.directives');
myApp.controller("DepartmentCtrl",
function ($scope, $location, $routeParams, crudService, $http, $cookieStore, $timeout) {
    $scope.lstData = [];
    $scope.lstBase = [];
    $scope.lstGroupMail = [];
    $scope.Phongban = {};
    $scope.ID = '';
    $scope.Name = '';
    $scope.lstselect = [];
    $scope.lstDepartmentGetAllLevel = [];
    $scope.skinIndex = false;
    $scope.lstRoles = [];
    $scope.lstSelectRoles = [];
    $scope.Phongban.Levels = 0;

    $scope.options = {
        checkboxes: {
            checkChildren: true
        }
    }
    var lblten = document.getElementById("lblName");
    var ten = document.getElementById("Name");
    // get all Departments
    var getData = function () {
        return $http({
            url: "/Departments/GetAll",
            method: "GET",
            params: {}
        });
    };
    // Clear Data
    var clearData = function () {
        $scope.Phongban = {};
        $("#lblCodeDep").hide();
        $("#lblName").hide();
        $("#lblemailerror").hide();
        $("#lblpassemailerror").hide();
        $("#treeViewFolder .k-checkbox-wrapper input").prop("checked", false).trigger("change");
    };
    // init
    var init = function () {
        getData()
            .success(function (data) {
                var items = [];
                var source = [];
                for (var i = 0; i < data.length ; i++) {
                    //set gia tri cho item
                    var itemdata = data[i];
                    var text = itemdata["Name"];
                    var parentid = itemdata["ParentId"];
                    var id = itemdata["Id"];
                    document.getElementById("TrangThai").checked = true;
                    $scope.Phongban.Status = true;
                    if (items[parentid]) {
                        var item = { id: id, parentid: parentid, text: text, items: [] };
                        if (!items[parentid].items) {
                            items[parentid].items = [];
                        }
                        items[parentid].items[items[parentid].items.length] = item;
                        items[id] = item;

                    }
                    else {
                        items[id] = { id: id, parentid: parentid, text: text, items: [] };
                        source.push(items[id]);
                    }
                }
                $scope.lstData = source;
                // Config option cho treeview
                $scope.optionTree = {
                    loadOnDemand: false
                }

                $scope.lstBase = data;
                $scope.Phongban.ParentId = 0;
            }).error(function (error) {
                $.notify("Lỗi khi lấy dữ liệu!", "error");
            });
        crudService.get("/Departments/getListEdit?Id=", -1)
        .success(function (data) {
            $scope.lstDepartmentGetAllLevel = data;
        });
    }
    init();
    // Ham convert date
    var parseDate = function (value) {
        if (value) {
            return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
        }
        return null;
    }
    // Hàm Create
    $scope.create = function (data) {
        $http({
            url: "/Departments/Create",
            method: "POST",
            data: { model: data, lstDepRoles: $scope.lstSelectRoles },
            dataType: "json"
        })
        .success(function (data) {
            $.notify("Tạo mới thành công!", "success");
            $("#lblName").hide();
            $("#lblchucvu").hide();
            document.getElementById("TrangThai").checked = true;
            $scope.Phongban.Status = true;
            clearData();
            init();
            $scope.checkfunction(data.Id, data.ParentId);
            setExtendTreeById(data.Id);
        })
        .error(function (error) {
            $("#lblName").hide();
            $("#lblchucvu").hide();
            $.notify("Tạo mới thất bại!", "error");
        });
    }
     
    // get roles
    var getCheckboxRole = function () {
        $http({
            url: "/Roles/getAllcheckbox",
            method: "GET",
            params: {}
        }).success(function (data) {
            $scope.lstRoles = data;
        });
    };
    getCheckboxRole();
    // Get Data By ID
    $scope.getbyID = function (id) {
        $scope.lstSelectRoles = [];
        crudService.get("/Departments/GetById?id=", id)
         .success(function (data) {
             $scope.Phongban = data;
             $scope.Name = data.Name;
             $scope.OldCode = data.Code;
             $scope.oldName = data.Name;
             document.getElementById("CodeDep").focus();
         })
         .error(function (error) {
             $.notify("Lỗi khi lấy dữ liệu!", "error");
         });
    }
    // get rolde theo phong ban
    var getRolebyDepId = function (id) {
        $http({
            url: "/Roles/getRolebyDepId",
            method: "GET",
            params: {
                id: id
            }
        }).success(function (data) {
            $scope.lstSelectRoles = data;
        });
    }
    // selected roles
    $scope.checkedRole = function (id) {
        var i = 0;
        var checked = false;
        for (i; i < $scope.lstSelectRoles.length; i++) {
            if (id == $scope.lstSelectRoles[i].RoleId) {
                checked = true;
                return checked;
            }
        }
        return checked;
    }
    //Check Create Update
    $scope.checkfunction = function (id, pid) {
        $("div.sub-div-menu").hide();
        clearData();
        $scope.clicked = true;
        if (id != -1) {
            document.getElementById("btnluuDepartment").innerHTML = '<i class="fa fa-floppy-o"></i> Cập nhật';
            $scope.getbyID(id);
        }
        else {
            document.getElementById("btnluuDepartment").innerHTML = '<i class="fa fa-floppy-o"></i> Lưu lại';
            $scope.Phongban.ParentId = pid;
            $scope.Phongban.Id = null;
            $scope.Phongban.Name = "";
            document.getElementById("TrangThai").checked = true;
            $scope.Phongban.Status = true;
            getRolebyDepId(0); 
            document.getElementById("CodeDep").focus();
        }
    };
    // Hàm Update 
    $scope.update = function (data) {
        $http({
            url: "/Departments/Update",
            method: "POST",
            data: { model: data, lstDepRoles: $scope.lstSelectRoles},
            dataType: "json"
        })
    .success(function (result) {
        $.notify("Cập nhật thành công!", "success");
        $("#lblName").hide();
        $("#lblchucvu").hide();
        clearData();
        init();
        $scope.getbyID(result.Id);
    })
    .error(function (error) {
        $("#lblName").hide();
        $("#lblchucvu").hide();
        $.notify("Lỗi cập nhật!", "error");
    });
    }
    // validate create form
    var validate = function (data) {
        var str = $("#CodeDep").val();
        var patt = new RegExp("[^\u0000-\u007F]+");
        var res = patt.test(str);

        if (data == null) {
            $("#lblName").show();
            $("#lblName").html("Trường bắt buộc không được để trống!");
            $("#Name").focus();
            return true;
        } else if (res) {
            $("#lblCodeDep").html('Trường mã không được nhập tiếng Việt có dấu!');
            $("#lblCodeDep").show();
            $("#lblCodeDep").focus();
            return true;
        }
        else {
            if (data.Code == null || data.Code == "") {
                $("#lblCodeDep").html("Trường bắt buộc không được để trống!");
                $("#lblCodeDep").show();
                $("#CodeDep").focus();
                return true;
            }
            else {
                if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.Code)) {
                    $("#lblCodeDep").html("Mã đơn vị không chứa ký tự đặc biệt!");
                    $("#lblCodeDep").show();
                    $("#CodeDep").focus();
                    return true;
                }
                else {
                    for (var i = 0; i < $scope.lstBase.length; i++) {
                        if (data.Code.toUpperCase() == $scope.lstBase[i].Code.toUpperCase()) {
                            $("#lblCodeDep").html("Mã đơn vị đã tồn tại trong hệ thống!");
                            $("#lblCodeDep").show();
                            $("#CodeDep").focus();
                            return true;
                        }
                    }
                    $("#lblCodeDep").hide();
                }
            }
            if (data.Name == null || data.Name == "") {
                $("#lblName").show();
                $("#lblName").html("Trường bắt buộc không được để trống!");
                $("#Name").focus();
                return true;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.Name) == true) {
                $("#lblName").show();
                $("#lblName").html("Tên đơn vị không chứa ký tự đặc biệt!");
                $("#Name").focus();
                return true;
            }
        }
        return false;
    };
    // validate update form
    var validateUpdate = function (data) {
        if (data == null) {
            $("#lblCodeDep").show();
            $("#lblCodeDep").html("Trường bắt buộc không được để trống!");
            $("#Name").focus();
            return true;
        }
        else {
            if (data.Code == null || data.Code == "" || data.Code == undefined) {
                $("#lblCodeDep").html("Trường bắt buộc không được để trống!");
                $("#lblCodeDep").show();
                $("#CodeDep").focus();
                return true;
            }
            else {
                if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.Code)) {
                    $("#lblCodeDep").html("Mã đơn vị không chứa ký tự đặc biệt!");
                    $("#lblCodeDep").show();
                    $("#CodeDep").focus();
                    return true;
                }
                else {
                    if (data.Code != $scope.OldCode) {
                        for (var i = 0; i < $scope.lstBase.length; i++) {
                            if (data.Code.toUpperCase() == $scope.lstBase[i].Code.toUpperCase()) {
                                $("#lblCodeDep").html("Mã đơn vị đã tồn tại trong hệ thống!");
                                $("#lblCodeDep").show();
                                $("#CodeDep").focus();
                                return true;
                            }
                        }
                    }
                    $("#lblCodeDep").hide();
                }
            }
            if (data.Name == null || data.Name == "" || data.Name == undefined) {
                $("#lblName").show();
                $("#lblName").html("Trường bắt buộc không được để trống!");
                ten.focus();
                return true;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.Name) == true) {
                $("#lblName").show();
                $("#lblName").html("Tên đơn vị không chứa ký tự đặc biệt!");
                ten.focus();
                return true;
            }
        }
        return false;
    };
    // kiểm tra tên trong CSDL
    var checkDepname = function (name) {
        return $http({
            url: "/Departments/checkDepartmentName",
            method: "GET",
            params: {
                name: name
            }
        });
    }
    var selectRoles = function () {
        var dt = $scope.lstRoles;
        $scope.lstSelectRoles = [];
        for (var i = 0; i < dt.length; i++) {
            var id = dt[i].Id;
            var chk = $("#roles" + id).is(":checked");
            if (chk) {
                $scope.lstSelectRoles.push({ "RoleId": dt[i].Id, "IsDefault": 1 });
            }
        }
    };
    // Click Save
    $scope.preCreate = function (data) {
        var btnupdate = '<i class="fa fa-floppy-o"></i> Cập nhật';
        selectRoles();
        if (document.getElementById("btnluuDepartment").innerHTML == '<i class="fa fa-floppy-o"></i> Lưu lại') {
            if (validate(data) == true)
            { return; }
            if ($("#TrangThai").is(":checked")) {
                data.Status = true;
            }
            else {
                data.Status = false;
            }
            if (data.ParentId == null || data.ParentId == undefined) {
                data.ParentId = 0;
            }
            if (data.Levels == null || data.Levels == undefined) {
                data.Levels = 0;
            }
            var checkName = false;
            checkDepname(data.Name).success(function (res) {
                if (res > 0) {
                    checkName = false;
                }
                else {
                    checkName = true;
                }
            });
            $timeout(function() {
                if (checkName) {
                    $("#lblName").hide();
                    $scope.create(data);
                    setExtendTreeById(data.Id);
                } else if (!checkName) {
                    $("#lblName").html("Đơn vị đã tồn tại trong đơn vị này!");
                    $("#lblName").show();
                    $("#Name").focus();
                }
            }, 700);

        }
        else if (document.getElementById("btnluuDepartment").innerHTML == btnupdate) {
            if (validateUpdate(data) == true) { return; }
            if (data.Name == $scope.oldName) {
                $("#lbltieude").hide();
                var statusParent = true;
                if (data.ParentId != 0) {
                    for (var i = 0; i < $scope.lstBase.length; i++) {
                        if ($scope.lstBase[i].Id == data.ParentId) {
                            if ($scope.lstBase[i].Status == false) {
                                statusParent = false;
                                break;
                            } else {
                                statusParent = true;
                                break;
                            }
                        }
                    }
                    if (statusParent) {
                        $scope.update(data);
                        setExtendTreeById(data.Id);
                    } else {
                        $.notify("Không thể đổi trạng thái khi cấp trên đang khóa!", "error");
                    }
                } else {
                    $scope.update(data);
                    setExtendTreeById(data.Id);
                }
                return;
            }
            var checkName = false;
        }

        if (data.Name != $scope.oldName) {
            checkDepname(data.Name).success(function (res) {
                if (res > 0) {
                    checkName = false;
                }
                else {
                    checkName = true;
                }
            });
        }
        else {
            checkName = true;
        }
        $timeout(function() {
            if (checkName) {
                $("#lbltieude").hide();
                var statusParent = true;
                if (data.ParentId != 0) {
                    for (var i = 0; i < $scope.lstBase.length; i++) {
                        if ($scope.lstBase[i].Id == data.ParentId) {
                            if ($scope.lstBase[i].Status == false) {
                                statusParent = false;
                                break;
                            } else {
                                statusParent = true;
                                break;
                            }
                        }
                    }

                    if (statusParent) {
                        $scope.update(data);
                        setExtendTreeById(data.Id);
                    } else {
                        $.notify("Không thể đổi trạng thái khi cấp trên đang khóa!", "error");
                    }
                    return;
                } else {
                    $scope.update(data);
                    setExtendTreeById(data.Id);
                    return;
                }
            } else if (!checkName) {
                $("#lblName").html("Đơn vị đã tồn tại trong đơn vị này!");
                $("#lblName").show();
                $("#Name").focus();
            } 
        }, 700);
    };
    // cập nhật trạng thái thu mục khi có thư mục con
    $scope.UpdateStatusDisable = function (data) {
        if (data.Name != $scope.oldName) {
            checkDepname(data.Name).success(function (res) {
                if (res > 0) {
                    $("#lblName").html("Đơn vị đã tồn tại trong đơn vị này!");
                    $("#lblName").show();
                }
                else {
                    $("#lblName").hide();
                    $("#confirm-updatestatus-folder").modal("hide");
                    $scope.update(data);
                    setExtendTreeById(data.Id);
                }
            });
        }
        else {
            $("#confirm-updatestatus-folder").modal("hide");
            $scope.update(data);
            setExtendTreeById(data.Id);
        }
    }
    // Hàm Delete
    $scope.delete = function (id) {
        // Điều kiện để được phép xóa
        crudService.get("/Departments/countContent?id=", id).success(
            function (resut) {
                if (resut != null) {
                    if (resut.RolesCount > 0 || resut.DepChild > 0) {
                        $("#confirm-delete-deparment").modal("hide");
                        if (resut.RolesCount > 0) {
                            $.notify("Đơn vị còn tồn tại nhóm quyền liên quan!", "error");
                        }
                        if (resut.DepChild > 0) {
                            $.notify("Đơn vị còn tồn tại đơn vị con!", "error");
                        }
                        return;
                    }
                    else {
                        crudService.remove("/Departments/Delete", id)
                                .success(function () {
                                    $("#confirm-delete-deparment").modal("hide");
                                    $.notify("Xóa thành công!", "success");
                                    clearData();
                                    $scope.checkfunction(-1, 0);
                                    init();
                                    setExtendTreeById();
                                })
                .error(function (error) {
                    $("#confirm-delete-deparment").modal("hide");
                    $.notify("Xóa thất bại!", "error");
                });
                    }
                }

            }).error(function (error) {
                $("#confirm-delete-deparment").modal("hide");
                $.notify("Xóa thất bại!", "error");
            });
    };
    $scope.collapseAll = function () {
        if ($('#btcollTree').val() == "Thu nhỏ") {
            $scope.tree.collapse(".k-item");
            $('#btcollTree').val('Phóng to');
            $scope.skinIndex = false;
        }
        else if ($('#btcollTree').val() == "Phóng to") {
            $scope.tree.expand(".k-item");
            $('#btcollTree').val('Thu nhỏ');
            $scope.skinIndex = true;
        }
    };
    // Thêm phòng ban cùng cấp
    $scope.addAfter = function (item) {
        $timeout(function() {
            $scope.Phongban.Levels = 0;
            $('#ddlLevels').val(0);
        }, 500);
        $scope.lstSelectRoles = [];
        var array = item.parent();
        var index = array.indexOf(item);
        var newItem = { id: -1, text: "Phòng ban mới", parentid: item.parentid, selected: true };
        array.splice(index + 1, 0, newItem);
        $scope.checkfunction(-1, item.parentid);
    };
    //Thêm phòng ban cấp con
    $scope.addBelow = function (item, parentid) {
        $timeout(function () {
            $scope.Phongban.Levels = 0;
            $('#ddlLevels').val(0);
        }, 500);
        $scope.lstSelectRoles = [];
        crudService.get("/Departments/GetById?id=", parentid)
         .success(function (data) {
             if (data.Status == false) {
                 $.notify("Đơn vị đã bị khóa, không thể thêm!", "error");
             } else {
                 var newItem = { id: -1, text: "Đơn vị mới", parentid: parentid, selected: true };
                 $scope.tree.append(newItem, item);
                 $scope.checkfunction(-1, parentid);
             }
         })
         .error(function (error) {
             $.notify("Lỗi khi kiểm tra hàm!", "error");
         });
    };
    // End
    $scope.removes = function (id, Ten, scope) {
        if (id != -1) {
            $scope.ID = id;
            $("#nameDepartment").html(Ten);
            $("#confirm-delete-deparment").modal("show");
        }
        else {
            var Array = scope.parent();
            var index = Array.indexOf(scope);
            Array.splice(index, 1);
            $("#confirm-delete-deparment").modal("hide");

        }
    }
    var getPageId = function () {
        var pathName = window.location.pathname;
        pathName = pathName.substr(1);
        crudService.get("/Pages/GetPagesIDbyLink?path=", pathName)
            .success(function (result) {
                $scope.PageId = result;
            });
    }
    getPageId();
    // Sửa/Thêm
    $scope.CheckAddEdit = function (roleid) {
        if ($cookieStore.get("SuperUser") != undefined && $cookieStore.get("SuperUser") != "") {
            return true;
        } else {
            var lstrole = $cookieStore.get("ListFuction");
            if (lstrole.length > 0) {
                for (var i = 0; i < lstrole.length; i++) {
                    if ($scope.PageId == lstrole[i]["PageId"]) {
                        var roleadd = lstrole[i]["FunctionId"];
                        return roleadd.includes(roleid);
                    }
                }
                if ($scope.stop == undefined && $scope.PageId != undefined) {
                    window.location.href = "/Home/Index";
                    $scope.stop = 1;
                    return;
                }
            }
        }
    }
    $scope.EntnkyDP = function (id) {
        $("#" + id + "").focus();
    }
    $scope.SetActive = function (id) {
        if (id == $scope.Phongban.ID) {
            return { color: "red" }
        }
        else
            return "";
    }
    kendo.ui.TreeView.prototype.getCheckedItems = (function () {
        function getCheckedItems() {
            var nodes = this.dataSource.view();
            return getCheckedNodes(nodes);
        }
        function getCheckedNodes(nodes) {
            var node, childCheckedNodes;
            var checkedNodes = [];

            for (var i = 0; i < nodes.length; i++) {
                node = nodes[i];
                if (node.checked) {
                    checkedNodes.push({ "FolderId": node.id });
                }
                if (node.hasChildren) {
                    childCheckedNodes = getCheckedNodes(node.children.view());
                    if (childCheckedNodes.length > 0) {
                        checkedNodes = checkedNodes.concat(childCheckedNodes);
                    }
                }
            }
            return checkedNodes;
        }
        return getCheckedItems;
    })();
    //Check trống
    $scope.CheckvalidateNullthis = function (infothis) {
        var idcheck = "#" + event.target.id;
        var idnoti = "#lbl" + event.target.id;

        if (event.target.id == 'CodeDep') {
            var str = $("#CodeDep").val();
            var patt = new RegExp("[^\u0000-\u007F]+");
            var res = patt.test(str);

            if ($(idcheck).val() == "") {
                $(idnoti).html('Trường bắt buộc không được để trống!');
                $(idnoti).show();
            } else if (res) {
                $(idnoti).html('Trường mã không được nhập tiếng Việt có dấu!');
                $(idnoti).show();
            } else
                $(idnoti).hide();
        } else {
            if ($(idcheck).val() == "") {
                $(idnoti).html('Trường bắt buộc không được để trống!');
                $(idnoti).show();
            } else {
                $(idnoti).hide();
            }
        }
    };
    var expandAndSelectNode = function (id, treeViewName) {
        var treeView = $(treeViewName).data("kendoTreeView");
        var item = $(treeViewName).find("li[data-id='" + id + "']").find(".k-in");
        var ktree = $(item).parentsUntil('.k-treeview');
        ktree.filter('.k-item').each(
                    function (index, element) {
                        $(treeViewName).data('kendoTreeView').expand($(this));
                    }
                );
        var nodeDataItem = treeView.dataSource.get(id);
        var node = treeView.findByUid(nodeDataItem.uid);
        treeView.select(node);
    }
    var setExtendTreeById = function (Id) {
        $timeout(function() {
            if (Id != undefined) {
                expandAndSelectNode(Id, '#treeView');
            }
            $scope.tree.expand(".k-item");
            $('#btcollTree').val('Thu nhỏ');
            $scope.skinIndex = true;
        }, 700);
    }
    $scope.searchSuggestAPI = function (userInputString, timeoutPromise) {
        return $http.post('/Departments/getSuggestDepartment', { keyword: userInputString }, { timeout: timeoutPromise });
    };
});
