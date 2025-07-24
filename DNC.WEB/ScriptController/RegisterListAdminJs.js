myApp.controller("RegisterListAdminJs",
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
            { Code: 1, Name: "Chờ xử lý" },
            { Code: 2, Name: "Đang xử lý" },
            { Code: 3, Name: "Đã phản hồi" },
            { Code: 4, Name: "Không tiếp nhận" },
            { Code: 5, Name: "Công dân huỷ" },
            { Code: 6, Name: "Đã tiếp dân" },
        ];
        $scope.selectedItem = null;
        $scope.formData = {};
        $scope.userInfo = {};

        $scope.formAccept = {};
        $scope.formClose = {};
        $scope.formRefuse = {};
        $scope.ListFileDelete = [];
        $scope.listNoiTiep = [];

        $scope.isAdmin = null;

        $scope.timeSlots = ["08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00"];
        $scope.selectedTime = null;
        $scope.lstDepartments = [];

        //lấy danh sách phòng ban
        var getAllDepartment = function () {
            crudService.getAll("/Departments/GetDepartmentsByLevel")
                .success(function (data) {
                    $scope.lstDepartments = data;
                });
        }
        getAllDepartment();

        $scope.sendEmail = function (data) {
            let body = {
                To: data.To,
                Subject: 'Phê duyệt đăng ký tiếp công dân',
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

        $scope.getListNoiTiep = function () {
            $http({
                url: "/DmDiaDiemTiepDan/GetIndex",
                method: "GET",
                params: {
                    thongtintimkiem: "",
                    IdDonVi: $scope.selectedItem.IdDonViTiepDan,
                    DiaChi: "",
                    TrangThai: -1,
                    order: "desc",
                    pageSize: 10000,
                    pageIndex: 1

                }
            }).success(function (data) {
                $scope.listNoiTiep = data;
            })
                .error(function (error) {
                });
        }

        var mergeDateTime = function (dateStr, timeStr) {
            // Tách ngày, tháng, năm
            let [day, month, year] = dateStr.split('/').map(Number);
            // Tách giờ, phút
            let [hour, minute] = timeStr.split(':').map(Number);

            // Tạo đối tượng Date
            let dateTime = new Date(year, month - 1, day, hour, minute);
            return dateTime;
        }

        $scope.selectTime = function (time) {
            $scope.selectedTime = time;
            $scope.timeSlotError = false;
        };

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

        $scope.updateStatus = function (id, status, content, appointment, idRecepion) {
            $("#resolveModal").modal("hide");
            $("#refuseModal").modal("hide");
            $("#acceptModal").modal("hide");
            $("#receiveModal").modal("hide");
            $("#closeModal").modal("hide");

            $("#divLoading").show();
            crudService.update("/DangKyTiepDan/UpdateStatus", { id, status, content, appointment, idRecepion })
                .success(function () {
                    $.notify("Cập nhật thành công!", "success");
                    if (status == 3) {
                        let noiTiep = $scope.listNoiTiep.find(x => x.id == $scope.formAccept.IdNoiTiep);
                        let body = `<p>Kính gửi ông/ bà: <strong>${$scope.selectedItem.UserName}</strong>,</p><p>Chúng tôi xin thông báo rằng đơn đăng ký tiếp công dân của quý vị đã được phê duyệt thành công.</p><p>Dưới đây là thông tin chi tiết:</p>` +
                            `<p>- Họ và tên: <strong>${$scope.selectedItem.UserName}</strong></p>` +
                            `<p>- Ngày tiếp công dân: <strong>${$scope.selectedItem.NgayDangKy}</strong></p>` +
                            `<p>- Thời gian: <strong>${$scope.selectedTime}</strong></p>` +
                            `<p>- Nơi tiếp: <strong>${noiTiep?.Ten}</strong></p>` +
                            `<p>- Địa chỉ: <strong>${noiTiep?.DiaChi}</strong></p>` +
                            `<p>Đề nghị Quý vị đến đúng thời gian và mang theo đầy đủ giấy tờ cần thiết. Nếu có bất kỳ thay đổi nào, vui lòng liên hệ trước với chúng tôi qua [số điện thoại] hoặc [email hỗ trợ].`
                        $scope.sendEmail({ To: $scope.selectedItem.Email, Body: body });
                    }
                    setTimeout(() => {
                        if (status == 6) {
                            window.location.href = "/TiepCongDan/TiepCongDan";
                        }
                        init();
                    }, 1000)
                })
                .error(function () {
                    $.notify("Cập nhật thất bại!", "error");
                    $("#divLoading").hide();
                });
        }

        $scope.onShowResolve = function (item) {
            $("#resolveModal").modal("show");
            $scope.selectedItem = item;
        }

        $scope.onShowRefuse = function (item) {
            $scope.formRefuse = [];
            $scope.refuseForm.$setPristine();
            $scope.refuseForm.$setUntouched();
            $("#refuseModal").modal("show");
            $scope.selectedItem = item;
        }

        $scope.onShowAccept = function (item) {
            $scope.formAccept = {};
            $scope.selectedTime = null;
            $scope.timeSlotError = false;
            $scope.acceptForm.$setPristine();
            $scope.acceptForm.$setUntouched();
            $("#acceptModal").modal("show");
            $scope.selectedItem = item;
            $scope.getListNoiTiep();
        }

        $scope.onShowReceive = function (item) {
            $("#receiveModal").modal("show");
            $scope.selectedItem = item;
        }

        $scope.onShowClose = function (item) {
            $scope.formClose = [];
            $scope.closeForm.$setPristine();
            $scope.closeForm.$setUntouched();
            $("#closeModal").modal("show");
            $scope.selectedItem = item;
        }

        $scope.onHandleRefuse = function () {
            $scope.refuseForm.Content.$setTouched();
            if ($scope.refuseForm.$valid) {
                $("#refuseModal").modal("hide");
                $scope.updateStatus($scope.selectedItem.Id, 4, $scope.formRefuse.Content);
            }
        }

        $scope.onHandleAccept = function () {
            $scope.acceptForm.NoiTiep.$setTouched();
            if (!$scope.selectedTime) {
                $scope.timeSlotError = true;
            } else {
                $scope.timeSlotError = false;
            }
            if ($scope.acceptForm.$valid && $scope.selectedTime) {
                $("#acceptModal").modal("hide");
                var appointment = mergeDateTime($scope.selectedItem.NgayDangKy, $scope.selectedTime);
                $scope.updateStatus($scope.selectedItem.Id, 3, $scope.formAccept.Content, appointment, $scope.formAccept.IdNoiTiep);
            }
        }

        $scope.onHandleClose = function () {
            $scope.closeForm.Content.$setTouched();
            if ($scope.closeForm.$valid) {
                $("#closeModal").modal("hide");
                $scope.updateStatus($scope.selectedItem.Id, 5, $scope.formClose.Content);
            }
        }

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

        $scope.redirectToPage = function () {
            window.location.href = "/dang-ky-tiep-dan/danh-sach";
        };

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
            let params = {
                pageIndex: $scope.pageIndex,
                pageSize: $scope.pageSize,
                status: $scope.Status,
                content: $scope.Content,
                registerDate: $scope.RegisterDate,
                department: $scope.DepartmentId,
            };

            $scope.isAdmin = $cookieStore.get("SuperUser");
            if ($scope.isAdmin != 1) {
                params.department = $cookieStore.get("DeparmentId");
            }

            return $http({
                url: "/DangKyTiepDan/GetListRegister",
                method: "GET",
                params: params
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


