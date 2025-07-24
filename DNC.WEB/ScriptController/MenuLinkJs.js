myApp.controller("MenuLinkJs",
function ($location, $scope, $routeParams, crudService, $http, $cookieStore) {
    $scope.lstData = [];
    $scope.lstBase = [];
    $scope.lstRoles = [];
    $scope.Menulink = {};
    $scope.ID = "";
    $scope.Name = "";
    $scope.lstselect = [];
    $scope.order = 1;
    $scope.lstRoleSelected = [];
    $scope.lstFunctions = [];

    // danh sách function lấy từ db
    $scope.lstFunctionSelected = [];

    // danh sách function được chọn
    $scope.lstFunctionSelect = [];

    $scope.lstRoleFunction = [];
    $scope.lstDirectoryLink = [];
    $scope.lstMenuLinkGetAllLevel = [];
    $scope.mId = -1;
    $scope.lstPutCookie = [];
    $scope.lstRoleID = "";
    $scope.skinIndex = true;

    $scope.pageId = "";
    $scope.functionId = "";

    var lblten = document.getElementById("lblTen");
    var ten = document.getElementById("lbName");
    // Phân Quyền Cho Link
    var menulinkFucRol = function (userID, lstRole) {
        return $http({
            url: "/MenuLink/GetMenuLinkFucRole",
            method: "GET",
            params: {
                userID: userID,
                roleID: lstRole
            }
        });
    }

    var checkRoleFuction = function () {
        var listrole = [];
        var func = $cookieStore.get("ListFuction");
        if (func != null) {
            return;
        }
        var userID = $cookieStore.get('UserID');
        crudService.get("/UserRole/UserRole_GetByUserId?id=", userID)
        .success(function (res) {
            angular.forEach(res, function (item) {

                listrole.push(item['RoleID']);
                $scope.lstRoleID = listrole.toString();

            })
            menulinkFucRol(userID, $scope.lstRoleID)
           .success(function (result) {
               $cookieStore.put("ListFuction", result);

           })
        })
    }
    //checkRoleFuction();


    var getData = function () {
        return $http({
            url: "/MenuLink/GetAll",
            method: "GET",
            params: {}
        });
    }
    // Get Function by PageId
    var getFunction = function(id)
    {
        crudService.get("/Function/GetByPageId?PageId=", id)
       .success(function (data) {
           $scope.lstFunctionSelected = [];
           $scope.lstFunctionSelect = [];
           //debugger;
           if (data.length > 0) {
               $scope.lstFunctionSelected = data;
               $scope.functionId = data.FunctionId;

               $scope.lstFunctionSelect = [];
               var result = $scope.lstFunctionSelected[0].FunctionId.split(';');
               if (result.length > 0) {
                   for (i = 0; i < result.length; i++) {

                       $scope.lstFunctionSelect.push({ 'PageId': id, 'FunctionId': result[i] });
                   }
               }
           }
       });
    }

    var init = function () {
        getData()
        .success(function (data) {
            var items = [];
            var source = [];
           
            for (var i = 0; i < data.length ; i++) {
                //set gia tri cho item
                var itemdata = data[i];
                var name = itemdata["Name"];
                var parentid = itemdata["ParentId"];
                var id = itemdata["Id"];
                var css = itemdata["CssClass"];
                var link = itemdata["Link"];
                var metatitle = itemdata["Metatitle"];
                var target = itemdata["Target"];
                //document.getElementById("TrangThai").checked = true;

                if (items[parentid]) {
                    var item = { id: id, parentid: parentid, name: name, css: css, link: link, metatitle: metatitle, target: target, items: [] };
                    if (!items[parentid].items) {
                        items[parentid].items = [];
                    }
                    items[parentid].items[items[parentid].items.length] = item;
                    items[id] = item;

                }
                else {
                    items[id] = { id: id, parentid: parentid, name: name, css: css, link: link, metatitle: metatitle, target: target, items: [] };
                    source.push(items[id]);
                }
            }
           
            $scope.lstData = source;
            $scope.lstBase = data;

            //$scope.Menulink.IsLocked = true;
            //$scope.Menulink.OrderNo = 1;
            //$scope.Menulink.ParentId = 0;

        })
            .error(function (error) {
                $.notify("Lỗi khi lấy dữ liệu!", "error");
            });

        //crudService.getAll("/Role/GetAll")
        //.success(function (data) {
        //    $scope.lstRoles = data;
        //});

        crudService.getAll("/Function/GetAll")
        .success(function (data) {
            $scope.lstFunctions = data;
        });



        //crudService.getAll("/MenuLink/DirectoryLink")
        //.success(function (data) {
        //    $scope.lstDirectoryLink = data;
        //});

        crudService.getAll("/MenuLink/GetAllLevel")
        .success(function (data) {
            $scope.lstMenuLinkGetAllLevel = data;
        });
    }
    var Clear = function()
    {
        $scope.pageId = "";
        $scope.Menulink.IsLocked = true;
        $scope.Menulink.Order = 1;
        $scope.Menulink.ParentID = 0;
        $scope.Menulink.Name = "";
        $scope.Menulink.Url = "";
        $scope.Menulink.CssClass = "";
        $scope.Menulink.Description = "";
        $scope.lstFunctionSelected = [];
        $scope.lstFunctionSelect = [];

    }
    // Change URL
    $scope.changeUrl = function () {
        var path = $("#lbName").val();
        crudService.get("/MenuLink/GenUrl?path=", path)
        .success(function (data) {
            $scope.Menulink.MetaTitle = data;
        });
    }

    init();
    // Load Functions push to Roles





    // Clear Data
    var ClearData = function () {
        $scope.lstPage = [];
    }

    // Hàm Create
    $scope.create = function (data) {
        debugger;
        crudService.create("/MenuLink/Create", data)
        .success(function (data) {

            $.notify("Tạo mới thành công!", "success");
            //
            debugger;
            var functionList = "";
            if ($scope.lstFunctionSelect.length > 0)  // có chọn quyền
            {
                for (i = 0; i < $scope.lstFunctionSelect.length; i++) {
                    functionList += $scope.lstFunctionSelect[i].FunctionId + ";"
                }

                functionList = functionList.substr(0, functionList.length - 1);
                $scope.dataPageFunction = {};
                $scope.dataPageFunction.PageId = data.Id;
                $scope.dataPageFunction.FunctionId = functionList;

                crudService.create("/Function/CreatePageFunction", $scope.dataPageFunction)
                .success(function (data) {
                       debugger;
                      
                })
                .error(function (error) {
                       //$.notify("Tạo mới thất bại!", "error");
                });
            }
            //
            init();
            $scope.checkfunction(data.Id, data.ParentId)

        })
        .error(function (error) {
            $.notify("Tạo mới thất bại!", "error");
        });
    }
    // Convert datetime
    var parseDate = function (value) {
        if (value) {
            return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
        }
        return null;
    }

    // Get Data By ID
    $scope.getbyID = function (id) {
        debugger
        crudService.get("/MenuLink/GetById?id=", id)
         .success(function (data) {
             $scope.Menulink = data;
             $scope.Name = data.Name;
             $scope.pageId = id;
             getFunction(data.Id);
         })
         .error(function (error) {
             $.notify("Lỗi khi lấy dữ liệu!", "error");
         });

        //$scope.lstRoleFunction = [];
        //var inputchild = $('#AllRole').find("div").find("input");
        //inputchild.prop('checked', false);

    }

    // Load Function checkbox by ID
    $scope.SetCheckFunction = function (id) {
        //debugger
        var i = 0;
        var checked = false;
        if ($scope.lstFunctionSelected.length > 0) {
            var pageid = $scope.lstFunctionSelected[0].PageId;
            var result = $scope.lstFunctionSelected[0].FunctionId.split(';');
            for (i; i < result.length; i++) {
                if (id == result[i]) {
                    checked = true;
                    return checked;
                }
            }
        }
        return checked;
    }
    // Khi check vào function
    $scope.setFunction = function (id) {
        //debugger;
        //if ($scope.pageId != "") {

            var chkfc = document.getElementById('function' + id);
            var check = chkfc.checked;
            //push list
            if (check) {
                $scope.lstFunctionSelect.push({ 'PageId': $scope.pageId, 'FunctionId': id });
            } else {
                for (var i = 0; i < $scope.lstFunctionSelect.length; i++) {
                    if ($scope.lstFunctionSelect[i].FunctionId == id && $scope.lstFunctionSelect[i].PageId == $scope.pageId) {
                        $scope.lstFunctionSelect.splice(i, 1);
                        break;
                    }
                }
            }
        //}
        console.log($scope.lstFunctionSelect);
    }

    //Check Create Update
    $scope.checkfunction = function (id, pid) {
        $("div.sub-div-menu").hide();
        $("#lblTen").hide();
        $scope.Menulink = {};
        if (id != -1) {
            document.getElementById("btnThemmoi").innerHTML = "Cập nhật";
            $scope.getbyID(id);
            document.getElementById("lbName").focus();
        }
        else {
            document.getElementById("btnThemmoi").innerHTML = '<i class="fa fa-floppy-o" aria-hidden="true"></i> Lưu lại';
            $scope.Menulink.ParentId = pid;
            $scope.Menulink.Id = null;
            document.getElementById("lbName").focus();
            $scope.lstRoleFunction = [];
            document.getElementById("TrangThai").checked = true;
            $scope.Menulink.IsLocked = true;

            $scope.lstFunctionSelect = [];
            $scope.lstFunctionSelected = [];

        }
    }
    // Hàm Update 
    $scope.update = function (data) {
        data.CreatedDate = parseDate(data.CreatedDate);
        //deleteMenuFunctionRole(data.ID);
        crudService.update("/MenuLink/Update", data)
        .success(function (result) {
           
          

            var functionList = "";
            if($scope.lstFunctionSelect.length > 0)  // có chọn quyền
            {
                // kiểm tra xem đã phân quyền chưa
                for(i = 0; i< $scope.lstFunctionSelect.length; i++)
                {
                    functionList += $scope.lstFunctionSelect[i].FunctionId + ";"
                }
                functionList = functionList.substr(0, functionList.length - 1);

                // kiểm tra xem đã phân quyền chưa
                crudService.get("/Function/GetByPageId?PageId=", result.Id)
                .success(function (data) {
                    debugger

                    if (data.length > 0) {  // cập nhật
                        if ($scope.functionId != functionList) // nếu có thay đổi so với dữ liệu ban đầu
                        {
                            $http({
                                url: "/Function/UpdatePageFunction",
                                method: "POST",
                                params: { PageId: result.Id, FunctionId: functionList }
                            }).success(function (data) {
                              
                              
                            });
                        }
                    }
                    else {
                       
                        $scope.dataPageFunction = {};
                        $scope.dataPageFunction.PageId = result.Id;
                        $scope.dataPageFunction.FunctionId = functionList;

                        crudService.create("/Function/CreatePageFunction", $scope.dataPageFunction)
                           .success(function (data) {
                            
                           })
                           .error(function (error) {
                              
                           });
                    }

                });

               
            } // xóa quyền cũ nếu không chọn
            else
            {
                crudService.remove("/Function/DeleteByPageId", result.Id)
                .success(function (data) {
                    debugger;
                })
                .error(function (error) {

                });
            }

            $.notify("Cập nhật thành công!", "success");
            init();
            debugger;
            $scope.getbyID(result.Id);
           

        })
        .error(function (error) {
            $.notify("Lỗi cập nhật!", "error");
        });
    }
    var checkExistChucNang = function () {
        var chucnang = $('#lbName').val();
        for (var i = 0; i < $scope.lstBase.length; i++) {

            if (chucnang.toUpperCase() == $scope.lstBase[i].Name.toUpperCase()) {
                $("#lblTen").show();
                lblten.textContent = "Chức năng đã tồn tại. Vui lòng nhập lại!";
                ten.focus();
                return true;
            } else {
                $("#lblTen").hide();
            }
        }
    }
    var checkExistChucNangUpdate = function () {
        var chucnang = $('#lbName').val();
        var chucnangsua = $scope.Name;
        for (var i = 0; i < $scope.lstBase.length; i++) {
            if (chucnangsua.toUpperCase() == chucnang.toUpperCase()) {
                return false;
            } else
                if (chucnang.toUpperCase() == $scope.lstBase[i].Name.toUpperCase()) {
                    $("#lblTen").show();
                    lblten.textContent = "Chức năng đã tồn tại. Vui lòng nhập lại!";
                    ten.focus();
                    return true;
                } else {
                    $("#lblTen").hide();
                }
        }
    }
    // Click Save
    var validate = function (data) {
        if (data == null) {
            $("#lblten").show();
            lblten.textContent = "Tên chức năng không để trống!";
            ten.focus();
            return true;
        }
        else {

            if (data.Name == null || data.Name == "") {
                $("#lblTen").show();
                lblten.textContent = "Tên chức năng không để trống!";
                ten.focus();
                return true;
            } else {
                if (checkExistChucNang(data) == true) {
                    return true;
                }
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.Name) == true) {
                $("#lblTen").show();
                lblten.textContent = "Tên chức năng không chứa ký tự đặc biệt!";
                ten.focus();
                return true;
            }
        }
        return false;
    }
    var validateUpdate = function (data) {
        if (data == null) {
            $("#lblten").show();
            lblten.textContent = "Tên chức năng không để trống!";
            ten.focus();
            return true;
        }
        else {

            if (data.Name == null || data.Name == "") {
                $("#lblTen").show();
                lblten.textContent = "Tên chức năng không để trống!";
                ten.focus();
                return true;
            } else {
                if (checkExistChucNangUpdate(data) == true) {
                    return true;
                }
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.Name) == true) {
                $("#lblTen").show();
                lblten.textContent = "Tên chức năng không chứa ký tự đặc biệt!";
                ten.focus();
                return true;
            }
        }
        return false;
    }
    $scope.preCreate = function (data) {
        var strLink = $('#link>option:selected').val();
        var btn = '<i class="fa fa-floppy-o" aria-hidden="true"></i> Lưu lại';
        //if (strLink == "" || strLink == null) {
        //    data.Link = "#";
        //} else {
        //    data.Link = strLink;
        //}

        if (document.getElementById("btnThemmoi").innerHTML == btn) {
            if (validate(data) == true)
            { return; }
            if (data.ParentId == null || data.ParentId == undefined) {
                data.ParentId = 0;
            }
            $scope.create(data);
            init();
        }
        else if (document.getElementById("btnThemmoi").innerHTML == "Cập nhật") {
            if (validateUpdate(data) == true)
            { return; }
            $scope.update(data);
            init();
        }
    }
    // Hàm Delete
    $scope.remove = function (id) {
        debugger
        if (id != undefined) {
            crudService.remove("/MenuLink/Delete", id)
            .success(function () {
              

                //Xóa chức năng
                crudService.remove("/Function/DeleteByPageId", id)
               .success(function (result) {
                   
               })
               .error(function (error) {

               });
               //
               $("#confirm-delete").modal("hide");
               $.notify("Xóa thành công!", "success");
               init();
               //
               Clear();

               //

            })
            .error(function (error) {
                $("#confirm-delete").modal("hide");
                $.notify("Xóa thất bại!", "error");
            });
        }
       
    }
    $scope.treeOptions = {
        accept: function (sourceNodeScope, destNodesScope, destIndex) {
            return true;
        }
    };
    $scope.collapseAll = function () {
        if ($('#collTree').val() == "Thu nhỏ") {
            $scope.$broadcast('angular-ui-tree:collapse-all');
            $('#collTree').val('Phóng to');
            $scope.skinIndex = false;
        }
        else if ($('#collTree').val() == "Phóng to") {
            $scope.$broadcast('angular-ui-tree:expand-all');
            $('#collTree').val('Thu nhỏ');
            $scope.skinIndex = true;
        }
    };


    $scope.expandAll = function () {
        $scope.$broadcast('angular-ui-tree:expand-all');
    };

    //Set gia tri Role tu form
    $scope.setRoleSelected = function (id) {
        var inputchild = $('#role' + id).find("div").find("input");
        var chk = document.getElementById('roleselect' + id);
        if (chk.checked) {
            for (var i = 0; i < inputchild.length; i++) {
                var inp = inputchild[i];
                var fcid = inputchild[i].id;
                if (fcid.length > 1) {
                    fcid = parseInt(fcid.substr(2, 1));
                }
                $(inp).prop('checked', true);
                $scope.lstRoleFunction.push({ 'RoleID': id, 'FunctionID': fcid });
            }
        } else {
            for (var i = 0; i < inputchild.length; i++) {
                var inp = inputchild[i];
                var fcid = inputchild[i].id;
                if (fcid.length > 1) {
                    fcid = parseInt(fcid.substr(2, 1));
                }
                $(inp).prop('checked', false);

                for (var j = 0; j < $scope.lstRoleFunction.length; j++) {
                    if ($scope.lstRoleFunction[j].FunctionID == fcid && $scope.lstRoleFunction[j].RoleID == id) {
                        $scope.lstRoleFunction.splice(j, 1);
                        break;
                    }
                }

            }
        }
    }
    // Set gia tri Func tu form
    $scope.setFunctions = function (roleid, fcid) {
        var inputchild = $('#role' + roleid).find("div").find("input");
        var chk = document.getElementById('roleselect' + roleid);
        var chkfc = document.getElementById('fc' + fcid);
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
            var fcidval = "input#fc" + fcid;
            if (cck == fcidval) {
                chkval = cckval;
                break;
            }
        }
        //push list
        if (chkval) {
            $scope.lstRoleFunction.push({ 'RoleID': roleid, 'FunctionID': fcid });
        } else {
            for (var j = 0; j < $scope.lstRoleFunction.length; j++) {
                if ($scope.lstRoleFunction[j].FunctionID == fcid && $scope.lstRoleFunction[j].RoleID == roleid) {
                    $scope.lstRoleFunction.splice(j, 1);
                    break;
                }
            }
        }
    }

    // Thêm Con
    $scope.newSubItem = function (data) {
        var itemData = data.items;
        itemData.push({
            id: -1,
            items: [],
            parentid: data.id,
            name: '--Liên kết con--'
        });
        $scope.checkfunction(-1, data.id);
    };
    // Thêm cùng cấp
    $scope.newItem = function (data) {
        
        $scope.pageId = "";
        $scope.functionId = "";

        if (data.parentid == 0) {
            $scope.lstData.push({
                id: -1,
                items: [],
                parentid: 0,
                name: '--Liên kết cùng cấp--'
            });
            $scope.checkfunction(-1, data.parentid);
        }
        else {
            var itemdata = $scope.lstData;
            for (var i = 0 ; i < itemdata.length; i++) {
                if (itemdata[i]["id"] == data.parentid) {
                    var item = itemdata[i].items;
                    item.push({
                        id: -1,
                        items: [],
                        parentid: data.parentid,
                        name: '--Liên kết cùng cấp--'
                    })
                    $scope.checkfunction(-1, data.parentid);
                }
                else {
                    for (var j = 0; j < itemdata[i].items.length; j++) {
                        if (itemdata[i].items[j]["id"] == data.parentid) {
                            var item = itemdata[i].items[j];
                            item.items.push({
                                id: -1,
                                items: [],
                                parentid: data.parentid,
                                name: '--Liên kết cùng cấp--'
                            })
                            $scope.checkfunction(-1, data.parentid);
                        }
                    }
                }
            }
        }

    }

    //Xoa du lieu menulink
    $scope.removes = function (id, Name, scope) {
        debugger
        if (id != -1) {
            $scope.Id = id;
            $scope.Name = Name;
            $('#confirm-delete').modal("show");
        }
        else {
            //var Array = scope.parent();
            //var index = Array.indexOf(scope);
            //Array.splice(index, 1);
            scope.remove();
            $('#confirm-delete').modal("hide");
        }
    }
    // Delete bảng trung gian
    var deleteMenuFunctionRole = function (id) {
        crudService.remove("/MenuLinkFunctionRole/Delete", id);
    }
    //Convert Tree
    var getbymenulink = function (id) {
        crudService.get("/MenuLinkFunctionRole/GetByMenuLinkId?id=", id)
            .success(function (data) {
                $scope.lstRoleFunction = data;
            });
    }
    $scope.setCBRole = function (id) {

        if ($scope.lstRoleFunction == null) {
            return;
        }
        var i = 0;
        var checked = false;
        for (i ; i < $scope.lstRoleFunction.length ; i++) {
            if (id == $scope.lstRoleFunction[i].RoleID) {
                checked = true;
                return checked;
            }
        }
        return checked;
    }

    $scope.setCBFuc = function (id, roleid) {
        if ($scope.lstRoleFunction == null) {
            return;
        }
        var i = 0;
        var checked = false;
        for (i ; i < $scope.lstRoleFunction.length ; i++) {
            if (id == $scope.lstRoleFunction[i].FunctionID && $scope.lstRoleFunction[i].RoleID == roleid) {
                checked = true;
                return checked;
            }
        }
        return checked;
    }
    //// Check Role 
    //var GetMenuIDfromUrl = function () {
    //    var pathName = window.location.pathname;
    //    pathName = pathName.substr(1);
    //    crudService.get("/MenuLink/GetMenuLinkIDbyLink?path=", pathName)
    //    .success(function (result) {
    //        $scope.MenuIDLink = result.ID;
    //    })
    //}
    //GetMenuIDfromUrl();

    // Check Role
    $scope.CheckRoleView = function (menuid) {
        if ($cookieStore.get("SuperUser") != undefined) {
            return true;
        }
        var role = $cookieStore.get("ListFuction");
        if (role != null || role != undefined) {
            for (var i = 0 ; i < role.length; i++) {
                if (role[i]["MenuLinkID"] == menuid) {
                    return true;
                }
            }
        }
        return false;
    }

    // Check Role View MEnuLink   $scope.MenuID
    $scope.CheckRoleViewPage = function () {
        if ($cookieStore.get("SuperUser") != undefined) {
            return true;
        }
        var role = $cookieStore.get("ListFuction");
        if (role != null || role != undefined) {
            for (var i = 0 ; i < role.length; i++) {
                if (role[i]["MenuLinkID"] == $scope.MenuIDLink) {
                    return true;
                }
            }
        }
        else {
            return false;
        }

    }
    // Set ActiveLink
    $scope.SetActive = function (item) {
        var path = window.location.pathname;
        if (item == 'Home/Index') {
            var linkactive = '/' + item;
        } else {
            var linkactive = '/' + item.link;
        }

        if (linkactive == path) {
            var id = "#menulink" + item.parentid;
            $(id).addClass('active open');
            return { active: true };
        }
        else {
            return {};
        }
    }


    $scope.setClassDrop = function (item) {

        if (item.items.length > 0) {
            $("#link" + item.id).addClass('dropdown-toggle');
            $("#link" + item.id).attr('href', '');
            $("#link" + item.id).append('<b class="arrow fa fa-angle-down"></b>');
            return;
        }
        else {
            return;
        }

    }
    $scope.EntnkyML = function (id) {
        $("#" + id + "").focus();
    }

    $scope.showContextmenu = function (event, id) {
        $("div.sub-div-menu").hide();
        switch (event.which) {
            case 1: $("div.sub-div-menu").hide();
            case 2: break;
            case 3: $("#contextmenu" + id).show();
        }
    }
    $scope.hidesubmenu = function () {
        $("div.sub-div-menu").hide();
    }

    $scope.ActiveMenu = function (id) {
        if (id == $scope.Menulink.ID) {
            return { color: "red" }
        }
        else
            return;
    }
})