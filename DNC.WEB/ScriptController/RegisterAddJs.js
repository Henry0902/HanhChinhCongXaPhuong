
myApp.controller("RegisterAddJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore) {
        $scope.formData = {};
        $scope.fileList = [];
        $scope.validateForm = function () {
            $scope.registrationForm.IdDonViTiepDan.$setTouched();
            $scope.registrationForm.NgayDangKy.$setTouched();
            $scope.registrationForm.ChuDe.$setTouched();
        };

        function generateCode(departmentCode, date, sequence) {
            const ddmmyy = date.toISOString().slice(0, 10).split('-').reverse().join('').slice(0, 6);
            const sequenceFormatted = String(sequence).padStart(4, '0');
            return `${departmentCode}-${ddmmyy}-${sequenceFormatted}`;
        }

        var create = function (formData) {
            $("#divLoading").show();
            crudService.create("/DangKyTiepDan/Create", formData)
                .success(function (data) {
                    if (data && data.StatusCode == 200) {
                        //Upload file sau khi tạo hồ sơ xong                    
                        if ($scope.fileList.length != 0) {
                            var count = 0;
                            for (var i = 0; i < $scope.fileList.length; i++) {
                                fileUploadService.UploadedFile($scope.fileList[i], data.Result.Id).then(function (res) {
                                    if (i == $scope.fileList.length) {
                                        setTimeout(() => {
                                            $scope.fileList = [];
                                            $("#divLoading").hide();
                                            window.location.href = "/dang-ky-tiep-dan/danh-sach";
                                        }, 1000);
                                    }
                                });
                            }
                        } else {
                            setTimeout(() => {
                                $scope.fileList = [];
                                $("#divLoading").hide();
                                window.location.href = "/dang-ky-tiep-dan/danh-sach";
                            }, 1000);
                        }

                        $.notify(data.Message, "success");
                    } else {
                        $.notify("Thêm mới đăng ký tiếp dân thất bại", "error");
                    }
                })
                .error(function (error) {
                    $.notify("Thêm mới không thành công. Hãy thử lại!", "error");
                });
        }

        $scope.removeFile = function (index) {
            $scope.fileList.splice(index, 1);
        }

        $scope.saveForm = function (status) {
            $scope.validateForm();
            if ($scope.registrationForm.$valid) {
                $scope.formData.IdCongDan = $cookieStore.get("UserID");
                $scope.formData.TrangThai = status;
                var code = $scope.lstDepartments.find(x => x.Id == $scope.formData.IdDonViTiepDan)?.Code;
                if (code) {
                    $scope.formData.MaDangKy = generateCode(code, new Date(), 1);
                    create($scope.formData);
                } else {
                    $.notify("Có lỗi xảy ra", "error");
                }
            }
        };

        $scope.validateFiles = function (event) {
            let files = event.files;
            if (files.length > 5) {
                $.notify("Chỉ được chọn tối đa 5 file", "error");
                return;
            }

            for (let file of files) {
                let fileExt = file.name.split('.').pop().toLowerCase();
                if (!["pdf", "jpg", "jpeg", "doc", "docx", "xcl"].includes(fileExt)) {
                    $.notify("Chỉ chấp nhận file PDF, JPG, JPEG, DOC, DOCX, XCL.", "error");
                    return;
                }
                if (file.size > 20 * 1024 * 1024) {
                    $.notify("Kích thước file không được vượt quá 20MB.", "error");
                    return;
                }
                $scope.fileList.push(file);
            }

            $scope.$apply();
        };

        $scope.redirectToPage = function () {
            window.location.href = "/dang-ky-tiep-dan/danh-sach"; 
        };

        $scope.triggerFileInput = function () {
            document.getElementById('file').click();
        };

        $scope.lstDepartments = [];
        //lấy danh phòng ban
        var getAllDepartment = function () {
            crudService.getAll("/Departments/getAllDropDown_Search")
                .success(function (data) {
                    $scope.lstDepartments = data;
                });
        }
        getAllDepartment();
    }).factory('fileUploadService', function ($http, $q) {
        var fac = {};
        fac.UploadedFile = function (file, docId) {
            var deferred = $q.defer();
            var formData = new FormData();
            formData.append("files", file);
            formData.append("idDangKy", docId);
            $http.post("/DangKyTiepDan/UploadFile", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                }).success(function (data) {
                    deferred.resolve(data);
                })
                .error(deferred.reject);
            return deferred.promise;
        }
        return fac;
    });


