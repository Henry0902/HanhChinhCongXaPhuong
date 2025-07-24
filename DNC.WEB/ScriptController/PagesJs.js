myApp.controller("PagesJs",
    function ($location, $scope, $routeParams, crudService, $http, $cookieStore, $timeout) {
        $scope.lstData = [];
        $scope.lstBase = [];
        $scope.lstRoles = [];
        $scope.Pages = {};
        $scope.ID = "";
        $scope.Name = "";
        $scope.lstselect = [];
        $scope.order = 1;
        $scope.lstRoleSelected = [];
        $scope.lstFunctions = [];
        $scope.lstFunctionSelected = [];
        $scope.lstRoleFunction = [];
        $scope.lstDirectoryLink = [];
        $scope.lstPagesGetAllLevel = [];
        $scope.mId = -1;
        $scope.lstPutCookie = [];
        $scope.lstRoleID = "";
        $scope.skinIndex = true;
        $scope.lstDocuments = [];
        $scope.pageIndex = 1;
        $scope.pageSize = 20;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;
        $scope.currentPage = 1;
        $scope.lstPage = [];

        $scope.currentPage = {};
        $scope.currentPage.stt = 1;


        // Convert datetime
        var parseDate = function(value) {
            if (value) {
                var dateString = value.substr(6);
                var currentTime = new Date(parseInt(dateString));
                var month = currentTime.getMonth() + 1;
                var day = currentTime.getDate();
                var year = currentTime.getFullYear();
                if (day < 10) {
                    day = "0" + day;
                }
                if (month < 10) {
                    month = "0" + month;
                }
                var date = day + "/" + month + "/" + year;
                return date;
            }
            return null;
        }
        var getData = function() {
            if ($cookieStore.get("SuperUser") != undefined && $cookieStore.get("SuperUser") != '') {
                return $http({
                    url: "/Pages/GetAllSuper",
                    method: "GET",
                    params: {}
                });
            }
            return $http({
                url: "/Pages/GetAll",
                method: "GET",
                params: {}
            });
        }
        var init = function() {
            getData()
                .success(function(data) {
                    var items = [];
                    var source = [];
                    for (var i = 0; i < data.length; i++) {
                        //set gia tri cho item
                        var itemdata = data[i];
                        var name = itemdata["Name"];
                        var parentid = itemdata["ParentId"];
                        var id = itemdata["PageId"];
                        var css = itemdata["CssClass"];
                        var link = itemdata["Url"];
                        var target = itemdata["Target"];
                        var hasChild = itemdata["hasChild"];
                        //-----------------------------------
                        if (items[parentid]) { //truong hop parentid <> 0
                            var item = { id: id, parentid: parentid, name: name, css: css, link: link, target: target, hasChild: hasChild, items: [] };
                            if (!items[parentid].items) {
                                items[parentid].items = [];
                            }
                            items[parentid].items[items[parentid].items.length] = item;
                            items[id] = item;
                        } else {
                            items[id] = { id: id, parentid: parentid, name: name, css: css, link: link, target: target, hasChild: hasChild, items: [] };
                            source.push(items[id]);
                        }
                    }
                    $scope.lstData = source;
                    $scope.lstBase = data;
                    $scope.Pages.IsLocked = true;
                    $scope.Pages.Order = 1;
                    $scope.Pages.ParentID = 0;
                })
                .error(function(error) {
                    $.notify("Lỗi khi lấy dữ liệu!", "error");
                });
        }
        init();
        // Set gia tri Func tu form
        $scope.setFunctions = function(roleid, fcid) {
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

        // Check Role
        $scope.CheckRoleView = function(menuid) {
            if ($cookieStore.get("SuperUser") != undefined && $cookieStore.get("SuperUser") != '') {
                return true;
            }
            var role = $cookieStore.get("ListFuction");
            if (role != null || role != undefined) {
                for (var i = 0; i < role.length; i++) {
                    if (role[i]["PageId"] == menuid) {
                        return true;
                    }
                }
            }
            return false;
        }
        // Check Role View Pages   $scope.MenuID
        $scope.CheckRoleViewPage = function() {
            if ($cookieStore.get("SuperUser") != undefined) {
                return true;
            }
            var role = $cookieStore.get("ListFuction");
            if (role != null || role != undefined) {
                for (var i = 0; i < role.length; i++) {
                    if (role[i]["PageId"] == $scope.PageId) {
                        return true;
                    }
                }
            } else {
                return false;
            }
        }
        // Check Role Add/Edit
        $scope.CheckAddandEdit = function(roleid) {
            if ($cookieStore.get("SuperUser") != undefined) {
                return true;
            }
            var role = $cookieStore.get("ListFuction");
            
            
            for (var i = 0; i < role.length; i++) {
                if (role[i]["PageId"] == $scope.PageId) {
                    var listrole = role[i]["FunctionId"];
                    return listrole.includes(roleid);
                }
            }
        }

        // Check Roles - mới sửa lỗi - dùng cho tất cả các view
        var getPageId = function () {
            
            var pathName = window.location.pathname;
            pathName = pathName.substr(1);

            crudService.get("/Pages/GetPagesIDbyLink?path=", pathName)
                .success(function (result) {
                    $scope.PageId = result;
                });
        }
        getPageId();
        $scope.checkFunctionRoles = function (roleid) {
            //console.log("roleid", roleid)
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

        // Set ActiveLink
        $scope.SetActive = function(item) {
            var path = window.location.pathname;
            if (item == 'Home/Index') {
                var linkactive = '/' + item;
            } else {
                var linkactive = '/' + item.link;
            }

            if (linkactive == path) {
                var id = "#page" + item.parentid;
                $(id).addClass('active open');
                return { active: true };
            } else {
                return {};
            }
        }

        // Set ActiveLink
        $scope.SetActiveParent = function (item) {
            var path = window.location.pathname;
            if (item == 'Home/Index') {
                var linkactive = '/' + item;
            } else {
                var linkactive = '/' + item.link;
            }

            if (linkactive == path) {
                var id = "#page" + item.id;
                $(id).addClass('active open');
                return { active: true };
            } else {
                return {};
            }
        }

        $scope.setClassDrop = function(item) {
            $timeout(function() {
                if (item.hasChild > 0) {
                    $("#link" + item.id).addClass('dropdown-toggle');
                    $("#link" + item.id).removeAttr('href', null);
                    $("#link" + item.id).append('<b class="arrow fa fa-angle-down"></b>');
                    return;
                } else {
                    return;
                }
            }, 1);
        }
        $scope.EntnkyML = function(id) {
            $("#" + id + "").focus();
        }

        $scope.showContextmenu = function(event, id) {
            $("div.sub-div-menu").hide();
            switch (event.which) {
                case 1:
                    $("div.sub-div-menu").hide();
                case 2:
                    break;
                case 3:
                    $("#contextmenu" + id).show();
            }
        }
        $scope.hidesubmenu = function() {
            $("div.sub-div-menu").hide();
        }

        $scope.isDisable = function(currentPage, pageCount) {
            if (currentPage == 0) {
                currentPage = 1;
            }
            if (currentPage == pageCount) {
                return "hidden";
            }
        }

        $scope.formatText = function(sStr, number) {
            if (sStr != null) {
                sStr = sStr.replace('_', ' ');
                sStr = sStr.replace('%20', ' ');
                if (number >= sStr.length) {
                    return sStr;
                }
                var last = sStr.lastIndexOf(' ', number);
                if (last > 0) {
                    return sStr.substring(0, last).replace('_', ' ') + '...';
                } else {
                    var sResult = '';
                    if (sStr.length > number) {
                        sResult = sStr.substring(0, number - 1);
                    } else {
                        sResult = sStr;
                    }
                    return sResult;
                }
            } else return "";
        }
        // Reload
        $scope.reload = function() {
            $scope.lstPage = [];
            $scope.lstDocuments = [];
            $scope.currentPage = 1;
            $scope.lstPage.splice(0);
            $scope.getDocumentExpired();
        }
        // Lấy về trang click vào trong phân trang
        $scope.selectPage = function(index) {
            if (index == 0) {
                index = 1;
            }
            $scope.currentPage = index;
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
                $scope.currentPage = index;
                $scope.getDocumentExpired();
            }
        }

    //    $scope.countActive = 0;

        $scope.activepage = function (page) {
     //       $scope.countActive++;
            if (page == $scope.currentPage.stt) {
                return { color: "white", background: "#4d79ff" }
            } else return "";
        }

        //Load thong bao
        $scope.ThongBao = {};
        var current = new Date();
        $scope.currentYear = current.getFullYear();
        $scope.cap = $cookieStore.get("CapDonVi");
        $scope.TongThongBao = 0;

        var getThongBaoCount = function () {
            return $http({
                 url: "/Pages/ThongBaoCount",
                 method: "GET",
                 params: {
                     IdDonVi: $cookieStore.get("DeparmentId")
                 }
             }).success(function (data) {
                 $scope.ThongBao = data;

                 if ($scope.CheckNotificationPage('DonThu/DonThuTiepNhan') == true)
                 {
                     $scope.TongThongBao += $scope.ThongBao.DonThuXuLy
                 }

                 if ($scope.CheckNotificationPage('DonThu/DonThuDuyetXuLy') == true) {
                     $scope.TongThongBao += $scope.ThongBao.DonThuDuyetXuLy
                 }

                 if ($scope.CheckNotificationPage('DonThu/DonThuGiaiQuyet') == true) {
                     $scope.TongThongBao += $scope.ThongBao.DonThuThuLy
                 }

                 if ($scope.CheckNotificationPage('DonThu/DonThuKetLuan') == true) {
                     $scope.TongThongBao += $scope.ThongBao.DonThuKetLuan
                 }

                 if ($scope.CheckNotificationPage('DonThu/DonThuDuyetKetLuan') == true) {
                     $scope.TongThongBao += $scope.ThongBao.DonThuDuyetKetLuan
                 }

                 if ($scope.CheckNotificationPage('DonThu/DonThuTraKetQua') == true) {
                     $scope.TongThongBao += $scope.ThongBao.DonThuTraKetQua
                 }

             }).error(function (error) {
                 $.notify("Lấy số liệu thông báo bị lỗi.", "error");
             });
         }
         getThongBaoCount();

        //$scope.setCookieHSLT = function (key, value) {
        //    $cookieStore.put(key, value);
        //}

        $scope.CheckNotificationPage = function (Url) {
            for (var i = 0; i < $scope.lstBase.length; i++) {
                if ($scope.lstBase[i].Url == Url) {
                    return true;
                }
            }
            return false;
        }
    });