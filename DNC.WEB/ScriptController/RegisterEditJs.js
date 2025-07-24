
myApp.controller("RegisterEditJs",
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

        function parseDate(dateStr) {
            var parts = dateStr.split("/");
            return new Date(parts[2], parts[1] - 1, parts[0]);

        }

        $scope.updateRegister = function (data) {
            $("#divLoading").show();
            crudService.update("/DangKyTiepDan/Update", data)
                .success(function () {
                    //Upload file sau khi tạo hồ sơ xong                    
                    if ($scope.fileList.length != 0) {
                        for (var i = 0; i < $scope.fileList.length; i++) {
                            fileUploadService.UploadedFile($scope.fileList[i], $scope.formData.Id).then(function (res) {
                            });
                        }
                    }

                    $.notify("Cập nhật thành công!", "success");
                    $("#divLoading").hide();
                    setTimeout(() => {
                        $scope.fileList = [];
                        window.location.href = "/dang-ky-tiep-dan/danh-sach";
                    }, 1000);
                })
                .error(function () {
                    $.notify("Cập nhật thất bại!", "error");
                    $("#divLoading").hide();
                });
        }

        getbyID = function () {
            $("#divLoading").show();
            crudService.get("/DangKyTiepDan/GetById?id=", $scope.id)
                .success(function (data) {
                    $scope.formData = data;
                    $scope.formData.NgayDangKy = parseDateV($scope.formData.NgayDangKy);
                    $scope.formData.NgayTao = parseDateV($scope.formData.NgayTao);
                    $("#divLoading").hide();
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                    $("#divLoading").hide();
                });

        };

        $scope.removeFile = function (index) {
            $scope.formData.ListFile.splice(index, 1);
            $scope.ListFileDelete.push($scope.formData.ListFile[index].Id);
        }

        $scope.removeFile2 = function (index) {
            $scope.fileList.splice(index, 1);
        }

        $scope.ListFileDelete = [];
        $scope.saveForm = function (status) {
            $scope.validateForm();
            if ($scope.registrationForm.$valid) {
                $scope.formData.TrangThai = status;
                $scope.formData.FileDelete = $scope.ListFileDelete;
                $scope.updateRegister($scope.formData);
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
                    getbyID();
                });
        }

        // Init data
        var init = function () {
            $("#divLoading").show();
            const pathSegments = window.location.href.split("/")
            const id = pathSegments[pathSegments.length - 1];
            $scope.id = !isNaN(id) ? id : null;
            if ($scope.id) {
                getAllDepartment();
               
            } else {
                $.notify("Có lỗi xảy ra", "error");
                $("#divLoading").hide();
            }
        }

        init();
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


