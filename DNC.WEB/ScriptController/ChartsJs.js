myApp.controller("Charts",
function ($location, $scope, $routeParams, crudService, $http, $cookieStore, $filter) {
    //Khai báo dữ liệu
    $scope.dsChart1 = {};
    $scope.lstChart1 = [];
    //
    $scope.dsChart2 = {};
    $scope.lstChart2 = [];
    //
    $scope.dsChart3 = {};
    $scope.lstChart3 = [];
    //
    $scope.dsChart4 = {};
    $scope.lstChart4 = [];

    $scope.lstDonVi = [];
    $scope.SelectlstDonVi = [];
    $scope.lstCoQuanBanHanh = [];
    $scope.SelectlstCoQuanBanHanh = [];

    $scope.currentYear = new Date().getFullYear();
    $scope.Year1 = $scope.currentYear;
    $scope.Year2 = $scope.currentYear;
    $scope.Year3 = $scope.currentYear;
    $scope.Cap = 0;
    $scope.DonViId = $cookieStore.get("DeparmentId");
    $scope.CapDonVi = $cookieStore.get("CapDonVi");
    $scope.UserID = $cookieStore.get("UserID");

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

    // load data chart 1
    var getDataChart1 = function () {
        return $http({
            url: "/BaoCao/BaoCao_ThongKeDonThuTiepNhan",
            method: "GET",
            params: {
                nam: $scope.Year1
            }
        })
        .success(function (data) {
            $scope.lstChart1 = data;
            $scope.lstLabel = [];
            $scope.lstDataDonThu = [];
            angular.forEach(data, function (item) {
                $scope.lstLabel.push({ 'label': item.Name });
                $scope.lstDataDonThu.push({ 'value': item.sl });
            });
            $scope.dsChart1 = {
                'chart': {
                    'caption': 'Đơn thư tiếp nhận ',
                    'captionFontSize': '25',
                    'xAxisName': 'Năm ban hành',
                    'yAxisName': 'Số lượng',
                    'theme': 'zune',
                    "divLineDashed": 1,
                    "divLineDashLen": 1,
                    "palette": 3,
                    "exportEnabled": 1,
                    "paletteColors": "#0075c2,#1aaf5d, #8e0000, #A66EDD",
                    "exportFileName": "So sánh kết quả đánh giá",
                    "captionPadding": 20,
                    "rotateValues": 0
                },
                'categories': [
                    {
                        'category': $scope.lstLabel,
                        "stepSkipped": false
                    }
                ],
                "dataset": [
                    {
                        "seriesname": 'Số lượng hồ sơ',
                        "color": "#1aaf5d",
                        "data": $scope.lstDataDonThu
                    }
                ]
            }
        });
    }
    $scope.reloadChart1 = function () {
        $("#chart1").show();
        $("#chart2").hide();
        $("#chart3").hide();
        $("#chart4").hide();
        getDataChart1();
    }

    // load data chart 2
    var getDataChart2 = function () {
        return $http({
            url: "/BaoCao/BaoCao_ThongKeDonThuGiaiQuyet",
            method: "GET",
            params: {
                nam: $scope.Year2
            }
        })
        .success(function (data) {
            $scope.lstChart2 = data;
            $scope.lstLabel = [];
            $scope.lstDataDonThu = [];
            angular.forEach(data, function (item) {
                $scope.lstLabel.push({ 'label': item.Name });
                $scope.lstDataDonThu.push({ 'value': item.sl });
            });
            $scope.dsChart2 = {
                'chart': {
                    'caption': 'Đơn thư thụ lý giải quyết ',
                    'captionFontSize': '25',
                    'xAxisName': 'Năm ban hành',
                    'yAxisName': 'Số lượng',
                    'theme': 'zune',
                    "divLineDashed": 1,
                    "divLineDashLen": 1,
                    "palette": 3,
                    "exportEnabled": 1,
                    "paletteColors": "#0075c2,#1aaf5d, #8e0000, #A66EDD",
                    "exportFileName": "So sánh kết quả đánh giá",
                    "captionPadding": 20,
                    "rotateValues": 0
                },
                'categories': [
                    {
                        'category': $scope.lstLabel,
                        "stepSkipped": false
                    }
                ],
                "dataset": [
                    {
                        "seriesname": 'Số lượng hồ sơ',
                        "color": "#1aaf5d",
                        "data": $scope.lstDataDonThu
                    }
                ]
            }
        });
    }
    $scope.reloadChart2 = function () {
        $("#chart1").hide();
        $("#chart2").show();
        $("#chart3").hide();
        $("#chart4").hide();
        getDataChart2();
    }
    // load data chart 3
    var getDataChart3 = function () {
        return $http({
            url: "/BaoCao/BaoCao_ThongKeKetQuaTiepDan",
            method: "GET",
            params: {
                nam: $scope.Year3
            }
        })
        .success(function (data) {
            $scope.lstChart3 = data;
            $scope.lstLabel = [];
            $scope.lstDataTiepCongDan = [];
            $scope.lstDataDonThu = [];
            angular.forEach(data, function (item) {
                $scope.lstLabel.push({ 'label': item.Name });
                $scope.lstDataTiepCongDan.push({ 'value': item.SLTiepCongDan });
                $scope.lstDataDonThu.push({ 'value': item.SLDonThu });
            });
            $scope.dsChart3 = {
                'chart': {
                    'caption': ' Tiếp công dân - Tiếp có đơn ',
                    'captionFontSize': '25',
                    'xAxisName': 'Cơ quan ban hành',
                    'yAxisName': 'Số lượng',
                    'theme': 'zune',
                    "divLineDashed": 1,
                    "divLineDashLen": 1,
                    "palette": 3,
                    "exportEnabled": 1,
                    "paletteColors": "#0075c2,#1aaf5d, #8e0000, #A66EDD",
                    "exportFileName": "So sánh kết quả đánh giá",
                    "captionPadding": 20,
                    "rotateValues": 0
                },
                'categories': [
                    {
                        'category': $scope.lstLabel,
                        "stepSkipped": false
                    }
                ],
                "dataset": [
                    {
                        "seriesname": 'Tiếp công dân',
                        "color": "#1aaf5d",
                        "data": $scope.lstDataTiepCongDan
                    },
                    {
                        "seriesname": 'Tiếp có đơn',
                        "color": "#ef4848",
                        "data": $scope.lstDataDonThu
                    }
                ]
            }
        });
    }
    $scope.reloadChart3 = function () {
        $("#chart1").hide();
        $("#chart2").hide();
        $("#chart3").show();
        $("#chart4").hide();
        getDataChart3();
    }

    var getAllDepartment = function () {
        crudService.getAll("/Departments/getAllDropDown")
            .success(function (data) {
                $scope.lstDonVi = data;
            });
    }
    getAllDepartment();

    $scope.SelectDonVi = function (id) {
        var check = $("#DonVi" + id).prop("checked");
        if (check == true) {
            $scope.SelectlstDonVi.push(id);
        }
        else {
            for (var i = 0; i < $scope.SelectlstDonVi.length; i++) {
                if ($scope.SelectlstDonVi[i] == id) {
                    $scope.SelectlstDonVi.splice(i, 1);
                    break;
                }
            }
        }
    }

    // load data chart 4
    var getDataChart4 = function () {
        var DonViID = $scope.SelectlstDonVi.toString();
        return $http({
            url: "/BaoCao/BaoCao_Hoso_ByDonVi",
            method: "GET",
            params: {
                nam: $scope.Year,
                donvi_id: DonViID
            }
        })
        .success(function (data) {
            $scope.lstChart4 = data;
            $scope.lstLabel = [];
            $scope.lstDataHoso = [];
            angular.forEach(data, function (item) {
                $scope.lstLabel.push({ 'label': item.Name });
                $scope.lstDataHoso.push({ 'value': item.Hoso });
            });
            $scope.dsChart4 = {
                'chart': {
                    'caption': 'Thống kê số lượng hồ sơ theo đơn vị quản lý ',
                    'captionFontSize': '25',
                    'xAxisName': 'Đơn vị quản lý',
                    'yAxisName': 'Số lượng',
                    'theme': 'zune',
                    "divLineDashed": 1,
                    "divLineDashLen": 1,
                    "palette": 3,
                    "exportEnabled": 1,
                    "paletteColors": "#0075c2,#1aaf5d, #8e0000, #A66EDD",
                    "exportFileName": "So sánh kết quả đánh giá",
                    "captionPadding": 20,
                    "rotateValues": 0
                },
                'categories': [
                    {
                        'category': $scope.lstLabel,
                        "stepSkipped": false
                    }
                ],
                "dataset": [
                    {
                        "seriesname": 'Số lượng hồ sơ',
                        "color": "#ef4848",
                        "data": $scope.lstDataHoso
                    }
                ]
            }
        });
    }
    $scope.reloadChart4 = function () {
        $("#chart1").hide();
        $("#chart2").hide();
        $("#chart3").hide();
        $("#chart4").show();
        getDataChart4();
    }

    // init data
    var init = function () {
        $("#chart1").show();
        $("#chart2").hide();
        $("#chart3").hide();
        $("#chart4").hide();
        $scope.Cap = $('#cbCap').val();
        getDataChart1();
    }
    init();
})