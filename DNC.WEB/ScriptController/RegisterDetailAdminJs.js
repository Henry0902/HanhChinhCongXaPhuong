
myApp.controller("RegisterDetailAdminJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore) {
        $scope.formData = {};
        $scope.formAdd = {};
        $scope.fileList = [];

        $scope.formAccept = {};
        $scope.formClose = {
            Content: null
        };
        $scope.formRefuse = {
            Content: null
        };
        $scope.listNoiTiep = [];

        $scope.timeSlots = ["08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30", "16:00"];
        $scope.selectedTime = null;

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

        $scope.getListNoiTiep = function () {
            $http({
                url: "/DmDiaDiemTiepDan/GetIndex",
                method: "GET",
                params: {
                    thongtintimkiem: "",
                    IdDonVi: $scope.formData.IdDonViTiepDan,
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
                        let body = `<p>Kính gửi ông/ bà: <strong>${$scope.formData.UserName}</strong>,</p><p>Chúng tôi xin thông báo rằng đơn đăng ký tiếp công dân của quý vị đã được phê duyệt thành công.</p><p>Dưới đây là thông tin chi tiết:</p>` +
                            `<p>- Họ và tên: <strong>${$scope.formData.UserName}</strong></p>` +
                            `<p>- Ngày tiếp công dân: <strong>${$scope.formData.NgayDangKy}</strong></p>` +
                            `<p>- Thời gian: <strong>${$scope.selectedTime}</strong></p>` +
                            `<p>- Nơi tiếp: <strong>${noiTiep?.Ten}</strong></p>` +
                            `<p>- Địa chỉ: <strong>${noiTiep?.DiaChi}</strong></p>` +
                            `<p>Đề nghị Quý vị đến đúng thời gian và mang theo đầy đủ giấy tờ cần thiết. Nếu có bất kỳ thay đổi nào, vui lòng liên hệ trước với chúng tôi qua [số điện thoại] hoặc [email hỗ trợ].`
                        $scope.sendEmail({ To: $scope.formData.Email, Body: body });
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
        $scope.onShowResolve = function () {
            $("#resolveModal").modal("show");
        }

        $scope.onShowRefuse = function () {
            $scope.formRefuse = [];
            $scope.refuseForm.$setPristine();
            $scope.refuseForm.$setUntouched();
            $("#refuseModal").modal("show");
        }

        $scope.onHandleRefuse = function () {
            $scope.refuseForm.Content.$setTouched();
            if ($scope.refuseForm.$valid) {
                $("#refuseModal").modal("hide");
                $scope.updateStatus($scope.formData.Id, 4, $scope.formRefuse.Content);
            }
        }

        $scope.onShowAccept = function () {
            $scope.formAccept = {};
            $scope.selectedTime = null;
            $scope.timeSlotError = false;
            $scope.acceptForm.$setPristine();
            $scope.acceptForm.$setUntouched();
            $scope.getListNoiTiep();
            $("#acceptModal").modal("show");
        }

        $scope.onHandleAccept = function () {
            $scope.acceptForm.NoiTiep.$setTouched();
            if (!$scope.selectedTime) {
                $scope.timeSlotError = true;
            } else {
                $scope.timeSlotError = false;
            }

            if ($scope.acceptForm.$valid) {
                $("#acceptModal").modal("hide");
                var appointment = mergeDateTime($scope.formData.NgayDangKy, $scope.selectedTime);
                $scope.updateStatus($scope.formData.Id, 3, $scope.formAccept.Content, appointment, $scope.formAccept.IdNoiTiep);
            }
        }
        $scope.onShowReceive = function (item) {
            $("#receiveModal").modal("show");
        }

        $scope.onShowClose = function () {
            $scope.formClose = [];
            $scope.closeForm.$setPristine();
            $scope.closeForm.$setUntouched();
            $("#closeModal").modal("show");
        }

        $scope.onHandleClose = function () {
            $scope.closeForm.Content.$setTouched();
            if ($scope.closeForm.$valid) {
                $("#closeModal").modal("hide");
                $scope.updateStatus($scope.formData.Id, 5, $scope.formClose.Content);
            }
        }

        $scope.onBack = function () {
            window.location.href = "/cong-dan-dang-ky/danh-sach";
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

        function parseDate(dateStr) {
            var parts = dateStr.split("/");
            return new Date(parts[2], parts[1] - 1, parts[0]);

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

        $scope.redirectToPage = function () {
            window.location.href = "/cong-dan-dang-ky/danh-sach"; 
        };

        // Init data
        var init = function () {
            $("#divLoading").show();
            const pathSegments = window.location.href.split("/")
            const id = pathSegments[pathSegments.length - 1];
            $scope.id = !isNaN(id) ? id : null;
            if ($scope.id) {
                getbyID();
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


