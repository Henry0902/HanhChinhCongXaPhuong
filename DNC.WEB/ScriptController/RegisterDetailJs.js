myApp.controller("RegisterDetailJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore) {
        $scope.formData = {};
        $scope.formAdd = {};
        $scope.fileList = [];
        $scope.formClose = {};
        $scope.lstDepartments = [];
        $scope.onBack = function () {
            window.location.href = "/dang-ky-tiep-dan/danh-sach";
        }

        $scope.isExpired = function (time) {
            if (!time) return false; // Nếu không có dữ liệu, không tính là quá hạn

            // Chuyển đổi từ "hh:mm dd/MM/yyyy" sang đối tượng Date
            var parts = time.split(' '); // Tách phần giờ phút và ngày tháng năm
            var timeParts = parts[0].split(':'); // Tách giờ phút
            var dateParts = parts[1].split('/'); // Tách ngày tháng năm

            var ngayDK = new Date(
                dateParts[2],        // Năm
                dateParts[1] - 1,    // Tháng (JavaScript bắt đầu từ 0)
                dateParts[0],        // Ngày
                timeParts[0],        // Giờ
                timeParts[1]         // Phút
            );

            // Cộng thêm 1 giờ vào thời gian đăng ký
            ngayDK.setHours(ngayDK.getHours() + 1);

            // Kiểm tra nếu hiện tại lớn hơn ngayDK + 1 giờ => quá hạn
            return new Date() > ngayDK;
        };

        var convertMicrosoftDate = function (msDateString) {
            if (!msDateString) return null;
            // Trích xuất số mili-giây từ chuỗi "/Date(1742369400000)/"
            let timestamp = parseInt(msDateString.match(/\d+/)[0], 10);

            // Tạo đối tượng Date từ timestamp
            let date = new Date(timestamp);

            // Lấy giờ, phút, ngày, tháng, năm
            let hours = String(date.getHours()).padStart(2, '0');
            let minutes = String(date.getMinutes()).padStart(2, '0');
            let day = String(date.getDate()).padStart(2, '0');
            let month = String(date.getMonth() + 1).padStart(2, '0'); // Tháng trong JS bắt đầu từ 0
            let year = date.getFullYear();

            // Trả về chuỗi định dạng HH:mm dd/MM/yyyy
            return `${hours}:${minutes} ${day}/${month}/${year}`;
        }

        $scope.showModalResend = function () {
            $("#divLoading").show();
            $scope.formAdd = {
                IdDonViTiepDan: $scope.formData.IdDonViTiepDan,
                NgayDangKy: $scope.formData.NgayDangKy,
                ChuDe: $scope.formData.ChuDe,
            };
            $scope.fileList = [];
            $scope.registrationForm.$setPristine();
            $scope.registrationForm.$setUntouched();
            $("#addModal").modal("show");
            setTimeout(() => {
                $("#divLoading").hide();
            }, 1000);

        }
        $scope.showModalAdd = function () {
            $scope.fileList = [];
            $scope.registrationForm.$setPristine();
            $scope.registrationForm.$setUntouched();
            $("#addModal").modal("show");
        }

        //Cập nhật
        $scope.showModalEdit = function () {
            $("#divLoading").show();
            $scope.fileList = [];
            $scope.ListFileDelete = [];
            $scope.registrationForm.$setPristine();
            $scope.registrationForm.$setUntouched();
            $scope.formAdd = {
                Id: $scope.formData.Id,
                IdDonViTiepDan: $scope.formData.IdDonViTiepDan,
                NgayDangKy: $scope.formData.NgayDangKy,
                ChuDe: $scope.formData.ChuDe,
                TrangThai: $scope.formData.TrangThai,
            };

            $("#editModal").modal("show");
            setTimeout(() => {
                $("#divLoading").hide();
            }, 1000);
        }

        $scope.saveFormUpdate = function (status) {
            $scope.validateForm();
            if ($scope.registrationForm.$valid) {
                $scope.formAdd.TrangThai = status;
                $scope.formAdd.FileDelete = $scope.ListFileDelete;
                $("#editModal").modal("hide")
                $scope.updateRegister($scope.formAdd);
            }
        };

        $scope.closeModalAdd = function () {
            $("#editModal").modal("hide");
            $("#addModal").modal("hide");
        }

        $scope.removeFileUpdate = function (index) {
            $scope.formData.ListFile.splice(index, 1);
            $scope.ListFileDelete.push($scope.formData.ListFile[index].Id);
        }

        $scope.removeFile = function (index) {
            $scope.fileList.splice(index, 1);
        }

        //Cập nhật

        // Gửi đăng ký
        $scope.showModalSendRegister = function () {
            $("#sendRegisterModal").modal("show");
        }

        $scope.sendRegister = function () {
            $("#sendRegisterModal").modal("hide");
            $scope.formData.TrangThai = 1;
            $scope.updateRegister($scope.formData);
        }
        //Gửi đăng ký

        //Xoá
        $scope.openDeleteModal = function (item) {
            $("#deleteModal").modal("show");
        };

        $scope.remove = function (id) {
            $("#deleteModal").modal("hide");
            $("#divLoading").show();
            crudService.remove("/DangKyTiepDan/Delete", id)
                .success(function (data) {
                    $.notify("Xoá đăng ký tiếp dân thành công", "success");
                    setTimeout(() => {
                        $("#divLoading").hide();
                        window.location.href = "/dang-ky-tiep-dan/danh-sach";
                    }, 1000);
                })
                .error(function (error) {
                    $.notify("Xóa thất bại !", "error");
                    $("#divLoading").hide();
                });
        }
        //Xoá

        //Huỷ đăng ký
        $scope.showModalClose = function () {
            $scope.formClose = {};
            $scope.closeForm.$setPristine();
            $scope.closeForm.$setUntouched();
            $("#closeModal").modal("show");
        }

        $scope.CloseRegister = function () {
            $scope.closeForm.Content.$setTouched();
            if ($scope.closeForm.$valid) {
                $("#closeModal").modal("hide");
                $scope.formData.TrangThai = 5;
                $scope.formData.NgayDangKy = null;
                $scope.formData.LyDoHuy = $scope.formClose.Content;
                $scope.updateRegister($scope.formData);
            }
        }
        //Huỷ đăng ký

        $scope.validateForm = function () {
            $scope.registrationForm.IdDonViTiepDan.$setTouched();
            $scope.registrationForm.NgayDangKy.$setTouched();
            $scope.registrationForm.ChuDe.$setTouched();
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
                    $scope.fileList = [];
                    $("#divLoading").hide();
                    init();
                })
                .error(function () {
                    $.notify("Cập nhật thất bại!", "error");
                    $("#divLoading").hide();
                });
        }

        $scope.viewFile = function (file) {
            $scope.content = "";
            var port = $location.port();
            if (port != '') {
                $scope.content = $location.protocol() + "://" + $location.host() + ':' + $location.port() + file.Url;
            }
            else {
                $scope.content = $location.protocol() + "://" + $location.host() + file.Url;
            }
            window.open($scope.content, "_blank");
        }

        getbyID = function () {
            $("#divLoading").show();
            crudService.get("/DangKyTiepDan/GetById?id=", $scope.id)
                .success(function (data) {
                    $scope.formData = data;
                    $scope.formData.NgayDangKy = parseDateV($scope.formData.NgayDangKy);
                    $scope.formData.NgayTao = parseDateV($scope.formData.NgayTao);
                    $scope.formData.NgayXuLy = parseDateV($scope.formData.NgayXuLy);
                    $scope.formData.NgayTiepNhan = parseDateV($scope.formData.NgayTiepNhan);
                    $scope.formData.ThoiGianHenText = convertMicrosoftDate($scope.formData.ThoiGianHen);
                    $scope.formData.ThoiGianHen = parseDateV($scope.formData.ThoiGianHen);
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
                $scope.formAdd.IdCongDan = $cookieStore.get("UserID");
                $scope.formAdd.TrangThai = status;
                var code = $scope.lstDepartments.find(x => x.Id == $scope.formAdd.IdDonViTiepDan)?.Code;
                if (code) {
                    $scope.formAdd.MaDangKy = generateCode(code, new Date(), 1);
                    create($scope.formAdd);
                } else {
                    $.notify("Có lỗi xảy ra", "error");
                }
            }
        };

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
                                            $("#divLoading").hide();
                                            window.location.href = "/dang-ky-tiep-dan/danh-sach";
                                        }, 1000);
                                    }
                                });
                            }
                        } else {
                            setTimeout(() => {
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

        function generateCode(departmentCode, date, sequence) {
            const ddmmyy = date.toISOString().slice(0, 10).split('-').reverse().join('').slice(0, 6);
            const sequenceFormatted = String(sequence).padStart(4, '0');
            return `${departmentCode}-${ddmmyy}-${sequenceFormatted}`;
        }

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

        //lấy danh sách phòng ban
        var getAllDepartment = function () {
            crudService.getAll("/Departments/GetDepartmentsByLevel")
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


