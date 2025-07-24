myApp.controller("BaoCaoDonThuTheoDiaBanJs",
function ($location, $scope, $routeParams, crudService, $http, $cookieStore, $filter) {
    //Khai báo dữ liệu
    $scope.dsChart1 = {};
    $scope.lstReport1 = [];
    //
    $scope.dsChart2 = {};
    $scope.lstReport2 = [];

    $scope.currentYear = new Date().getFullYear();
    $scope.Year = 2020;

    $scope.DonViId = $cookieStore.get("DeparmentId");
    $scope.CapDonVi = $cookieStore.get("CapDonVi");
    $scope.UserID = $cookieStore.get("UserID");

    $scope.IdDonVi = $cookieStore.get("DeparmentId");
    $scope.IdTinhThanh = 60;//60-TQ , 6-BN
    $scope.TuNgay = '';
    $scope.DenNgay = '';

    $scope.position = "position: sticky;";

    // Convert datetime
    var parseDate = function (value) {
        if (value) {
            return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
        }
        return null;
    }

    var date = new Date();
    $scope.TuNgay = '01/' + '01' + '/' + date.getFullYear();
    $scope.DenNgay = '31/' + '12' + '/' + date.getFullYear();

    // Thực thi dữ liệu
    //
    var addCommas = function (nStr) {
        nStr += '';
        var x = nStr.split('.');
        var x1 = x[0];
        var x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }
    //convertDate 
    var convertDate = (function (date) {
        var d1 = date.split('/');
        var datetime = new Date(d1[2], d1[1] - 1, d1[0]);
        return datetime;

    });
    // Năm
    var listYear = function () {
        var date = new Date().getFullYear();
        var lst = [];
        lst.push(date);
        for (var i = 1; i < 5; i++) {
            lst.push(date - i);
        }
        $scope.lstYear = lst;
    }
    listYear();

    $scope.lstDepartments = [];
    //lấy danh phòng ban
    var getAllDepartment = function () {
        return $http({
            url: "/Departments/getAllDropDown_ByParentId/",
            method: "GET",
            params: {
                parentId: $cookieStore.get("DeparmentId")
            }
        }).success(function (data) {
            $scope.lstDepartments = data;
        });
    }
    //getAllDepartment();

    // lấy danh sách tỉnh thành
    $scope.lstTinhThanh = [];
    var getlstTinhThanh = function () {
        crudService.getAll("/DmTinhThanh/getAllDropDown")
            .success(function (data) {
                $scope.lstTinhThanh = data.listTinhThanh;
            }).error(function (error) {
                $.notify("Lấy danh sách tỉnh thành bị lỗi.", "error");
            });;
    }
    getlstTinhThanh();

    // load data Report 1
    var getDataReport1 = function () {
        var dateSplit = "";
        var TuNgay = $scope.TuNgay;
        var DenNgay = $scope.DenNgay;
        if (TuNgay != null && TuNgay != "") {
            dateSplit = TuNgay.split('/');
            TuNgay = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
        }

        if (DenNgay != null && DenNgay != "") {
            dateSplit = DenNgay.split('/');
            DenNgay = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
        }

        return $http({
            url: "/BaoCao/BaoCao_DonThuTheoDiaBan",
            method: "GET",
            params: {
                IdTinhThanh: $scope.IdTinhThanh,
                TuNgay: TuNgay,
                DenNgay: DenNgay
            }
        })
        .success(function (data) {
            $scope.lstReport1 = data;
        }).error(function (error) {
            $.notify("Không tìm thấy dữ liệu!", "error");
        });
    }
    $scope.reloadReport1 = function () {
        $("#Report1").show();
        $("#Report2").hide();
        getDataReport1();
    }

    $scope.Export_DonThuTheoDiaBan = function () {
        var dateSplit = "";
        var TuNgay = $scope.TuNgay;
        var DenNgay = $scope.DenNgay;
        if (TuNgay != null && TuNgay != "") {
            dateSplit = TuNgay.split('/');
            TuNgay = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
        }

        if (DenNgay != null && DenNgay != "") {
            dateSplit = DenNgay.split('/');
            DenNgay = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
        }

        return $http({
            url: "/BaoCao/BC_DonThuTheoDiaBan",
            method: "GET",
            params: {
                IdTinhThanh: $scope.IdTinhThanh,
                TuNgay: TuNgay,
                DenNgay: DenNgay
            }
        }).success(function (data) {
            document.getElementById('BC_DonThuTheoDiaBan').src = data;
            $.notify("Xuất báo cáo thành công!", "success");
        }).error(function () {
            $.notify("Xuất báo cáo thất bại!", "error");
        });
    }

    // init data
    var init = function () {        
        $("#Report1").show();
        $("#Report2").hide();
        getDataReport1();
    }
    init();
})