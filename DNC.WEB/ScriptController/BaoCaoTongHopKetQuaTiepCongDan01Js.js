myApp.controller("BaoCaoTongHopKetQuaTiepCongDan01Js",
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
    $scope.TuNgay = '';
    $scope.DenNgay = '';

    $scope.position = "position: sticky;";

    // Hàm tính tổng lstReport1
    $scope.calculateTotals = function () {
        $scope.totals = {
            TSLuot: 0,
            TSNguoi: 0,
            TSVuViec: 0,

            TTXSoLuot: 0,
            TTXSoNguoi: 0,
            TTXVuViecLanDau: 0,
            TTXVuViecNhieuLan: 0,
            TTXSoDoan: 0,
            TTXSoNguoiTrongDoan: 0,
            TTXSoDoanLanDau: 0,
            TTXSoDoanNhieuLan: 0,

            TDKSoky: 0,
            TDKSoLuot: 0,
            TDKSoNguoi: 0,
            TDKVuViecLanDau: 0,
            TDKVuViecNhieuLan: 0,
            TDKSoDoan: 0,
            TDKSoNguoiTrongDoan: 0,
            TDKSoDoanLanDau: 0,
            TDKSoDoanNhieuLan: 0,

            TDK_UQSoKy: 0,
            TDK_UQSoLuot: 0,
            TDK_UQSoNguoi: 0,
            TDK_UQVuViecLanDau: 0,
            TDK_UQVuViecNhieuLan: 0,
            TDK_UQSoDoan: 0,
            TDK_UQSoNguoiTrongDoan: 0,
            TDK_UQSoDoanLanDau: 0,
            TDK_UQSoDoanNhieuLan: 0
        };

        angular.forEach($scope.lstReport1, function (item) {
            angular.forEach($scope.totals, function (_, key) {
                $scope.totals[key] += item[key] || 0;
            });
        });
    };

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


    //lấy danh phòng ban
    $scope.lstDepartments = [];
    var getDepartmentLevel1 = function () {
        return $http({
            url: "/Departments/getDropDownLevel1_ByParentId/",
            method: "GET",
            params: {
                parentId: $cookieStore.get("DeparmentId")
            }
        }).success(function (data) {
            $scope.lstDepartments = data;
            console.log("getDropDownLevel1_ByParentId", data);
        });
    }
    getDepartmentLevel1();

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
            url: "/BaoCao/BaoCao_TongHopKetQuaTiepCongDan01",
            method: "GET",
            params: {
                IdDonVi: $scope.IdDonVi,
                TuNgay: TuNgay,
                DenNgay: DenNgay
            }
        })
        .success(function (data) {
            $scope.lstReport1 = data;
            console.log("reprot1", data);
            $scope.calculateTotals(); // <-- Gọi tính tổng ngay sau khi load dữ liệu
        }).error(function (error) {
            $.notify("Không tìm thấy dữ liệu!", "error");
        });
    }
    $scope.reloadReport1 = function () {
        $("#Report1").show();
        $("#Report2").hide();
        getDataReport1();
    }

    //export BC_TongHopTiepCongDan
    $scope.Export_TongHopKetQuaTiepCongDan01 = function () {
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
            url: "/BaoCao/BC_TongHopKetQuaTiepCongDan01",
            method: "GET",
            params: {
                IdDonVi: $scope.IdDonVi,
                TuNgay: TuNgay,
                DenNgay: DenNgay
            }
        }).success(function (data) {
            document.getElementById('BC_TongHopKetQuaTiepCongDan01').src = data;
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