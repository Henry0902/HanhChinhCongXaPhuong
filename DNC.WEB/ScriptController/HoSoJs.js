
myApp.controller("HoSoJs",
    function ($location, $scope, $routeParams, $route, crudService, $http, $cookieStore) {
        // Tham số   
        $scope.namID = -1;
        $scope.khoID = -1;
        $scope.kegiaID = -1;
        $scope.hopcapID = -1;
        $scope.departmentID = -1;
        $scope.loaihsID = -1;
        $scope.domatID = -1;
        $scope.tinhtrangvlID = -1;
        $scope.ngay_baoquan_from = '';
        $scope.ngay_baoquan_to = '';
        $scope.ngay_luutru_from = '';
        $scope.ngay_luutru_to = '';
        $scope.Keyword = '';
        $scope.hoso_type = '-1';
        $scope.trang_thai = '-1';
        $scope.check = false;
        $scope.typefile = 1;
        $scope.hosoId_New = '';

        $scope.lstNam = [];
        $scope.lstKho = [];
        $scope.lstKeGia_Search = [];
        $scope.lstHopCap_Search = [];
        $scope.lstKeGia = [];
        $scope.lstHopCap = [];
        $scope.lstLinhVuc = [];
        $scope.lstTinhTrangVL = [];
        $scope.lstLoaiHoSo = [];
        $scope.lstDoMat = [];
        $scope.lstMucDoTruyCap = [];
        $scope.lstThoiHan = [];
        $scope.lstDepartments = [];
        $scope.lstDuAn = [];
        $scope.lstDepartments_Search = [];

        $scope.lstAllHoso = [];
        $scope.lstHoSo = [];
        $scope.lstDocs = [];
        $scope.HoSo = {};
        $scope.today = new Date();
        $scope.lstPage = [];
        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        //-------------------------------------------------------------------------------
        var lblsohieuhs = document.getElementById("lblsohieuhs");
        var lbltenhs = document.getElementById("lbltenhs");
        var lblmahs = document.getElementById("lblmahs");
        var lblsohs = document.getElementById("lblsohs");
        //-------------------------------------------------------------------------------        

        // Convert datetime
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        //Lấy danh sách lĩnh vực
        var getListNam = function () {
            var currentTime = new Date();
            var nam = currentTime.getFullYear();
            $scope.namID = nam;
            for (i = nam; i > nam - 5; i--) {
                $scope.lstNam.push(i);
            }
        }
        getListNam();

        //Lấy danh sách dự án
        var getListDuAn = function () {
            crudService.getAll("/DuAn/GetAll")
                .success(function (data) {
                    $scope.lstDuAn = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách dự án bị lỗi.", "error");
                });
        }
        getListDuAn();

        //Lấy danh sách kho
        var getListKho = function () {
            crudService.getAll("/Kho/GetAll")
                .success(function (data) {
                    $scope.lstKho = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách kho bị lỗi.", "error");
                });
        }
        getListKho();

        $scope.getKeGiabyKhoID_Search = function (id) {
            $scope.kegiaID = -1;
            $scope.hopcapID = -1;
            $scope.lstKeGia_Search = null;
            $scope.lstHopCap_Search = null;

            crudService.get("/KeGia/GetKegiaByKhoId/", id)
                .success(function (data) {
                    $scope.lstKeGia_Search = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách kệ giá bị lỗi.", "error");
                });
             $scope.reload();
        }

        $scope.getHopCapbyKegiaID_Search = function (id) {
            $scope.hopcapID = -1;
            $scope.lstHopCap_Search = null;

            crudService.get("/HopCap/GetHopCapByKegiaId/", id)
                .success(function (data) {
                    $scope.lstHopCap_Search = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách hộp cặp bị lỗi.", "error");
                });
               $scope.reload();
        }        

        $scope.getKeGiabyKhoID = function (id) {       
            $scope.lstKeGia = null;
            $scope.lstHopCap = null;
            $scope.HoSo.kegia_id = "";
            $scope.HoSo.hopcap_id = "";

            crudService.get("/KeGia/GetKegiaByKhoId/", id)
                .success(function (data) {
                    $scope.lstKeGia = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách kệ giá bị lỗi.", "error");
                });
        }

        $scope.getHopCapbyKegiaID = function (id) {
            $scope.lstHopCap = null;
            $scope.HoSo.hopcap_id = "";

            crudService.get("/HopCap/GetHopCapByKegiaId/", id)
                .success(function (data) {
                    $scope.lstHopCap = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách hộp cặp bị lỗi.", "error");
                });
        }

        //Lấy danh sách lĩnh vực
        //var getListLinhVuc = function () {
        //    crudService.getAll("/LinhVuc/GetAll")
        //        .success(function (data) {
        //            $scope.lstLinhVuc = data;
        //        }).error(function (error) {
        //            $.notify("Lấy danh sách Lĩnh vực bị lỗi.", "error");
        //        });
        //}
        //getListLinhVuc();

        //Lấy danh sách tình trạng vật lý
        var getListTinhTrangVL = function () {
            crudService.getAll("/TinhTrangVatLy/GetAll")
                .success(function (data) {
                    $scope.lstTinhTrangVL = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách tình trạng vật lý bị lỗi.", "error");
                });
        }
        getListTinhTrangVL();

        //Lấy danh sách loại hồ sơ
        var getListLoaiHoSo = function () {
            crudService.getAll("/LoaiHoSo/GetAll")
                .success(function (data) {
                    $scope.lstLoaiHoSo = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách loại hồ sơ bị lỗi.", "error");
                });
        }
        getListLoaiHoSo();

        //Lấy danh sách độ mật
        var getListDoMat = function () {
            crudService.getAll("/DoMat/GetAll")
                .success(function (data) {
                    $scope.lstDoMat = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách độ mật bị lỗi.", "error");
                });
        }
        getListDoMat();

        //Lấy danh sách mức độ truy cập
        var getListMucDoTruyCap = function () {
            crudService.getAll("/MucDoTruyCap/GetAll")
                .success(function (data) {
                    $scope.lstMucDoTruyCap = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách mức độ truy cập bị lỗi.", "error");
                });
        }
        getListMucDoTruyCap();

        //Lấy danh sách mức độ truy cập
        var getListThoiHan = function () {
            crudService.getAll("/ThoiHan/GetAll")
                .success(function (data) {
                    $scope.lstThoiHan = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách thời hạn bị lỗi.", "error");
                });
        }
        getListThoiHan();

        //lấy danh phòng ban
        var getAllDepartment = function() {
            crudService.getAll("/Departments/getAllDropDown")
                .success(function(data) {
                    $scope.lstDepartments = data;
                });

            var supper = $cookieStore.get("SuperUser");
            if (supper != 1) {
                $scope.departmentID = $cookieStore.get("DeparmentId");
            }
        }
        getAllDepartment();

        //lấy danh phòng ban phần tìm kiếm
        var getAllDepartmentSearch = function () {
            crudService.getAll("/Departments/getAllDropDown_Search")
                .success(function (data) {
                    $scope.lstDepartments_Search = data;
                });           
        }
        getAllDepartmentSearch();

        //Load danh sách data
        var getData = function () {
            return $http({
                url: "/HoSo/GetIndex",
                method: "GET",
                params: {
                    nam: $scope.namID, kho_id: $scope.khoID, kegia_id: $scope.kegiaID, hopcap_id: $scope.hopcapID,
                    ngay_baoquan_from: $scope.ngay_baoquan_from, ngay_baoquan_to: $scope.ngay_baoquan_to, ngay_luutru_from: $scope.ngay_luutru_from, ngay_luutru_to: $scope.ngay_luutru_to,
                    donvi_id: $scope.departmentID, loaihs_id: $scope.loaihsID,
                    domat_id: $scope.domatID, ttvl_id: $scope.tinhtrangvlID,
                    Keyword: $scope.Keyword, hoso_type: $scope.hoso_type,
                    trang_thai: $scope.trang_thai, order: $scope.Order, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex
                }
            });
        }

        // Init data
        var init = function () {
          //  var keyword = $cookieStore.get("keyword");
            var keyword = crudService.getCookie("keyword");
            if (!keyword == '' || !keyword == null) {
                $scope.Keyword = keyword;
                crudService.removeCookie("keyword")
            }

            getData()
            .success(function (data) {
                angular.forEach(data, function (item) {
                    item.ngay_tao = parseDate(item.ngay_tao);
                    item.ngay_baoquan_from = parseDate(item.ngay_baoquan_from);
                    item.ngay_baoquan_to = parseDate(item.ngay_baoquan_to);
                    item.ngay_luutru_from = parseDate(item.ngay_luutru_from);
                    item.ngay_luutru_to = parseDate(item.ngay_luutru_to);
                });
                $scope.lstHoSo = data;
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
                        item.ngay_tao = parseDate(item.ngay_tao);
                        item.ngay_baoquan_from = parseDate(item.ngay_baoquan_from);
                        item.ngay_baoquan_to = parseDate(item.ngay_baoquan_to);
                        item.ngay_luutru_from = parseDate(item.ngay_luutru_from);
                        item.ngay_luutru_to = parseDate(item.ngay_luutru_to);
                    });
                    $scope.lstHoSo = data;
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
            $scope.currentPage.stt = 1;
            $scope.lstPage.splice(0);
            $scope.lstPage = [];
            init();
        }

        // Hiển thị thông tin modal Insert Update
        $scope.show = function (id) {
            $scope.HoSo = {};
            $('#lblsohieuhs').hide();
            $('#lbltenhs').hide();
            $('#lblmahs').hide();
            $('#lblsohs').hide();
            $("#lblkhohs").hide();
            $("#lblkegiahs").hide();
            $("#lblhopcaphs").hide();
            $("#lblphongban").hide();
            $("#lblbaoquanhs").hide();
            $('#autofocus').hide();
            if (id != "") {
                $scope.currentId = id;
                $("#crud").val("update");
                $('#Mahs').attr('readonly', true);
                $('#SoHieuhs').attr('readonly', true);
                $scope.getbyID(id);
            }
            else {
                $("#crud").val("create");
                $('#Mahs').attr('readonly', false);
                $('#SoHieuhs').attr('readonly', false);               
                $scope.HoSo.donvi_id = $cookieStore.get("DeparmentId");
                $scope.HoSo.hoso_type = 0;
            }
        }

        //var getDataForPlugin = function (id, typeoffile) {
        //    return $http({
        //        url: "/Files/GetAllFileByDocId",
        //        method: "GET",
        //        params: {
        //            id: id,
        //            fileType: typeoffile
        //        }
        //    });
        //}

        var getDataForPlugin = function (id, typeoffile) {
            return $http({
                url: "/Files/GetAllFileByDocId",
                method: "GET",
                params: {
                    id: id, fileType: typeoffile
                }
            });
        }

        // Get Data By ID
        $scope.getbyID = function (id) {
            crudService.get("/HoSo/GetById?id=", id)
                .success(function (data) {
                    data.ngay_tao = parseDate(data.ngay_tao);
                    data.ngay_baoquan_from = parseDateV(data.ngay_baoquan_from);
                    data.ngay_baoquan_to = parseDateV(data.ngay_baoquan_to);
                    data.ngay_luutru_from = parseDateV(data.ngay_luutru_from);
                    data.ngay_luutru_to = parseDateV(data.ngay_luutru_to);
                    $scope.hoso_id = data.hoso_id;
                    $scope.getKeGiabyKhoID(data.kho_id);
                    $scope.getHopCapbyKegiaID(data.kegia_id);

                    if (data.ngay_baoquan_from == null) {
                        $scope.check = true;
                    }
                    else
                    {
                        $scope.check = false;
                    }

                    $scope.HoSo = data;

                    // lấy dánh sách tài liệu thuộc hồ sơ
                    crudService.get("/Docs/GetAllDocsByHosoId?hosoId=", id)
                        .success(function (data) {
                            angular.forEach(data, function (item) {
                                item.ngay_ban_hanh = parseDate(item.ngay_ban_hanh);
                                
                                // lấy danh sách file đính kèm của tài liệu
                                getDataForPlugin(item.doc_id, $scope.typefile)
                                  .success(function (result) {
                                    angular.forEach(result, function (items) {                        
                                        if (items.file_extension == ".ppt" || items.file_extension == ".pptx" || items.file_extension == ".PPT" || items.file_extension == ".PPTX") {
                                            items.icon = '/Content/assets/images/icon/ppt.png';
                                        } else if (items.file_extension == ".doc" || items.file_extension == ".docx" || items.file_extension == ".DOC" || items.file_extension == ".DOCX") {
                                            items.icon = '/Content/assets/images/icon/doc.png';
                                        } else if (items.file_extension == ".xlsx" || items.file_extension == ".xls" || items.file_extension == ".XLSX" || items.file_extension == ".XLS") {
                                            items.icon = '/Content/assets/images/icon/xlsx.png';
                                        } else if (items.file_extension == ".pdf" || items.file_extension == ".PDF") {
                                            items.icon = '/Content/assets/images/icon/pdf.png';
                                        } else if (items.file_extension == ".jpeg" || items.file_extension == ".jpg" || items.file_extension == ".png" || items.file_extension == ".tif" || items.file_extension == ".gif" || items.file_extension == ".JPEG" || items.file_extension == ".JPG" || items.file_extension == ".PNG" || items.file_extension == ".TIF" || items.file_extension == ".GIF") {
                                            items.icon = '/Content/assets/images/icon/jpg.png';
                                        }
                                    });
                                    item.listSubfile = result;
                                  });                                
                            })
                            $scope.lstDocs = data;
                        }).error(function (error) {
                            $.notify("Lấy danh sách văn bản bị lỗi.", "error");
                        });

                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };
        
        // Thực hiện lưu trên modal Insert Update
        $scope.preCreate = function (data) {
            if (data == null) {
                $("#lblsohieuhs").show();
                lblsohieuhs.textContent = "Số hiệu hồ sơ không được để trống";
                $("#lbltenhs").show();
                lbltenhs.textContent = "Tên hồ sơ không được để trống";
                $("#lblmahs").show();
                lblmahs.textContent = "Mã hồ sơ không được để trống";
                $("#lblsohs").show();
                lblsohs.textContent = "Số hồ sơ không được để trống";
                $("#lblkhohs").show();
                $("#lblkegiahs").show();
                $("#lblhopcaphs").show();
                $("#lblphongban").show();
                $("#lblbaoquanhs").show();
                $("#lblloaihoso").show();
                return;
            }           

            if ($("#crud").val() == "create") {
                if (validate(data) == false) {
                    return;
                }
                
                $scope.create(data);
                $("#myModal").modal("hide");
            }
            else if ($("#crud").val() == "update") {
                if (validate(data) == false) {
                    return;
                }
                $scope.update(data);
                $("#myModal").modal("hide");
            }
        }

        var validate = function (data) {

            //if (data.duan_ma != null && data.duan_ma != "") {
            //    data.duan_ma = data.duan_ma.toUpperCase();
            //}

            $("#lblsohieuhs").hide();
            $("#lbltenhs").hide();
            $("#lblmahs").hide();
            $("#lblsohs").hide();
            
            if (data.hoso_ma == null || data.hoso_ma == "") {
                $("#lblmahs").show();
                lblmahs.textContent = "Mã hồ sơ không để trống!";
                $("#Mahs").focus();
                return false;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.hoso_ma) == true) {
                $("#lblmahs").show();
                lblmahs.textContent = "Mã hồ sơ không chứa ký tự đặc biệt!";
                $("#Mahs").focus();
                return false;
            }else {
                //check exist
                if ($scope.checkExistMaHoSo() == false && $("#crud").val() == "create") {
                    return false;
                }
            }


            data.hoso_ma = data.hoso_ma.toUpperCase();

            if (data.hoso_sohieu == null || data.hoso_sohieu == "") {
                $("#lblsohieuhs").show();
                lblsohieuhs.textContent = "Số hiệu hồ sơ không để trống!";
                $("#SoHieuhs").focus();                
                return false;
            }
            data.hoso_sohieu = data.hoso_sohieu.toUpperCase();

            if (data.hoso_ten == null || data.hoso_ten == "") {
                $("#lbltenhs").show();
                lbltenhs.textContent = "Tên hồ sơ không để trống!";
                $("#Tenhs").focus();
                return false;
            }
            if (/[`~!@#$%^&*()_|+\=?;:'"<>\{\}\[\]\\\/]/gi.test(data.hoso_ten) == true) {
                $("#lbltenhs").show();
                lbltenhs.textContent = "Tên hồ sơ không chứa ký tự đặc biệt!";
                $("#Tenhs").focus();
                return false;
            }            

            if (data.hoso_number == null || data.hoso_number == "") {
                $("#lblsohs").show();
                lblsohs.textContent = "Số hồ sơ không để trống!";
                $("#Hohs").focus();
                return false;
            }
            //else {
            //    //check exist
            //    if ($scope.checkExistUserName() == true) {
            //        return false;
            //    }
            //}

            if (data.kho_id == null || data.kho_id == "") {
                $("#lblkhohs").show();
                $("#Khohs").focus();
                return false;
            } else {
                $("#lblkhohs").hide();
            }
            if (data.kegia_id == null || data.kegia_id == "") {
                $("#lblkegiahs").show();
                $("#kegiahs").focus();
                return false;
            } else {
                $("#lblkegiahs").hide();
            }
            if (data.hopcap_id == null || data.hopcap_id == "") {
                $("#lblhopcaphs").show();
                $("#hopcaphs").focus();
                return false;
            } else {
                $("#lblhopcaphs").hide();
            }
            if (data.donvi_id == null || data.donvi_id == "") {
                $("#lblphongban").show();
                $("#phongban").focus();
                return false;
            } else {
                $("#lblphongban").hide();
            }
            if (data.loaihs_id == null || data.loaihs_id == "") {
                $("#lblloaihoso").show();
                $("#loaihoso").focus();
                return false;
            } else {
                $("#lblloaihoso").hide();
            }

            if ($scope.check == false) {
                if (data.ngay_baoquan_from == null || data.ngay_baoquan_from == "" || data.ngay_baoquan_to == null || data.ngay_baoquan_to == "") {
                    $("#lblbaoquanhs").show();
                    $("#ngay_baoquan_from").focus();
                    return false;
                }
            }            
            return true;
        }
        var getAllHoSo = function () {
            return $http({
                url: "/HoSo/GetAll",
                method: "GET"
            }).success(function (data) {
                $scope.lstAllHoso = data;
            });
        }
        getAllHoSo();

        $scope.checkExistMaHoSo = function () {
            var mahoso = $('#Mahs').val();
            for (var i = 0; i < $scope.lstAllHoso.length; i++) {
                if (mahoso.toUpperCase() == $scope.lstAllHoso[i].hoso_ma.toUpperCase()) {
                    $("#lblmahs").show();
                    lblmahs.textContent = "Mã hồ sơ đã được sử dụng !";
                    $("#Mahs").focus();
                    return false;
                } 
            }
            return true;
        }

        // Hàm thực hiện Create
        $scope.create = function (data) {

            if ($scope.check == true) {
                data.ngay_baoquan_from = null;
                data.ngay_baoquan_to = null;
                data.thoihan_id = 0;
            }

            //Format date dd/mm/yyyy -> mm/dd/yyyy
            var dateSplit = "";
            if (data.ngay_baoquan_from != null && data.ngay_baoquan_from != "" ) {
                dateSplit = data.ngay_baoquan_from.split('/');
                data.ngay_baoquan_from = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.ngay_baoquan_to != null && data.ngay_baoquan_to != "") {
                dateSplit = data.ngay_baoquan_to.split('/');
                data.ngay_baoquan_to = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }
            
            if (data.ngay_luutru_from != null && data.ngay_luutru_from != "") {
                dateSplit = data.ngay_luutru_from.split('/');
                data.ngay_luutru_from = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.ngay_luutru_to != null && data.ngay_luutru_to != "") {
                dateSplit = data.ngay_luutru_to.split('/');
                data.ngay_luutru_to = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }
            
            crudService.create("/HoSo/Create", data)
                .success(function (data) {
                    $.notify("Tạo mới thành công!", "success");
                    clearData();
                    init();
                    $scope.hosoId_New = data.hoso_id;
                    $("#ModalThemTaiLieu").modal("show");
                })
                .error(function (error) {
                    $.notify("Tạo mới thất bại!", "error");
                });
        }

        // Hàm thực hiện Update 
        $scope.update = function (data) {

            if ($scope.check == true) {
                data.ngay_baoquan_from = null;
                data.ngay_baoquan_to = null;
                data.thoihan_id = 0;
            }

            //Format date dd/mm/yyyy -> mm/dd/yyyy
            var dateSplit = "";
            if (data.ngay_baoquan_from != null && data.ngay_baoquan_from != "") {
                dateSplit = data.ngay_baoquan_from.split('/');
                data.ngay_baoquan_from = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.ngay_baoquan_to != null && data.ngay_baoquan_to != "") {
                dateSplit = data.ngay_baoquan_to.split('/');
                data.ngay_baoquan_to = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.ngay_luutru_from != null && data.ngay_luutru_from != "") {
                dateSplit = data.ngay_luutru_from.split('/');
                data.ngay_luutru_from = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.ngay_luutru_to != null && data.ngay_luutru_to != "") {
                dateSplit = data.ngay_luutru_to.split('/');
                data.ngay_luutru_to = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            crudService.update("/HoSo/Update", data)
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
            if (data.trang_thai == true) {
                data.trang_thai = false;
            } else {
                data.trang_thai = true;
            }
            UpdateIsLocked(data.hoso_id, data.trang_thai)
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
                url: "/HoSo/updateStatus",
                method: "GET",
                params: {
                    Id: Id,
                    Status: Status
                }
            });
        }

        $scope.TimNangCaoHoSo = function () {
            //$scope.ngay_baoquan_from = '';
            //$scope.ngay_baoquan_to = '';
            //$scope.ngay_luutru_from = '';
            //$scope.ngay_luutru_to = '';
            //$scope.departmentID = -1;
            //$scope.loaihsID = -1;
            //$scope.domatID = -1;
            //$scope.tinhtrangvlID = -1;
            var tt = $("#collapseOne").attr("aria-expanded");
            if (tt == 'false') {
                $("#collapseOne").attr("aria-expanded", true);
                $("#collapseOne").removeClass("collapse").addClass("collapse in");
            }
            else {
                $("#collapseOne").attr("aria-expanded", false);
                $("#collapseOne").removeClass("collapse in").addClass("collapse");
            }
            clearData();
            $scope.reload();
        }

        $scope.setCookiehosoId = function (value) {
            crudService.setCookie('hosoId', value);
            window.location.href = '../Docs/Index';    
        }

        $scope.ThemTaiLieu = function () {
            crudService.setCookie('hosoId', $scope.hosoId_New);
            window.location.href = '../Docs/Index';
        }

        $scope.shhDocs = function () {
            //alert("shhDocs");
            var today = new Date();
            var endday = new Date(today);
            endday.setDate(endday.getDate() + 6);
            $scope.ngay_baoquan_from = (today.getDate() > 9 ? today.getDate() : "0" + today.getDate()) +
                '/' + ((today.getMonth() + 1) > 9 ? (today.getMonth() + 1) : "0" + (today.getMonth() + 1)) + '/' + today.getFullYear();
            $scope.ngay_baoquan_to = (endday.getDate() > 9 ? endday.getDate() : "0" + endday.getDate()) +
                '/' + ((endday.getMonth() + 1) > 9 ? (endday.getMonth() + 1) : "0" + (endday.getMonth() + 1)) + '/' + endday.getFullYear();
            clearData();
            init();
        }

        $scope.hhDocs = function () {
            //alert("hhDocs");
            var today = new Date();
            today.setDate(today.getDate() - 1);
            $scope.ngay_baoquan_from = "01/01/1800";
            $scope.ngay_baoquan_to = (today.getDate() > 9 ? today.getDate() : "0" + today.getDate()) +
                '/' + ((today.getMonth() + 1) > 9 ? (today.getMonth() + 1) : "0" + (today.getMonth() + 1)) + '/' + today.getFullYear();
            clearData();
            init();
        }

        $scope.chDocs = function () {
            //alert("chDocs");
            var today = new Date();
            $scope.ngay_baoquan_from = (today.getDate() > 9 ? today.getDate() : "0" + today.getDate()) +
                '/' + ((today.getMonth() + 1) > 9 ? (today.getMonth() + 1) : "0" + (today.getMonth() + 1)) + '/' + today.getFullYear();
            $scope.ngay_baoquan_to = "31/12/3000";
            clearData();
            init();
        }

        //view file in main menu
        $scope.viewFileByDocId = function (fileExtension, fileUrl) {
            $scope.content = "";
            var port = $location.port();
            if (port != '') {
                $scope.content = $location.protocol() + "://" + $location.host() + ':' + $location.port() + fileUrl;
            } else {
                $scope.content = $location.protocol() + "://" + $location.host() + fileUrl;
            }

            if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".tif" || fileExtension == ".gif") {
                document.getElementById('iframe_viewfile').src = $scope.content;
            }
            else {
              //  $scope.content = "https://files.thuvienphapluat.vn/uploads/FileLargeTemp/2017/12/28/32_2017_TT-BCT_363171.doc";
                document.getElementById('iframe_viewfile').src = " http://docs.google.com/gview?url=" + $scope.content + "&embedded=true";
            }
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

        // Thay Đổi Màu Sắc của Hồ Sơ theo ngày bảo quản
        $scope.Changestyle = function (date) {
            var today = new Date();
            today.setDate(today.getDate() - 1);
            var time = (1000 * 60 * 60 * 24);

            if (date == null || ((date - today) / time) > 7) { // nếu bảo quản vĩnh viễn hoặc chưa đến hạn
                return { background: 'white' }
            }
            if (((date - today) / time) < 7 && ((date - today) / time) > 0) { // Sắp Hết Hạn Bảo Quản
                return { background: '#FFB752', color: 'white' }
            }
            if (((today - date) / time) > 0) {
                return { background: '#d62c2c', color: 'white' }  // Đã Hết Hạn Bảo Quản
            }

        }

        $scope.PrintInfo = function() {
            var divContents = document.getElementById("PrintInfo").innerHTML;
            var printWindow = window.open('', '', 'height=768,width=1024');
            ///  printWindow.document.write('<html><head><title>Print DIV Content</title>');
            printWindow.document.write('</head><body style="text-align: center" >');
            printWindow.document.write(divContents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.print();
        }
    });


