myApp.controller("CitizenListJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore) {
        // Tham số  
        $scope.lstData = [];
        $scope.lstPage = [];

        // Tham số phân trang
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        $scope.keyword = null;
        $scope.idCard = null;
        $scope.createdDate = null;
        $scope.gender = -1;
        $scope.status = -1;
        $scope.address = null;
        $scope.isLocked = -1;

        $scope.ListStatus = [
            { Code: 0, Name: "Chờ đăng ký" },
            { Code: 1, Name: "Cần cập nhật" },
            { Code: 2, Name: "Đã kích hoạt" },
            { Code: 3, Name: "Vô hiệu hoá" },
        ];
        $scope.selectedItem = null;
        $scope.formData = {};
        $scope.formClose = {};

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

        $scope.onShowLock = function (item) {
            $("#lockModal").modal("show");
            $scope.selectedItem = item;
        }

        $scope.onLock = function () {
            $("#lockModal").modal("hide");
            let status = null;
            if ($scope.selectedItem.Status == 2) {
                status = 3;
            } else if ($scope.selectedItem.Status == 3) {
                status = 2;
            }

            $scope.UpdateStatusCititzen(status, "");
        }

        $scope.onShowApprove = function (item) {
            $("#approveModal").modal("show");
            $scope.selectedItem = item;
        }

        $scope.sendEmail = function (data) {
            let body = {
                To: data.To,
                Subject: 'Phê duyệt tài khoản công dân',
                Body: data.Body
            };

            $http.post('/Email/SendEmail', body).then(function (response) {
                if (response.data == true) {
                } else {
                    $.notify("Có lỗi xảy ra", "error");
                }
            }).catch(function (error) {
                console.log("Error: " + error.data);
            });
        }

        $scope.onApprove = function () {
            $("#approveModal").modal("hide");
            $scope.UpdateStatusCititzen(2, "");
        }

        $scope.onShowClose = function (item) {
            $scope.closeForm.$setPristine();
            $scope.closeForm.$setUntouched();
            $("#closeModal").modal("show");
            $scope.selectedItem = item;
        }

        $scope.OnClose = function () {
            $scope.closeForm.Content.$setTouched();
            if ($scope.closeForm.$valid) {
                $("#closeModal").modal("hide");
                $scope.UpdateStatusCititzen(1, $scope.formClose.Content);
            }
        }

        $scope.UpdateStatusCititzen = function (status, note) {
            $("#divLoading").show();
            $http({
                url: "/Users/UpdateStatusCititzen",
                method: "GET",
                params: {
                    id: $scope.selectedItem.Id,
                    status,
                    note
                }
            }).success(function (result) {
                $.notify("Cập nhật thành công!", "success");
                if (status == 2) {
                    let body = `<p>Chúng tôi xin thông báo rằng tài khoản <strong>${$scope.selectedItem.Mobile}</strong> của công dân đã được phê duyệt thành công.</p><p>Công dân hiện đã có thể sử dụng tính năng đăng ký lịch tiếp công dân.</p><p>Trân trọng.</p>`
                    $scope.sendEmail({ To: $scope.selectedItem.Email, Body: body });
                }
                init();
            })
            .error(function (error) {
                $.notify("Cập nhật thất bại!", "error");
            });
        }

        $scope.onEnter = function (event) {
            if (event.keyCode === 13) { // 13 là mã phím Enter
                $scope.reload();
            }
        };

        $scope.redirectToPage = function () {
            window.location.href = "/dang-ky-tiep-dan/danh-sach";
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

        //Load danh sách data
        var getData = function () {
            return $http({
                url: "/TaiKhoanCongDan/GetListCitizen",
                method: "GET",
                params: {
                    keyword: $scope.keyword,
                    createdDate: $scope.createdDate,
                    idCard: $scope.idCard,
                    gender: $scope.gender,
                    status: $scope.status,
                    address: $scope.address,
                    pageNumber: $scope.pageIndex,
                    pageSize: $scope.pageSize,
                }
            });
        }

        // Init data
        var init = function () {
            $("#divLoading").show();
            getData()
                .success(function (data) {
                    angular.forEach(data, function (item) {
                        item.CreatedDate = parseDateV(item.CreatedDate);
                        item.IssuanceDate = parseDateV(item.IssuanceDate);
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
                    setTimeout(() => {
                        $("#divLoading").hide();
                    }, 1000)
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
                        //angular.forEach(data, function (item) {
                        //    item.NgayNhap = parseDate(item.NgayNhap);
                        //});
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


