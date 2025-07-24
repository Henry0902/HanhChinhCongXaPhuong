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

myApp.controller("ForgotPasswordJs",
    function ($location, $scope, $interval, $routeParams, crudService, myService, $http, $cookieStore, $timeout) {
        $scope.submitted = false;
        $scope.formData = {};
        $scope.formData.Mobile = '';
        $scope.formData.Password = '';
        $scope.formData.ConfirmPassword = '';
        $scope.numbers = [null, null, null, null, null, null];
        $scope.dataUser = null;
        $scope.phoneNumber = "";
        $scope.formatText = "Định dạng chữ, số và các ký tự đặc biệt (!, @, #, $, %, ^, &, *,(, ), -,+, _,).";
        $scope.currentStep = 1;
        $scope.timer;
        $scope.isCheckingOtp = false;
        $scope.countdown = 60;
        $scope.userPassword = null;
        $scope.isActive = 1;
        let inputRefs = [];
        $scope.setInputRef = function (index, event) {
            inputRefs[index] = event.target;
        };

        // Tạo mới captcha
        $scope.loadCaptcha = function () {
            $http.get('/Users/GenerateCaptcha')
                .then(function (response) {
                    $scope.captcha = response.data;
                    $scope.userAnswer = '';
                    $scope.message = '';
                });
        };

        // Gửi câu trả lời
        $scope.submitCaptcha = function () {
            $("#divLoading").show();
            $http.post('/Users/ValidateCaptcha', {
                UserAnswer: $scope.userAnswer,
                CorrectAnswer: $scope.captcha.answer,
                PhoneNumber: $scope.formData.Mobile
            })
                .then(function (response) {
                    $scope.message = response.data.message;
                    $scope.success = response.data.success;
                    if (response.data.success == true) {
                        $scope.sendOTP(1);
                    } else {
                        $http.get('/Users/GenerateCaptcha')
                            .then(function (response) {
                                $scope.captcha = response.data;
                                $scope.userAnswer = '';
                            });
                        $("#divLoading").hide();
                    }
                })
                .catch(function (error) {
                    $scope.message = error.data.message;
                    $scope.loadCaptcha();
                    $("#divLoading").hide();
                });
        };

        $scope.loadCaptcha();

        $scope.onConfirm = function () {
            if ($scope.isActive == 1) {
                if ($('#form-captcha').valid()) {
                    $scope.submitCaptcha();
                }
            } else {
                $scope.currentStep = $scope.isActive;
            }
        }

        $scope.onClose = function () {
            window.location.href = "/tai-khoan/quen-mat-khau";
        }

        $scope.onBack = function () {
            $scope.currentStep = 1;
        }

        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        $scope.onContinue = function () {
            let otp = $scope.numbers.join('');
            $("#divLoading").show();
            crudService.update("/Users/CheckOtp", { id: $scope.userPassword.Id, otp })
                .success(function (data) {
                    if (data && data.StatusCode == 200) {
                        setTimeout(() => {
                            $scope.currentStep = 3;
                            $scope.isActive = 3;
                            $("#divLoading").hide();
                        }, 1000);
                    } else {
                        $.notify(data.Message, "error");
                        $("#divLoading").hide();
                    }
                })
                .error(function () {
                    $.notify("OTP không hợp lệ", "error");
                    $scope.isCheckingOtp = false;
                    $("#divLoading").hide();
                });
        }

        $scope.onComplete = function () {
            if ($('#form-password').valid()) {
                $("#divLoading").show();
                crudService.get("/Users/GetById?id=", $scope.userPassword.IdCongDan)
                    .success(function (data) {
                        data.CreatedDate = parseDate(data.CreatedDate);
                        data.Password = $scope.formData.ConfirmPassword;
                        data.LastChangePassword = new Date();
                        crudService.update("/Users/ChangePassword", data)
                            .success(function (result) {
                                setTimeout(() => {
                                    crudService.remove("/Users/DeleteUserPassword", $scope.userPassword.Id);
                                    $("#divLoading").hide();
                                    $.notify("Lấy lại mật khẩu thành công", "success");
                                    $scope.currentStep = 4;
                                    $scope.isActive = 4;
                                }, 1000)
                            });
                    })
                    .error(function (error) {
                        $.notify("Không tìm thấy dữ liệu!", "error");
                    });
            }
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

        $scope.sendOTP = function (type) {
            $("#divLoading").show();
            crudService.update("/Users/SendOTP", { mobile: $scope.formData.Mobile })
                .success(function (data) {
                    if (data && data.StatusCode == 200) {
                        if (type == 1) {
                            $scope.currentStep = 2;
                            $scope.isActive = 2;
                        } else {
                            $scope.numbers = [null, null, null, null, null, null];
                        }
                        $timeout(function () {
                            let firstInput = document.getElementById("otp-input-0");
                            if (firstInput) {
                                firstInput.focus();
                            }
                        }, 100);
                        $scope.userPassword = data.Result;
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
                        $.notify(data.Message, "error");
                        $("#divLoading").hide();
                    }
                    $("#divLoading").hide();
                })
                .error(function () {
                    $.notify("Gửi OTP thất bại", "error");
                });
        }

        $scope.sendAgain = function () {
            $scope.sendOTP(2);
        };

        function updateCountdownDisplay() {
            var minutes = Math.floor($scope.countdown / 60);
            var seconds = $scope.countdown % 60;
            $scope.countdownDisplay = (minutes < 10 ? "0" + minutes : minutes) +
                ":" +
                (seconds < 10 ? "0" + seconds : seconds);
        }
    });

myApp.service("myService", function ($http) {

});
