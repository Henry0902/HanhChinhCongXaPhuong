myApp.controller("HomeJs",
function ($location, $scope, $routeParams, crudService, $timeout, $http, $cookieStore, $filter) {
    $scope.currentYear = new Date().getFullYear();
    $scope.cap = -1;
    $scope.keyword = '';
    $scope.lstBoChiTieu = [];
    $scope.lstNews = [];
    $scope.lstKetQuaTheoNam = [];
    $scope.lstRanks = [];
    $scope.lstYear = [];

    $scope.dsChart1 = {};
    $scope.lstChart1 = [];

    $scope.dsChart2 = {};
    $scope.lstChart2 = [];

    $scope.namID = '';
    $scope.Year = 2018;

    $scope.Order = 'desc';
    $scope.pageIndex = 1;
    $scope.pageSize = 10;

    var parseDate = function (value) {
        if (value) {
            return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
        }
        return null;
    }

    var getNam = function () {
        var year = new Date().getFullYear();
        var range = [];
        //range.push(year);
        for (var i = 2017; i < year + 10; i++) {
            range.push(i);
        }
        $scope.lstYear = range;
        $('#Nam').val($scope.currentYear);
        $scope.Year = $('#Nam').val();
    }
    getNam();

    //Lấy danh sách năm
    $scope.lstNam = [];
    var getListNam = function () {
        var currentTime = new Date();
        var nam = currentTime.getFullYear();
        $scope.namID = nam;
        for (i = nam; i > nam - 5; i--) {
            $scope.lstNam.push(i);
        }
    }
    getListNam();

    // load data chart 1
    var getDataChart1 = function () {
        return $http({
            url: "/BaoCao/BaoCao_TiepCongDan_DonThu_ByYear",
            method: "GET",
            params: {
                Nam: $scope.namID
            }
        })
        .success(function (data) {
            $scope.lstChart1 = data;
            $scope.lstLabel = [];
            $scope.lstDataTiepCongDan= [];
            $scope.lstDataDonThu = [];
            angular.forEach(data, function (item) {
                $scope.lstLabel.push({ 'label': 'Tháng ' + item.Thang });
                $scope.lstDataTiepCongDan.push({ 'value': item.TiepCongDan });
                $scope.lstDataDonThu.push({ 'value': item.DonThu });
            });
            $scope.dsChart1 = {
                'chart': {
                    'caption': 'Biểu đồ tiếp nhận hồ sơ - tiếp công dân',
                    'captionFontSize': '14',
                    'captionFontColor': '#116bb5',
                   // 'xAxisName': 'Năm ban hành',
                   // 'yAxisName': 'Số lượng',
                    'theme': 'zune',
                    "divLineDashed": 1,
                    "divLineDashLen": 1,
                    "palette": 3,
                    "exportEnabled": 1,
                    "paletteColors": "#5d62b5,#54cfcb, #A66EDD",
                    "exportFileName": "So sánh kết quả đánh giá",
                    "captionPadding": 10,
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
                        "seriesname": 'Đơn thư',
                        "data": $scope.lstDataDonThu
                    },
                    {
                        "seriesname": 'Tiếp công dân',
                        "renderAs": "area",
                        "data": $scope.lstDataTiepCongDan
                    }                    
                ]
            }
        });
    }

    // load data chart 2
    var getDataChart2 = function () {
        return $http({
            url: "/BaoCao/BaoCao_DonThuTheoLoai",
            method: "GET",
            params: {
                Nam: $scope.namID
            }
        })
            .success(function (data) {
                $scope.dsChart2 = {
                    'chart': {
                        'caption': 'Biểu đồ hồ sơ theo loại',
                        'captionFontSize': '14',
                        'captionFontColor': '#116bb5',
                        'theme': 'zune',
                        "divLineDashed": 1,
                        "divLineDashLen": 1,
                        "palette": 3,
                        "exportEnabled": 1,
                        "paletteColors": "#5d62b5,#54cfcb, #A66EDD, #FFFF00, #00FFFF, #FF00FF, #C0C0C0, #808080, #800000, #808000, #008000, #800080, #008080, #000080",
                        "exportFileName": "SỐ LIỆU TIẾP NHẬN ĐƠN THƯ",
                        "captionPadding": 10,
                        "rotateValues": 0,
                        "showLabels": 0,
                        "showValues": 0
                    },
                    "data": data
                }
            });
    }

    //Load danh sách data
    var getData = function () {
        return $http({
            url: "/DonThu/SearchQuaHan",
            method: "GET",
            params: {
                order: $scope.Order, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex
            }
        });
    }
    $scope.lstDonThu = [];
    // Init data
    var initDonThu = function () {
        $("#divLoading").show();
        getData()
            .success(function (data) {
                angular.forEach(data, function (item) {
                    item.NgayNhap = parseDate(item.NgayNhap);
                });
                $scope.lstDonThu = data;
                if (data.length > 0) {
                    $scope.totalRecords = data[0].TotalRecords;
                } else $scope.totalRecords = 0;
                $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
                for (i = 1; i <= $scope.pageCount && i <= 9; i++) {
                    $scope.lstPage.push(i);
                }
                $("#divLoading").hide();
            })
            .error(function (error) {
                console.log(error);
                $("#divLoading").hide();
            });
    }

    initDonThu();

    var init = function () {
        //$scope.keyword = $cookieStore.get("keyword");
         //var current = new Date();
         //$scope.currentYear = current.getFullYear();
         $scope.cap = $cookieStore.get("CapDonVi");
         $scope.donvi = $cookieStore.get("DeparmentId");

         $("#chart1").show();
        getDataChart1();

        $("#chart2").show();
        getDataChart2();
     }
    init();

    var getNews = function () {
        return $http({
            url: "/News/GetAllPuslished",
            method: "GET",
            params: { count: 5}
        }).success(function (data) {
            $scope.lstNews = data;
        });
    }
 //   getNews();
    $scope.changeYear = function () {
        getDataChart1();
        getDataChart2();
    }
    $scope.searchHoSo = function () {
        var value = $("#txtFilter2").val();
        //$cookieStore.put("keyword", value);
        crudService.setCookie('keyword', value);
        window.location.href = '../HoSo/Timkiem';
    }
    $scope.searchTaiLieu = function () {
        var value = $("#txtFilter1").val();
        //$cookieStore.put("keyword", value);
        crudService.setCookie('keyword', value);
        window.location.href = '../Docs/Timkiem';
    }    
});