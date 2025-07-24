myApp.controller("ProfileEditJs",
    function ($location, $scope, $interval, $routeParams, crudService, myService, $http, $cookieStore) {
        $scope.submitted = false;
        $scope.formData = {};
        $scope.formData.DisplayName = '';
        $scope.formData.Mobile = '';
        $scope.formData.Password = '';
        $scope.formData.ConfirmPassword = '';
        $scope.formData.Email = '';
        $scope.formData.Gender = "";
        $scope.formData.DateOfBirth = "";
        $scope.formData.IdCard = "";
        $scope.formData.IssuanceDate = "";
        $scope.formData.IssuanceAgency = "";
        $scope.formData.ProvinceId = "";
        $scope.formData.DistrictId = "";
        $scope.formData.CommuneId = "";
        $scope.formData.SpecifiedAddress = "";
        $scope.formData.EthnicId = "";
        $scope.formData.NationalityId = "";
        $scope.numbers = [0, 0, 0, 0, 0, 0];
        $scope.dataUser = null;
        $scope.phoneNumber = "";
        $scope.currentStep = -1;
        $scope.timer;
        $scope.lstQuocTich = [];
        $scope.lstDanToc = [];
        $scope.listProvince = [];
        $scope.listDistrict = [];
        $scope.listCommune = [];

        $scope.today = new Date();

        // Hàm kiểm tra ngày nhập vào
        $scope.validateDate = function () {
            if (!$scope.formData.DateOfBirth) return;

            // Chuyển đổi định dạng từ dd/MM/yyyy sang Date object
            let parts = $scope.formData.DateOfBirth.split('/');
            let inputDate = new Date(parts[2], parts[1] - 1, parts[0]); // YYYY, MM, DD

            // Kiểm tra nếu ngày nhập vào lớn hơn ngày hiện tại
            if (inputDate > $scope.today) {
                $scope.formData.DateOfBirth = ""; // Xóa giá trị nếu sai
            }
        };

        $scope.triggerFileInput = function () {
            document.getElementById("fileInput").click();
        };

        $scope.removeFile = function () {
            $scope.formData.FileName = null;
            $scope.formData.AvatarFile = null;
        }

        $scope.truncateText = function (text) {
            return text && text.length > 20 ? text.slice(0, 20) + "..." : text;
        }

        $scope.fileChanged = function (element) {
            var file = element.files[0];
            if (file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $scope.$apply(function () {
                        $scope.formData.AvatarFile = e.target.result.split(',')[1];
                        $scope.formData.FileName = file.name;
                    });
                };

                reader.readAsDataURL(file);
            }
        };

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

        $scope.redirectToPage = function () {
            window.location.href = "/thong-tin-ca-nhan/chi-tiet";
        };

        $scope.updateProfile = function (data) {
            console.log(data)
            $("#divLoading").show();
            crudService.update("/Users/UpdateProfile", data)
                .success(function () {
                    $.notify("Cập nhật thành công!", "success");
                    setTimeout(() => {
                        $("#divLoading").hide();
                        window.location.href = "/thong-tin-ca-nhan/chi-tiet";
                    }, 1000);
                })
                .error(function () {
                    $.notify("Cập nhật thất bại!", "error");
                    $("#divLoading").hide();
                });
        }

        $scope.submitRegister = function (type) {
            if ($('#validation-form').valid()) {
                $scope.formData.Status = type == 1 ? 0 : $scope.formData.Status;
                $scope.updateProfile($scope.formData);
            }
        };

        getbyID = function () {
            $("#divLoading").show();
            crudService.get("/Users/GetById?id=", $cookieStore.get("UserID"))
                .success(function (data) {
                    $scope.formData = data;
                    $scope.formData.Gender = $scope.formData.Gender ? 1 : 0;
                    $scope.formData.DateOfBirth = parseDateV($scope.formData.DateOfBirth);
                    $scope.formData.IssuanceDate = parseDateV($scope.formData.IssuanceDate);
                    $scope.formData.CreatedDate = parseDateV($scope.formData.CreatedDate);
                    getListProvince();
                    crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", $scope.formData.ProvinceId)
                        .success(function (data) {
                            $scope.listDistrict = data;
                        }).error(function (error) {
                            $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
                        });

                    crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", $scope.formData.DistrictId)
                        .success(function (data) {
                            $scope.listCommune = data;
                        }).error(function (error) {
                            $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                        });

                    $("#divLoading").hide();
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                    $("#divLoading").hide();
                });
        };


        var getlstQuocTich = function () {
            crudService.getAll("/DmQuocTich/getAll")
                .success(function (data) {
                    $scope.lstQuocTich = data;
                });
        }

        var getlstDanToc = function () {
            crudService.getAll("/DmDanToc/getAll")
                .success(function (data) {
                    $scope.lstDanToc = data;
                });
        }

        var getListProvince = function () {
            crudService.getAll("/DmTinhThanh/getAllDropDown")
                .success(function (data) {
                    $scope.listProvince = data.listTinhThanh;
                }).error(function (error) {
                    $.notify("Lấy danh sách tỉnh thành bị lỗi.", "error");
                });
        }


        $scope.getListDistrict = function (id) {
            crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", id)
                .success(function (data) {
                    $scope.listDistrict = data;
                    $scope.listCommune = [];
                    $scope.formData.DistrictId = "";
                    $scope.formData.CommuneId = "";
                }).error(function (error) {
                    $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
                });
        }

        $scope.getListCommune = function (id) {
            crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", id)
                .success(function (data) {
                    $scope.listCommune = data;
                    $scope.formData.CommuneId = "";
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                });
        }

        var init = function () {

            getlstDanToc();
            getlstQuocTich();

            getbyID();
        }
        init();
    });

myApp.service("myService", function ($http) {
    this.listDistrict = function () {
        return $http.get('/DmQuanHuyen/GetQuanHuyenByTinhThanh?id=' + id);
    }

    this.listCommune = function (id) {
        return $http.get('/DmPhuongXa/GetPhuongXaByQuanHuyen?id=' + id);
    }
});
