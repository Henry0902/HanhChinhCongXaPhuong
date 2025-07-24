var myApp = angular.module("myApp", ["ngRoute", "crud", 'ui.bootstrap', 'ngCookies'])
    .directive('trimInput', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                // Listen for the 'blur' event to trim the input value
                element.on('blur', function () {
                    var value = element.val().trim();
                    element.val(value);
                    ngModel.$setViewValue(value);
                    ngModel.$render();
                });
            }
        };
    });

myApp.controller("RegisterAccountJs",
    function ($location, $scope, $interval, $routeParams, crudService, myService, $http, $cookieStore, $timeout) {
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
        $scope.numbers = [null, null, null, null, null, null];

        $scope.dataUser = null;
        $scope.phoneNumber = "";
        $scope.userEmail = "";
        $scope.formatText = "Định dạng chữ, số và các ký tự đặc biệt (!, @, #, $, %, ^, &, *,(, ), -,+, _,).";
        $scope.currentStep = -1;
        $scope.timer;
        $scope.lstQuocTich = [];
        $scope.lstDanToc = [];
        $scope.listProvince = [];
        $scope.listDistrict = [];
        $scope.listCommune = [];
        $scope.isCheckingOtp = false;
        $scope.countdown = 60;


        let inputRefs = [];
        $scope.setInputRef = function (index, event) {
            inputRefs[index] = event.target;
        };

        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        var createUser = function () {
            const user = {
                UserName: $scope.dataUser.UserName,
                Password: $scope.dataUser.Password,
                DisplayName: $scope.dataUser.DisplayName,
                Avatar: $scope.dataUser.Avatar,
                Gender: $scope.dataUser.Gender,
                Mobile: $scope.dataUser.Mobile,
                Email: $scope.dataUser.Email,
                DateOfBirth: parseDate($scope.dataUser.DateOfBirth),
                IsLocked: false,
                IsDeleted: false,
                Status: 0,
                IsSuper: 2,
                IdCard: $scope.dataUser.IdCard,
                IssuanceDate: parseDate($scope.dataUser.IssuanceDate),
                IssuanceAgency: $scope.dataUser.IssuanceAgency,
                EthnicId: $scope.dataUser.EthnicId,
                NationalityId: $scope.dataUser.NationalityId,
                SpecifiedAddress: $scope.dataUser.SpecifiedAddress,
                ProvinceId: $scope.dataUser.ProvinceId,
                DistrictId: $scope.dataUser.DistrictId,
                CommuneId: $scope.dataUser.CommuneId,
            };

            crudService.create("/Users/Create", user)
                .success(function (result) {
                    crudService.remove("/UsersTemp/Delete", $scope.dataUser.Id);
                    $.notify("Đăng ký tài khoản thành công", "success");
                    setTimeout(() => {
                        $('#fuelux-wizard-container').wizard('next');
                        $("#divLoading").hide();
                    }, 1000)
                })
                .error(function (error) {
                    $.notify("Đăng ký tài khoản thất bại. Hãy thử lại!", "error");
                    $("#divLoading").hide();
                });
        }

        $scope.sendEmail = function (data, type) {
            let body = {
                To: data.To,
                Subject: 'OTP xác nhận đăng ký tài khoản',
                Body: `<p>Xin chào,</p><p>Mã OTP của bạn là: <strong>${data.Body}</strong></p><p>OTP này có hiệu lực trong 1 phút.</p>`
            };
            $http.post('/Email/SendEmail', body).then(function (response) {
                if (response.data == true) {
                    $.notify("Gửi OTP thành công. Vui lòng nhập mã được gửi về email.", "success");
                    $("#divLoading").hide();

                    if (type == 1) {
                        $('#fuelux-wizard-container').wizard('next');
                        document.getElementById(`otp-input-0`).focus();
                    }

                    $scope.countdown = 60;
                    updateCountdownDisplay();
                    $scope.timer = $interval(function () {
                        if ($scope.countdown > 0) {
                            $scope.countdown--;
                            updateCountdownDisplay();
                        } else {
                            $interval.cancel($scope.timer);
                        }
                    }, 1000);
                } else {
                    $("#divLoading").hide();
                    $.notify("Gửi OTP thất bại", "error");
                }
            }).catch(function (error) {
                $.notify("Gửi OTP thất bại", "error");
                $("#divLoading").hide();
            });
        }

        $scope.checkNull = function () {
            return $scope.numbers.some(item => item == null || item == '');
        }

        $scope.moveNext = function (event, index) {
            const key = event.key;
            if (key === "Backspace" && !$scope.numbers[index] && index > 0) {
                document.getElementById(`otp-input-${index - 1}`).focus();
            } else if (key === "Delete") {
                $scope.numbers[index] = "";
            } else if ($scope.numbers[index] && index < $scope.numbers.length - 1) {
                document.getElementById(`otp-input-${index + 1}`).focus();
            }
        };

        $scope.listCheck = [];
        $scope.isSend = false;
        $('#fuelux-wizard-container')
            .ace_wizard({
            })
            .on('actionclicked.fu.wizard', function (e, info) {
                if (info.step == 1) {
                    if (info.direction == "next") {
                        if (!$('#validation-form').valid()) {
                            e.preventDefault();
                        } else {
                            if ($scope.currentStep == 1 && $scope.isSend == true) {
                                $scope.currentStep = 2;
                                $scope.$apply();
                            }

                            if ($scope.isSend == true) return;
                            e.preventDefault();
                            $("#divLoading").show();
                            $scope.formData.Password = $scope.formData.ConfirmPassword;
                            $scope.phoneNumber = $scope.formData.Mobile;
                            $scope.userEmail = $scope.formData.Email;
                            let params = { phoneNumber: $scope.formData.Mobile, email: $scope.formData.Email, idCard: $scope.formData.IdCard };
                            crudService.update("/usersTemp/deleteUserExisted", params)
                                .success(function (res) {
                                    if (res == true) {
                                        crudService.create("/UsersTemp/Create", $scope.formData)
                                            .success(function (data) {
                                                if (data && data.StatusCode == 200) {
                                                    $scope.dataUser = data.Result;
                                                    $scope.isSend = true;
                                                    $scope.currentStep = 2;
                                                    $scope.sendEmail({ To: $scope.dataUser.Email, Body: $scope.dataUser.Otp }, 1);
                                                } else {
                                                    $scope.listCheck = data.Result;
                                                    $scope.currentStep = 1;
                                                    //$scope.$apply();
                                                    $("#divLoading").hide();
                                                }
                                            })
                                            .error(function (error) {
                                                $("#divLoading").hide();
                                                $.notify("Tạo mới không thành công. Hãy thử lại!", "error");
                                            });
                                    } else {
                                        $("#divloading").hide();
                                        $.notify("Có lỗi xảy ra", "error");
                                    }
                                })
                                .error(function (error) {
                                    $("#divloading").hide();
                                    $.notify("Có lỗi xảy ra", "error");
                                });
                        }
                    }
                } else if (info.step == 2) {
                    if (info.direction == "next") {
                        let otp = $scope.numbers.join('');
                        if ($scope.isCheckingOtp) return;
                        e.preventDefault();
                        $scope.isCheckingOtp = true;
                        $("#divLoading").show();
                        crudService.update("/UsersTemp/CheckOtp", { id: $scope.dataUser.Id, otp })
                            .success(function (data) {
                                if (data && data.StatusCode == 200) {
                                    $scope.currentStep = 3;
                                    createUser();
                                    $scope.isCheckingOtp = true;
                                } else {
                                    $.notify(data.Message, "error");
                                    $scope.isCheckingOtp = false;
                                    $("#divLoading").hide();
                                }
                            })
                            .error(function () {
                                $.notify("OTP không hợp lệ", "error");
                                $scope.isCheckingOtp = false;
                                $("#divLoading").hide();
                            });
                    } else {
                        $scope.currentStep = 1;
                        $scope.$apply();
                    }
                }
            })
            .on('finished.fu.wizard', function (e) {
            })
            .on('stepclick.fu.wizard', function (e) {
            });

        $('.btn-prev').removeClass('disabled').prop('disabled', false).off('click').on('click', function () {
            let currentStep = $('#fuelux-wizard-container').wizard('selectedItem').step;

            // Kiểm tra nếu đang ở bước 1 → Redirect
            if (currentStep === 1) {
                window.location.href = "/";
                return;
            }

            // Ngược lại, thực hiện quay lại bước trước đó
            $('#fuelux-wizard-container').wizard('previous');
        });

        function updateCountdownDisplay() {
            var minutes = Math.floor($scope.countdown / 60);
            var seconds = $scope.countdown % 60;
            $scope.countdownDisplay = (minutes < 10 ? "0" + minutes : minutes) +
                ":" +
                (seconds < 10 ? "0" + seconds : seconds);
        }

        $scope.sendAgain = function () {
            $("#divLoading").show();
            crudService.update("/UsersTemp/SendOTP", { id: $scope.dataUser.Id })
                .success(function (data) {
                    if (data && data.StatusCode == 200) {
                        $scope.numbers = [null, null, null, null, null, null];
                        $timeout(function () {
                            let firstInput = document.getElementById("otp-input-0");
                            if (firstInput) {
                                firstInput.focus();
                            }
                        }, 100);
                        $scope.sendEmail({ To: data.Result.Email, Body: data.Result.Otp });
                    } else {
                        $.notify(data.Message, "error");
                        $("#divLoading").hide();
                    }
                })
                .error(function () {
                    $.notify("Gửi OTP thất bại", "error");
                });
        };

        $scope.submitForm = function () {
            $scope.submitted = true;
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
                });;
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
            getListProvince();
            getlstDanToc();
            getlstQuocTich();
        }
        init();
    });

myApp.service("myService", function ($http) {

});
