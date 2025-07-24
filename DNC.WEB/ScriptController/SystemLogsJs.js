
    myApp.controller("SystemLogsJs",
function ($location, $scope, $routeParams, crudService, $http,$cookieStore) {
    $scope.keyword = "";
    $scope.type = -1;
    $scope.status = -1;
    $scope.pageIndex = 1;
    $scope.pageSize = 20;
    $scope.totalRecords = 0;
    $scope.pageCount = 0;
    $scope.currentPage = 1;
    $scope.lstSystemLogs = [];
    $scope.lstPage = [];
    $scope.dateFrom = '';
    $scope.dateTo = '';

    var getData = function (pageIndex, pageSize) {
        return $http({
            url: "/SystemLogs/GetIndex",
            method: "GET",
            params: {
                keyword: $scope.keyword, type: $scope.type,
                status: $scope.status, dateFrom: $scope.dateFrom,
                dateTo: $scope.dateTo, pageSize: $scope.pageSize, pageIndex: $scope.currentPage
            }
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
    var clearData = function () {
        $scope.lstPage = [];
    }
    // Hàm Init để đổ dữ liệu
    var init = function () {
        getData($scope.currentPage, $scope.pageSize)
        .success(function (data) {
            angular.forEach(data, function (item) {
                item.ActionDate = parseDate(item.ActionDate);
            });
            $scope.lstSystemLogs = data;
            if (data.length > 0) {
                $scope.totalRecords = data[0].TotalRecords;
                
            } else $scope.totalRecords = 0;
            $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
            for (var i = 1; i <= $scope.pageCount && i <= 9; i++) {
                $scope.lstPage.push(i);
            }
        })
        .error(function (error) {
            console.log(error);
        });
        //GetData();
    }
    init();
    // Lấy về trang click vào trong phân trang 
    $scope.selectPage = function (index) {
        $scope.currentPage = index; 
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
                    item.ActionDate = parseDate(item.ActionDate);
                });
                $scope.lstSystemLogs = data;
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
    // Xử Lý intenr Hàm Tìm Kiếm
    $('#txtKeyword').keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $scope.Search();
        }

    });
    // Xử Lý Hàm Tìm Kiếm
    $scope.Search = function () {
        $scope.name = $("#txtKeyword").val();
        if ($("#txtDateFrom").val() != null || $("#txtDateFrom").val() != "") {
            $scope.dateFrom = $("#txtDateFrom").val();
        } 
        if ($("#txtDateTo").val() != null || $("#txtDateTo").val() != "") {
            $scope.dateTo = $("#txtDateTo").val();
        }
        clearData();
        $scope.reload();
    }
    // display index
    $scope.reload = function () {
        $scope.lstPage = [];
        $scope.currentPage = 1;
        $scope.lstPage.splice(0);
        init();
    }
    // Get data by Id    
    var getById = function (id) {
        crudService.get("/SystemLogs/GetById?id=", id)
     .success(function (data) {
         data.ActionDate = parseDate(data.ActionDate);
         $scope.SystemLogs = data;
     })
        .error(function (error) {
            $.notify("Không tìm thấy dữ liệu!", "error");
        });
    } 
    // Hàm Delete
    $scope.remove = function (id) {
        crudService.remove("/SystemLogs/Delete", id)
        .success(function () {
            $("#confirm-delete").modal("hide");
            $.notify("Xóa thành công!", "success");
                $scope.reload();
            })
		.error(function (error) {
		    console.log(error);
		});
    }
    //Check delete
    $scope.checkremove = function (id, ActionName) {
        $scope.Id = id;
        $scope.ActionName = ActionName;
    }
    //Check All
    $scope.CheckAllCb = function () {
        var lfckv = document.getElementById("checkAll").checked;
        var data = $scope.lstSystemLogs;
        if (lfckv) {
            for (var i = 0; i < data.length; i++) {
                var vid = "#" + data[i].Id;
                $(vid).prop("checked", true);
            }
            $("#btnDel").removeClass("hidden").addClass("block");
        } else {
            for (var i = 0; i < data.length; i++) {
                var vid = "#" + data[i].Id;
                $(vid).prop("checked", false);
            }
            $("#btnDel").removeClass("block").addClass("hidden");
        }
    }
    $scope.checkDelete = function(id) {
        var lfckv = document.getElementById(id).checked;
        if (lfckv) {
            $(id).prop("checked", true);
            $("#btnDel").removeClass("hidden").addClass("block");
        } else {
            $(id).prop("checked", false);
            $("#btnDel").removeClass("block").addClass("hidden");
        }

    }
    // Hàm Delete
    $scope.DeleteAll = function () {
        var data = $scope.lstSystemLogs;
        for (var i = 0; i < data.length; i++) {
            var vid =data[i].Id;
            var id = document.getElementById(vid).checked;
            if (id) {
            crudService.remove("/SystemLogs/Delete", vid)
                .success(function() {
            });
            }
        } 
        $("#confirm-multidelete").modal("hide");
        $.notify("Xóa thành công!", "success");
        $scope.reload();
        $('#checkAll').attr('checked', false);
        $("#btnDel").removeClass("block").addClass("hidden");
    }
    // Hàm Delete by time
    $scope.DeleteByTime = function () {
        var time = $('#cbtime option:selected').val();
        crudService.get("/SystemLogs/DeleteByTime?time=", time)
        .success(function () {
            $("#confirm-deletebytime").modal("hide");
            $.notify("Xóa thành công!", "success");
            $scope.reload();
        });
        $scope.reload();
    }
    // Load dữ liệu theo record
    $scope.GetDataById = function (id) {
        getById(id);
    }
    // Set type logs
    $scope.setTypeLogs = function (type) {
        if (type == 1) {
            return "LOGIN";
        }else if (type == 2) {
            return "LOGOUT";
        }else if (type == 3) {
            return "ADD";
        }else if (type == 4) {
            return "UPDATE";
        }else if (type == 5) {
            return "DELETE";
        }else if (type == 6) {
            return "GET";
        }else if (type == 7) {
            return "COMMENT";
        }else if (type == 8) {
            return "DOWNLOAD";
        }else if (type == 9) {
            return "BOOKMARK";
        }else if (type == 10) {
            return "SETTING";
        } else {
            return "REPORT";
        }
    }
    // Ham load thong tin chi tiet
    $(document).on("click", ".show-details-btn", function () {
        var allElement = document.getElementsByClassName("detail-row");
        var element = $(this).closest("tr").next();
        if ($(element).hasClass("hide")) {
            $(allElement).addClass("hide").removeClass('open');
            $(element).addClass("open").removeClass('hide');
        } else if ($(element).hasClass("open")) {
            $(element).addClass("hide").removeClass('open');
        }
    }); 
    $scope.activepage = function (page) {
        if (page == $scope.currentPage) {
            return { color: "white", background: "#f07030" }
        }
    }
});