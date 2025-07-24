myApp.controller("ProfileJs",
    function ($location, $q, $scope, $interval, $routeParams, crudService, myService, $http, $cookieStore) {
        $scope.formData = {};
        $scope.userId = $cookieStore.get("UserID");
        $scope.lstQuocTich = [];
        $scope.lstDanToc = [];
        $scope.dataProvince = {};
        //khai báo dữ liệu
        var lblpass = document.getElementById("lblpass");
        var lblopass = document.getElementById("lbloldpass");
        var lblpassconfirm = document.getElementById("lblpassconfirm");
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        $scope.passwordData = {};
        $scope.showChangePasswordModal = function () {
            // Xóa dữ liệu cũ
            $scope.passwordData = {};

            if ($scope.passwordForm) {
                // Reset trạng thái validate của form
                $scope.passwordForm.$setPristine();
                $scope.passwordForm.$setUntouched();
            }

            // Hiển thị modal
            $('#changePasswordModal').modal('show');
        };

        $scope.savePass = function () {
            if ($scope.passwordForm.$invalid) {
                // Đánh dấu các trường là "touched" để hiển thị lỗi
                angular.forEach($scope.passwordForm.$error, function (field) {
                    angular.forEach(field, function (errorField) {
                        errorField.$setTouched();
                    });
                });
                return; // Dừng lại nếu form chưa hợp lệ
            }

            // Nếu hợp lệ, thực hiện đổi mật khẩu
            crudService.get("/Users/GetById?id=", $scope.userId)
                .success(function (data) {
                    data.CreatedDate = parseDate(data.CreatedDate);
                    data.DateOfBirth = parseDate(data.DateOfBirth);
                    data.IssuanceDate = parseDate(data.IssuanceDate);
                    var databyid = data;
                    databyid.Password = $scope.passwordData.newPassword;
                    databyid.LastChangePassword = new Date();
                    crudService.update("/Users/ChangePassword", databyid)
                        .success(function () {
                            $("#changePasswordModal").modal("hide");
                            $.notify("Thay đổi mật khẩu thành công!", "success");
                        });
                })
                .error(function () {
                    $("#changePasswordModal").modal("hide");
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };

        $scope.redirectUpdate = function () {
            window.location.href = "/thong-tin-ca-nhan/chinh-sua";
        };

        var init = function () {
            $("#divLoading").show();
            var quocTichPromise = myService.getlstQuocTich();
            var danTocPromise = myService.getlstDanToc();
            var provincePromise = myService.listProvince();

            // Chờ cả hai API hoàn thành
            $q.all([quocTichPromise, danTocPromise, provincePromise]).then(function (responses) {
                $scope.lstQuocTich = responses[0].data;
                $scope.lstDanToc = responses[1].data;
                $scope.dataProvince = responses[2].data;

                // Sau khi cả hai API hoàn tất, gọi getById()
                return myService.getbyID($scope.userId);
            }).then(function (response) {
                $scope.formData = response.data;
                $scope.formData.DateOfBirth = parseDate($scope.formData.DateOfBirth);
                $scope.formData.IssuanceDate = parseDate($scope.formData.IssuanceDate);
                $scope.formData.CreatedDate = parseDate($scope.formData.CreatedDate);
                $scope.formData.EthnicId = $scope.lstDanToc.find(x => x.id == $scope.formData.EthnicId)?.TenDanToc;
                $scope.formData.NationalityId = $scope.lstQuocTich.find(x => x.id == $scope.formData.NationalityId)?.TenQuocTich;

                let commune = $scope.dataProvince.listPhuongXa.find(x => x.Id == response.data.CommuneId)?.Name;
                if (commune) {
                    $scope.formData.SpecifiedAddress += ", " + commune;
                }

                let district = $scope.dataProvince.listQuanHuyen.find(x => x.Id == response.data.DistrictId)?.Name;
                if (district) {
                    $scope.formData.SpecifiedAddress += ", " + district;
                }

                let province = $scope.dataProvince.listTinhThanh.find(x => x.Id == response.data.ProvinceId)?.Name;
                if (province) {
                    $scope.formData.SpecifiedAddress += ", " + province;
                }

                $("#divLoading").hide();
            }).catch(function (error) {
                $.notify("Có lỗi xảy ra", "error");
                console.error("Lỗi khi tải dữ liệu:", error);
                $("#divLoading").hide();
            });
        }
        init();
    });
myApp.service("myService", function ($http) {
    this.getlstQuocTich = function () {
        return $http.get('/DmQuocTich/getAll');
    }

    this.getlstDanToc = function () {
        return $http.get('/DmDanToc/getAll');
    }

    this.getbyID = function (id) {
        return $http.get("/Users/GetById?id=" + id);
    };

    this.listProvince = function () {
        return $http.get('/DmTinhThanh/getAllDropDown');
    }
});