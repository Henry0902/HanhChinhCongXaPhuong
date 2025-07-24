
myApp.controller("RegisterListJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore) {
        // Tham số  
        $scope.lstData = [];
        $scope.lstPage = [];

        // Tham số phân trang
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        $scope.Content = '';
        $scope.Status = -1;
        $scope.DepartmentId = -1;
        $scope.RegisterDate = null;
        $scope.ListStatus = [
            { Code: 0, Name: "Đang soạn thảo" },
            { Code: 1, Name: "Chờ xử lý" },
            { Code: 2, Name: "Đang xử lý" },
            { Code: 3, Name: "Đã phản hồi" },
            { Code: 4, Name: "Không tiếp nhận" },
            { Code: 5, Name: "Công dân huỷ" },
            { Code: 6, Name: "Đã tiếp dân" },
        ];
        $scope.selectedItem = null;
        $scope.formData = {};
        $scope.fileList = [];

        $scope.userInfo = {};

        $scope.formClose = {};
        $scope.lstDepartments = [];
        $scope.ListFileDelete = [];

        $scope.onEnter = function (event) {
            if (event.keyCode === 13) { // 13 là mã phím Enter
                $scope.reload();
            }
        };

        $scope.onChange = function (event) {
            if ($scope.Content == null || $scope.Content == "") {
                $scope.reload();
            }
        };

        var getUserbyID = function () {
            crudService.get("/Users/GetById?id=", $cookieStore.get("UserID"))
                .success(function (data) {
                    $scope.userInfo = data;
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
        };
        getUserbyID();

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
            $("#addModal").modal("hide");
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
                                            init();
                                        }, 1000);
                                    }
                                });
                            }
                        } else {
                            setTimeout(() => {
                                $("#divLoading").hide();
                                init();
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

        $scope.CloseRegister = function () {
            $scope.closeForm.Content.$setTouched();
            if ($scope.closeForm.$valid) {
                $("#closeModal").modal("hide");
                $scope.selectedItem.TrangThai = 5;
                $scope.selectedItem.NgayDangKy = null;
                $scope.selectedItem.LyDoHuy = $scope.formClose.Content;
                $scope.updateRegister($scope.selectedItem);
            }
        }

        $scope.showModalSendRegister = function (item) {
            $scope.selectedItem = item;
            $("#sendRegisterModal").modal("show");
        }

        $scope.showModalClose = function (item) {
            $scope.formClose = {};
            $scope.closeForm.$setPristine();
            $scope.closeForm.$setUntouched();
            $scope.selectedItem = item;
            $("#closeModal").modal("show");
        }

        $scope.sendRegister = function (data) {
            $("#sendRegisterModal").modal("hide");
            data.TrangThai = 1;
            $scope.updateRegister(data);
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
                    setTimeout(() => {
                        $scope.fileList = [];
                        init();
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

        $scope.removeFileUpdate = function (index) {
            $scope.formData.ListFile.splice(index, 1);
            $scope.ListFileDelete.push($scope.formData.ListFile[index].Id);
        }

        $scope.saveFormUpdate = function (status) {
            $scope.validateForm();
            if ($scope.registrationForm.$valid) {
                $scope.formData.TrangThai = status;
                $scope.formData.FileDelete = $scope.ListFileDelete;
                $("#editModal").modal("hide")
                $scope.updateRegister($scope.formData);
            }
        };

        $scope.closeModalAdd = function () {
            $("#editModal").modal("hide");
            $("#addModal").modal("hide");
        }

        $scope.openDeleteModal = function (item) {
            $scope.selectedItem = item;
            $("#deleteModal").modal("show");
        };

        $scope.remove = function (id) {
            $("#deleteModal").modal("hide");
            $("#divLoading").show();
            crudService.remove("/DangKyTiepDan/Delete", id)
                .success(function (data) {
                    $.notify("Xoá đăng ký tiếp dân thành công", "success");
                    setTimeout(() => {
                        init();
                        $("#divLoading").hide();
                    }, 1000);
                })
                .error(function (error) {
                    $.notify("Xóa thất bại !", "error");
                    $("#divLoading").hide();
                });
        }

        // Convert datetime
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        $scope.getClassCss = function (status) {
            if (status == 0) return "status-common status-dang-soan-thao";
            if (status == 1) return "status-common status-cho-xu-ly";
            if (status == 2) return "status-common status-dang-xu-ly";
            if (status == 3) return "status-common status-da-phan-hoi";
            if (status == 4) return "status-common status-khong-tiep-nhan";
            if (status == 5) return "status-common status-cong-dan-huy";
            if (status == 6) return "status-common status-da-tiep-dan";
        }

        $scope.showModalResend = function (item) {
            $("#divLoading").show();
            $scope.formData = {
                IdDonViTiepDan: item.IdDonViTiepDan,
                NgayDangKy: item.NgayDangKy,
                ChuDe: item.ChuDe,
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
            $scope.formData = { IdDonViTiepDan: "", NgayDangKy: "", ChuDe: ""};
            $scope.fileList = [];
            $scope.registrationForm.$setPristine();
            $scope.registrationForm.$setUntouched();
            $("#addModal").modal("show");
        }

        $scope.showModalEdit = function (id) {
            $("#divLoading").show();
            $scope.formData = {};
            $scope.fileList = [];
            $scope.ListFileDelete = [];
            $scope.registrationForm.$setPristine();
            $scope.registrationForm.$setUntouched();
            crudService.get("/DangKyTiepDan/GetById?id=", id)
                .success(function (data) {
                    $scope.formData = data;
                    $scope.formData.NgayDangKy = parseDateV($scope.formData.NgayDangKy);
                    $scope.formData.NgayTao = parseDateV($scope.formData.NgayTao);
                    setTimeout(() => {
                        $("#divLoading").hide();
                    }, 1000);
                }).error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                    $("#divLoading").hide();
                });

            $("#editModal").modal("show");
        }

        //lấy danh sách phòng ban
        var getAllDepartment = function () {
            crudService.getAll("/Departments/GetDepartmentsByLevel")
                .success(function (data) {
                    $scope.lstDepartments = data;
                });
        }
        getAllDepartment();

        //Load danh sách data
        var getData = function () {
            return $http({
                url: "/DangKyTiepDan/GetListRegister",
                method: "GET",
                params: {
                    pageIndex: $scope.pageIndex,
                    pageSize: $scope.pageSize,
                    status: $scope.Status,
                    content: $scope.Content,
                    department: $scope.DepartmentId,
                    registerDate: $scope.RegisterDate,
                    idCongDan: $cookieStore.get("UserID")
                }
            });
        }

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

        // Init data
        var init = function () {
            $("#divLoading").show();
            getData()
                .success(function (data) {
                    angular.forEach(data, function (item) {
                        item.NgayDangKy = parseDateV(item.NgayDangKy);
                        item.ThoiGianHenTemp = convertMicrosoftDate(item.ThoiGianHen);
                        item.ThoiGianHen = parseDateV(item.ThoiGianHen);
                    });
                    $scope.lstPage = [];
                    $scope.lstData = data;
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
                    $("#divLoading").hide();
                });
        }

        init();

        // Clear Data
        var clearData = function () {
            $scope.lstPage = [];
        }

        // Lấy về trang click vào trong phân trang
        $scope.selectPage = function (index) {
            if (index == 0) {
                index = 1;
            }
            $scope.pageIndex = index;
            $scope.lstPage.splice(0);
            var pageCount = $scope.pageCount;
            if (pageCount <= 9) {
                for (var i = 1; i <= pageCount; i++) {
                    $scope.lstPage.push(i);
                }
            } else {
                if (index >= 5) {
                    if (index <= pageCount - 4) {
                        for (i = index - 4; i <= index + 4 && i <= pageCount; i++) {
                            $scope.lstPage.push(i);
                        }
                    } else {
                        for (i = pageCount - 8; i <= pageCount; i++) {
                            $scope.lstPage.push(i);
                        }
                    }
                } else {
                    for (i = 1; i <= 9; i++) {
                        $scope.lstPage.push(i);
                    }
                }
            }

            if (!(index > pageCount)) {
                $scope.currentPage.stt = index;
            }

            if (!(index > pageCount)) {
                $scope.pageIndex = index;
                getData()
                    .success(function (data) {
                        angular.forEach(data, function (item) {
                            item.NgayDangKy = parseDateV(item.NgayDangKy);
                            item.ThoiGianHenTemp = convertMicrosoftDate(item.ThoiGianHen);
                            item.ThoiGianHen = parseDateV(item.ThoiGianHen);
                        });
                        $scope.lstData = data;
                        $scope.totalRecords = data[0].TotalRecords;
                        $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
                    })
                    .error(function (error) {
                        $.notify("Không tìm thấy dữ liệu!", "error");
                    });
            }
        }

        // Reload trang
        $scope.reload = function () {
            $scope.pageIndex = 1;
            $scope.currentPage.stt = 1;
            $scope.lstPage.splice(0);
            $scope.lstPage = [];
            init();
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


