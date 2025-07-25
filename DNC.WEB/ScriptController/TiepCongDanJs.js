
myApp.controller("TiepCongDanJs",
    function ($location, $scope, $routeParams, $route, crudService, fileUploadService, $http, $cookieStore, $timeout) {
        // Tham số   
        $scope.namID = -1;
        
        $scope.TiepDan = {};
     //   $scope.TiepDan.Id = '1';
        $scope.TiepDan.KieuTiepDan = 1;
        $scope.TiepDan.IdPhienTCD = -1;
        $scope.TiepDan.IdDoiTuong = 1; // Mạc định là cá nhân
        $scope.TiepDan.GioiTinh = 0;
        $scope.TiepDan.IdTinhThanh = 16; // Mạc định là Bắc Ninh
        $scope.TiepDan.IdQuanHuyen = -1;
        $scope.TiepDan.IdPhuongXa = -1;
        $scope.TiepDan.IdQuocTich = 1; // Mạc định là Việt Nam
        $scope.TiepDan.IdDanToc = 1; // Mạc định là Kinh
        $scope.TiepDan.IdNguonDonThu = -1;
       
      

        $scope.IdDonVi = $cookieStore.get("DeparmentId");
        $scope.TiepDan.IdLoaiVu = $scope.IdDonVi;

        //$scope.TiepDan.IdLoaiVu = $scope.IdDonVi;
        
        $scope.TiepDan.IdLoaiVuKNTC = -1;
        $scope.TiepDan.IdLoaiVuKNTCChiTiet = -1;
        //$scope.TiepDan.LanGiaiQuyet = 0;
        $scope.TiepDan.LanTiepNhan = 0;
        $scope.TiepDan.Pre_Status = 0;
        $scope.TiepDan.IdLuotTiep = false;
        $scope.TiepDan.IdUyQuyen = false;

        $scope.DonThu = {};
        //$scope.CheckThongTinDonThu = false;
        $scope.CheckThongTinDonThu = true;
        $scope.CheckDonThuXuLy = false;
        $scope.CheckThongTinNguoiBiKNTC = false;
        $scope.CheckThongTinNguoiDaiDien = false;

        $scope.HoTenSearch = '';
        $scope.NoiDungSearch = '';
        $scope.CMTNDSearch = '';
        $scope.IdTinhThanhSearch = 16; // Mạc định là Bắc Ninh
        $scope.IdQuanHuyenSearch = -1;
        $scope.IdPhuongXaSearch = -1;
    
        

        $scope.today = new Date();       
        $scope.lstPage = [];
        // Tham số phân trang
        $scope.Order = 'desc';
        $scope.pageIndex = 1;
        $scope.pageSize = 10;
        $scope.totalRecords = 0;
        $scope.pageCount = 0;

        $scope.DmPhienTiepCongDan = {
            Id:-1
        }

        //-------------------------------------------------------------------------------
        var lblsohieuhs = document.getElementById("lblsohieuhs");
        var lbltenhs = document.getElementById("lbltenhs");
        var lblmahs = document.getElementById("lblmahs");
        var lblsohs = document.getElementById("lblsohs");
    
        //-------------------------------------------------------------------------------   
        var connection = null;
        //var ws = "ws://192.168.5.1:8000";
        var ws = "ws://113.160.214.213:3333";
        var startWebsocket = function () {
            /*connection = new WebSocket(ws);
            connection.addEventListener('close', function (event) {
                console.log('Đang mất kết nối thiết bị')
            });

            connection.addEventListener('open', function (event) {
                console.log('Kết nối thiết bị thành công')
            });
            connection.onmessage = function (message) {
                console.log(message);
            }
            connection.onclose = function () {
                connection = null;
                $timeout(function () {
                    startWebsocket();
                }, 3000);
            }*/
            const socket = io('https://192.168.5.1:8000',{transports:['websocket'], upgrade: false});
            socket.on('connect', function() {
                console.log('Socket connected');
            });
            socket.on('/event', function(dgs) { 
                var dataInfo = dgs.data;
                console.log("GETQR", dgs);
                if (dgs.id == 2) {
                    console.log(dgs);
                    console.log("lstDanToc", $scope.lstDanToc);
                    console.log("lstTinhThanh", $scope.lstTinhThanh);
                    console.log("lstQuanHuyen", $scope.lstQuanHuyen);
                    console.log("lstPhuongXa", $scope.lstPhuongXa);
                    genInfoUserFromCard(dgs.data);
                } else if(dgs.id == 6) {
                    if(dgs.data != '') {
                        genInfoUserFromQRCode(dgs.data)
                    }
                }
            });

            socket.on('/info', function(data) {
                console.log(data.serial_device);
            });
            document.getElementById('getQR').onclick = function () {
                console.log("GET QR");
                socket.emit('/get_qr');
            }
        }
        //startWebsocket();
        var genInfoUserFromCard = function (data) {
            var objDanToc = $scope.lstDanToc.find(x => x.TenDanToc == data.race);
            if(objDanToc) {
                $scope.TiepDan.IdDanToc = objDanToc.id;
            }
            var arrAddress = [];
            if(data.residencePlace != '') {
                arrAddress = data.residencePlace.split(",");
                $scope.TiepDan.DiaChi = data.residencePlace;
            } else {
                var arrStrQr = data.qr.split('|');
                console.log("arrStrQr", arrStrQr);
                if(arrStrQr.length > 4){
                    arrAddress = arrStrQr[5].split(",");
                    $scope.TiepDan.DiaChi = arrStrQr[5];
                }
            }
            
            if(arrAddress.length > 0){
                var length = arrAddress.length;
                console.log("arrAddress", arrAddress);
                var objTinh = $scope.lstTinhThanh.find(x => x.Name == arrAddress[length - 1].trim());
                if(objTinh) {
                    $scope.TiepDan.IdTinhThanh = objTinh.Id;
                    updateQuanHuyeFromCard(objTinh.Id, arrAddress);
                }
            }


            if (data.gender == 'Nam' || data.gender == 'nam') {
                $scope.TiepDan.GioiTinh = 0;
            } else {
                $scope.TiepDan.GioiTinh = 1;
            }
            $scope.TiepDan.HoTen = data.personName;
            $scope.TiepDan.SoGiayTo = data.idCode;
            $scope.TiepDan.NgayCap = data.dateOfBirth;
            
            $scope.$apply();
        }

        $scope.onSearch = function () {
            onGetDocuments();
        }

        $scope.listDocument = [];
        $scope.totalDocument = 0;

        $scope.iDisplayStart = 1;
        $scope.sSearch = "";
        $scope.pageCountDocument = 0;
        $scope.lstPageDocument = [];

        var getDocuments = function () {
            let params = {
                iDisplayStart: $scope.iDisplayStart,
                sSearch: $scope.sSearch,
            };

            return $http({
                url: "/TiepCongDan/GetDocuments",
                method: "GET",
                params: params
            })
        }

        $scope.selectPageDocument = function (index) {
            if (index == 0) {
                index = 1;
            }
            $scope.iDisplayStart = index;
            $scope.lstPageDocument.splice(0);
            var pageCount = $scope.pageCountDocument;
            if (pageCount <= 9) {
                for (var i = 1; i <= pageCount; i++) {
                    $scope.lstPageDocument.push(i);
                }
            } else {
                if (index >= 5) {
                    if (index <= pageCount - 4) {
                        for (i = index - 4; i <= index + 4 && i <= pageCount; i++) {
                            $scope.lstPageDocument.push(i);
                        }
                    } else {
                        for (i = pageCount - 8; i <= pageCount; i++) {
                            $scope.lstPageDocument.push(i);
                        }
                    }
                } else {
                    for (i = 1; i <= 9; i++) {
                        $scope.lstPageDocument.push(i);
                    }
                }
            }

            if (!(index > pageCount)) {
                $scope.currentPage.stt = index;
            }

            if (!(index > pageCount)) {
                $scope.iDisplayStart = index;
                $("#divLoading").show();
                getDocuments().success(function (data) {
                    $scope.listDocument = data.Value.Data;
                    $scope.totalDocument = data.Value.Total;
                    $scope.pageCount = Math.ceil($scope.totalDocument / $scope.iDisplayStart);

                    $("#divLoading").hide();
                })
                    .error(function (error) {
                        $("#divLoading").hide();
                    });
            }
        }

        var onGetDocuments = function () {
            $("#divLoading").show();
            $scope.iDisplayStart = 1;
            $scope.currentPage.stt = 1;

            getDocuments().success(function (data) {
                $scope.listDocument = data.Value.Data;
                $scope.totalDocument = data.Value.Total;
                $scope.pageCountDocument = Math.ceil($scope.totalDocument / 11);
                $scope.lstPageDocument = [];
                for (i = 1; i <= $scope.pageCountDocument && i <= 9; i++) {
                    $scope.lstPageDocument.push(i);
                }

                $("#divLoading").hide();
            })
                .error(function (error) {
                    $("#divLoading").hide();
                });
        }

        $scope.onShowDocument = function () {
            $("#documentModal").modal("show");
            $("#divLoading").show();
            $scope.listDocument = [];
            $scope.totalDocument = 0;
            $scope.pageCount = 1;
            $scope.sSearch = "";
            onGetDocuments();
        }

        var genInfoUserFromQRCode = function (data) {
            var arrStrQr = data.split('|');
            var arrAddress = [];
            console.log("arrStrQr", arrStrQr);
            if(arrStrQr.length > 4){
                arrAddress = arrStrQr[5].split(",");
                $scope.TiepDan.DiaChi = arrStrQr[5];
            }
            
            if(arrAddress.length > 0){
                var length = arrAddress.length;
                console.log("arrAddress", arrAddress);
                var objTinh = $scope.lstTinhThanh.find(x => x.Name == arrAddress[length - 1].trim());
                if(objTinh) {
                    $scope.TiepDan.IdTinhThanh = objTinh.Id;
                    updateQuanHuyeFromCard(objTinh.Id, arrAddress);
                }
            }


            if (arrStrQr[4] == 'Nam' || arrStrQr[4] == 'nam') {
                $scope.TiepDan.GioiTinh = 0;
            } else {
                $scope.TiepDan.GioiTinh = 1;
            }
            $scope.TiepDan.HoTen = arrStrQr[2];
            $scope.TiepDan.SoGiayTo = arrStrQr[0];
            $scope.TiepDan.NgayCap = arrStrQr[3].substr(0, 2) + "/" + arrStrQr[3].substr(2, 2) + "/" + arrStrQr[3].substr(4, 4);
            
            $scope.$apply();
        }
        updateQuanHuyeFromCard = function (tinhId, arrAddress) {
            $scope.lstQuanHuyen = [];
            crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", tinhId)
            .success(function (data) {
                $scope.lstQuanHuyen = data;
                $scope.lstPhuongXa = [];
                $scope.TiepDan.IdQuanHuyen = -1;
                $scope.TiepDan.IdPhuongXa = -1;
                if(arrAddress.length > 1) {
                    var length = arrAddress.length;
                    var tenGoc = arrAddress[length - 2].trim();
                    var tenHuyen = "Huyện " + tenGoc;
                    //var tenThanhPho = "Thành phố " + tenGoc;
                    var tenThanhPho = "Thành phố " + tenGoc.replace("TP. ", "").trim().replace("Tp. ", "").trim();
                    //var tenThiXa = "Thị xã " + tenGoc;
                    var tenThiXa = "Thị xã " + tenGoc.replace("TX. ", "").trim().replace("Tx. ", "").trim();
                    var tenQuan = "Quận " + tenGoc;
                    var objHuyen = $scope.lstQuanHuyen.find(x => x.TenQuanHuyen == tenGoc || x.TenQuanHuyen == tenHuyen || x.TenQuanHuyen == tenThanhPho 
                        || x.TenQuanHuyen == tenThiXa || x.TenQuanHuyen == tenQuan);
                    console.log("lstQuanHuyen", $scope.lstQuanHuyen);
                    console.log("objHuyen", objHuyen);
                    if(objHuyen){
                        $scope.TiepDan.IdQuanHuyen = objHuyen.id;
                        updatePhuongXaFromCard(objHuyen.id, arrAddress);
                    }
                }
            }).error(function (error) {
                $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
            });
        }
        updatePhuongXaFromCard = function (huyenId, arrAddress) {
            $scope.lstPhuongXa = [];
            crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", huyenId)
                .success(function (data) {
                    $scope.lstPhuongXa = data;
                    $scope.TiepDan.IdPhuongXa = -1;
                    if(arrAddress.length > 2) {
                        var length = arrAddress.length;
                        var tenGoc = arrAddress[length - 3].trim();
                        var tenXa = "Xã " + tenGoc;
                        var tenPhuong = "Phường " + tenGoc;
                        var tenThitran = "Thị trấn " + tenGoc;
                        var objPhuongXa = $scope.lstPhuongXa.find(x => x.TenPhuongXa == tenGoc || x.TenPhuongXa == tenXa 
                            || x.TenPhuongXa == tenPhuong || x.TenPhuongXa == tenThitran);
                        console.log("objPhuongXa", objPhuongXa);
                        if(objPhuongXa){
                            $scope.TiepDan.IdPhuongXa = objPhuongXa.id;
                        }
                    }
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                });
        }

        var date = new Date();
        $scope.TiepDan.NgayTiep = ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();

        // Convert datetime
        var parseDate = function (value) {
            if (value) {
                return new Date(parseInt(value.replace("/Date(", "").replace(")/", "")));
            }
            return null;
        }

        //Lấy danh sách năm
        $scope.lstNam = [];
        var getListNam = function () {
            var currentTime = new Date();
            var nam = currentTime.getFullYear();
            $scope.namID = nam;
            for (i = nam; i > nam - 5; i--) {
                $scope.lstNam.push(i);
            }
        }

        // lấy danh sách loại đối tượng
        $scope.lstLoaiDoiTuong = [];
        var getlstLoaiDoiTuong = function () {
            crudService.getAll("/DmLoaiDoiTuong/getAll")
                .success(function (data) {
                    $scope.lstLoaiDoiTuong = data;
                });
        }        

        // lấy danh sách tỉnh thành
        $scope.lstTinhThanh = [];
        $scope.lstTinhThanhSearch = [];
        var getlstTinhThanh = function () {
            crudService.getAll("/DmTinhThanh/getAllDropDown")
                .success(function (data) {
                    $scope.lstTinhThanh = data.listTinhThanh;
                    $scope.lstTinhThanhSearch = data.listTinhThanh;

                    if ($scope.TiepDan.IdTinhThanh != -1) {
                        $scope.getlstQuanHuyen($scope.TiepDan.IdTinhThanh);
                    }
                }).error(function (error) {
                    $.notify("Lấy danh sách tỉnh thành bị lỗi.", "error");
                });;
        }
        var getlstTinhThanhCookie = function () {
            crudService.getAll("/DmTinhThanh/getAllDropDown")
                .success(function (data) {
                    $scope.lstTinhThanh = data.listTinhThanh;
                    $scope.lstTinhThanhSearch = data.listTinhThanh;
                }).error(function (error) {
                    $.notify("Lấy danh sách tỉnh thành bị lỗi.", "error");
                });;
        }

        // Lấy danh sách quận huyện
        $scope.lstQuanHuyen = [];
        $scope.getlstQuanHuyen = function (id) {
            crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", id)
                .success(function (data) {
                    $scope.lstQuanHuyen = data;
                    $scope.lstPhuongXa = [];
                    $scope.TiepDan.IdQuanHuyen = -1;
                    $scope.TiepDan.IdPhuongXa = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
                });
        }

        $scope.lstQuanHuyenSearch = [];
        $scope.getlstQuanHuyenSearch = function (id) {
            crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", id)
                .success(function (data) {
                    $scope.lstQuanHuyenSearch = data;
                    $scope.lstPhuongXaSearch = [];
                    $scope.IdQuanHuyenSearch = -1;
                    $scope.IdPhuongXaSearch = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
                });
        }

        var getlstQuanHuyenCookie = function (id) {
            crudService.get("/DmQuanHuyen/GetQuanHuyenByTinhThanh/", id)
                .success(function (data) {
                    $scope.lstQuanHuyen = data;                   
                }).error(function (error) {
                    $.notify("Lấy danh sách quận huyện bị lỗi.", "error");
                });
        }

        // Lấy danh sách Phường xã
        $scope.lstPhuongXa = [];
        $scope.getlstPhuongXa = function (id) {
            crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", id)
                .success(function (data) {
                    $scope.lstPhuongXa = data;
                    $scope.TiepDan.IdPhuongXa = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                });
        }

        $scope.lstPhuongXaSearch = [];
        $scope.getlstPhuongXaSearch = function (id) {
            crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", id)
                .success(function (data) {
                    $scope.lstPhuongXaSearch = data;
                    $scope.IdPhuongXaSearch = -1;
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                });
        }

        var getlstPhuongXaCookie = function (id) {
            crudService.get("/DmPhuongXa/GetPhuongXaByQuanHuyen/", id)
                .success(function (data) {
                    $scope.lstPhuongXa = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách phường xã bị lỗi.", "error");
                });
        }

        // Lấy danh sách quốc tịch
        $scope.lstQuocTich = [];
        var getlstQuocTich = function () {
            crudService.getAll("/DmQuocTich/getAll")
                .success(function (data) {
                    $scope.lstQuocTich = data;
                });
        }        

        // Lấy danh sách dân tộc
        $scope.lstDanToc = [];
        var getlstDanToc = function () {
            crudService.getAll("/DmDanToc/getAll")
                .success(function (data) {
                    $scope.lstDanToc = data;
                });
        }        

        // Lấy danh sách loại hồ sơ
        $scope.lstLoaiDonThu = [];
        var getlstLoaiDonThu = function () {
            crudService.getAll("/DmLoaiDonThu/getAll")
                .success(function (data) {
                    $scope.lstLoaiDonThu = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách loại hồ sơ bị lỗi.", "error");
                });
        }

        // Lấy danh sách loại KNTC by loại hồ sơ
        $scope.lstLoaiKNTC = [];
        $scope.getlstLoaiKNTC = function (id) {
            crudService.get("/DmLoaiKNTC/GetLoaiKNTCByLoaiDonThu/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTC = data;
                    $scope.lstLoaiKNTCChiTiet = [];
                    $scope.TiepDan.IdLoaiVuKNTC = '-1';
                    $scope.TiepDan.IdLoaiVuKNTCChiTiet = '-1';
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC bị lỗi.", "error");
                });
        }

        var getlstLoaiKNTCCookie = function (id) {
            crudService.get("/DmLoaiKNTC/GetLoaiKNTCByLoaiDonThu/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTC = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC bị lỗi.", "error");
                });
        }

        // Lấy danh sách loại KNTC chi tiết
        $scope.lstLoaiKNTCChiTiet = [];
        $scope.getlstLoaiKNTCChiTiet = function (id) {
            crudService.get("/DmLoaiKNTCCT/GetLoaiKNTCCTByLoaiKNTC/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTCChiTiet = data;
                    $scope.TiepDan.IdLoaiVuKNTCChiTiet = '-1';
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC chi tiết bị lỗi.", "error");
                });
        }

        var getlstLoaiKNTCChiTietCookie = function (id) {
            crudService.get("/DmLoaiKNTCCT/GetLoaiKNTCCTByLoaiKNTC/", id)
                .success(function (data) {
                    $scope.lstLoaiKNTCChiTiet = data;
                }).error(function (error) {
                    $.notify("Lấy danh sách loại KNTC chi tiết bị lỗi.", "error");
                });
        }


        // Lấy danh sách loại KNTC bằng nguồn hồ sơ
        //$scope.lstLoaiKNTCByNguonDonThu = [];
        //var getlstLoaiKNTCByNguonDonThu = function (id) {
        //    crudService.get("/DmLoaiKNTC/GetLoaiKNTCByNguonDonThu/", id)
        //        .success(function (data) {
        //            $scope.lstLoaiKNTCByNguonDonThu = data;
        //            $scope.TiepDan.IdNguonDonThu = '-1';
        //            console.log("getlstLoaiKNTCByNguonDonThu", data);
        //        }).error(function (error) {
        //            $.notify("Lấy danh sách loại KNTC bị lỗi.", "error");
        //        });
        //}

        //var getlstLoaiKNTCByNguonDonThuCookie = function (id) {
        //    crudService.get("/DmLoaiKNTC/GetLoaiKNTCByNguonDonThu/", id)
        //        .success(function (data) {
        //            $scope.lstLoaiKNTCByNguonDonThu = data;
        //        }).error(function (error) {
        //            $.notify("Lấy danh sách loại KNTC bị lỗi.", "error");
        //        });
        //}


        // Lấy danh sách phiên tiếp dân thuộc đơn vị
        $scope.getPhienTCDByDonViIncludingAll = function () {
            return $http({
                url: "/DmPhienTiepCongDan/GetPhienTCDByDonViIncludingAll",
                method: "GET",
                params: {
                    IdDonVi: $cookieStore.get("DeparmentId")
                }
            }).success(function (data) {
                $scope.lstPhienTCD = data;
            }).error(function (error) {
                console.log(error);
            });
        }

        // Init data
        var init = function () {

            var IdTiepCongDan = crudService.getCookie("IdTiepCongDan");
            $("#divLoading").show();

            $scope.getPhienTCDByDonViIncludingAll()
                .success(function (data) {
                    $scope.LstPhienTCDByDonViIncludingAll = data;
                    console.log("LstPhienTCDByDonViIncludingAll", data);
                    $("#divLoading").hide();
                })
                .error(function (error) {
                    console.log(error);
                    $("#divLoading").hide();
                });


            if (IdTiepCongDan != '' && IdTiepCongDan != null && IdTiepCongDan != undefined) {
                $("#divLoading").show();
                crudService.removeCookie("IdTiepCongDan");
                crudService.get("/TiepCongDan/GetInfoByID?id=", IdTiepCongDan)
                   .success(function (data) {
                       data.NgayTiep = parseDateV(data.NgayTiep);
                       data.NgayCap = parseDateV(data.NgayCap);
                       data.NgayTao = parseDateV(data.NgayTao);
                       $scope.TiepDan = data;

                       //getlstPhienTCD();
                       getNguonDonThu();
                       getlstLoaiDoiTuong();

                       getlstTinhThanhCookie();
                       getlstQuanHuyenCookie($scope.TiepDan.IdTinhThanh);
                       getlstPhuongXaCookie($scope.TiepDan.IdQuanHuyen);

                       getlstQuocTich();
                       getlstDanToc();                       
                       
                       getlstLoaiDonThu();

                       
                       $scope.getlstLoaiKNTC($scope.TiepDan.IdLoaiVu);

                       //getlstLoaiKNTCCookie($scope.TiepDan.IdLoaiVu);
                       ////getlstLoaiKNTCByNguonDonThuCookie($scope.TiepDan.IdNguonDonThu);
                       //getlstLoaiKNTCChiTietCookie($scope.TiepDan.IdLoaiVuKNTC);
                       

                       //if (data.KieuTiepDan == 1) {
                       //    $scope.CheckThongTinDonThu = true;
                       //    $scope.DonThu.Id = data.IdDonThu;
                       //    $scope.DonThu.IdLoaiDonThu = data.IdLoaiDonThu;
                       //    $scope.DonThu.IdLoaiKNTC = data.IdLoaiKNTC;
                       //    $scope.DonThu.IdLoaiKNTCChiTiet = data.IdLoaiKNTCChiTiet;
                       //    $scope.DonThu.NoiDungDonThu = data.NoiDungDonThu;
                       //    getFileByIdDonThu($scope.DonThu.Id);

                       //    getlstLoaiKNTCCookie($scope.DonThu.IdLoaiDonThu);
                       //    getlstLoaiKNTCChiTietCookie($scope.DonThu.IdLoaiKNTC);
                       //}
                       //else
                       //{
                       //    $scope.CheckThongTinDonThu = false;
                       //}
                           
                       $("#divLoading").hide();
                   }).error(function (error) {
                       $.notify("Không tìm thấy dữ liệu!", "error");
                       $("#divLoading").hide();
                   });
            }
            else {
                //getlstPhienTCD();
                //getlstNguonDonThu();
                getlstLoaiDoiTuong();
                getlstTinhThanh();
                getlstQuocTich();
                getlstDanToc();
                getlstLoaiDonThu();
                // --- Bổ sung logic theo yêu cầu ---
                $scope.TiepDan.IdLoaiVu = $scope.IdDonVi;
                $scope.getlstLoaiKNTC($scope.TiepDan.IdLoaiVu);
                // --- End bổ sung ---
                $("#divLoading").hide();
            }
        }

        init();

        //get data doc id
        var getFileByIdDonThu = function (id) {
            GetAllFileByObjectId(id, $scope.FileType)
                .success(function (data) {
                    $scope.lstDataForPlugin = data;
                });
        }

        var GetAllFileByObjectId = function (Id, FileType) {
            return $http({
                url: "/Files/GetAllFileByObjectId",
                method: "GET",
                params: {
                    Id: Id,
                    FileType: FileType
                }
            });
        }

        $scope.ThemMoi = function () {
            //    $scope.lstPage = [];
            $scope.TiepDan = {};
            $scope.TiepDan.KieuTiepDan = 1;
            $scope.TiepDan.IdPhienTCD = -1;
            $scope.TiepDan.IdDoiTuong = 1;
            $scope.TiepDan.GioiTinh = 0;
            $scope.TiepDan.IdTinhThanh = 16;
            $scope.TiepDan.IdQuanHuyen = -1;
            $scope.TiepDan.IdPhuongXa = -1;
            $scope.TiepDan.IdQuocTich = 1;
            $scope.TiepDan.IdDanToc = 1;
            $scope.TiepDan.IdLoaiVu = $scope.TiepDan.DepartmentId;
           
            $scope.TiepDan.IdLoaiVuKNTC = -1;
            $scope.TiepDan.IdLoaiVuKNTCChiTiet = -1;
            //$scope.TiepDan.LanGiaiQuyet = 0;
            $scope.TiepDan.LanTiepNhan = 0;
            $scope.TiepDan.Pre_Status = 0;
            $scope.TiepDan.NgayTiep = ('0' + date.getDate()).slice(-2) + '/' + ('0' + (date.getMonth() + 1)).slice(-2) + '/' + date.getFullYear();

            $scope.DonThu = {};
            //$scope.CheckThongTinDonThu = false;
            $scope.CheckThongTinDonThu = true;
            $scope.CheckThongTinNguoiBiKNTC = false;
            $scope.CheckThongTinNguoiDaiDien = false;
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

        $scope.ChangeKieuTiep = function () {
            if ($scope.CheckThongTinDonThu == true) // Tiếp có đơn
            {
                $scope.TiepDan.NoiDungTiepDan = null;
                $scope.TiepDan.KetQuaTiepDan = null;
            }
            else
            {
                $scope.DonThu.NoiDungDonThu = null;
            }
        }

        // Thực hiện lưu trên modal Insert Update
        $scope.preCreate = function (data) {
            console.log("Tiepcondan", data);
            if (data == null) {
                $("#txtNgayTiep").notify("Nhập ngày tiếp công dân", "error");
                return;
            }
            
            if ($scope.CheckThongTinDonThu == true) {
                $scope.TiepDan.KieuTiepDan = 1;

                if ($scope.CheckDonThuXuLy == false) {
                    $scope.DonThu.IdDonThuXuLy = 0;
                }
                else {
                    $scope.DonThu.IdDonThuXuLy = 1;
                }

                $scope.DonThu.IdNguonDon = 0; // Nguồn đơn trực tiếp
                $scope.DonThu.NgayNhap = data.NgayTiep;
                $scope.DonThu.IdDoiTuong = data.IdDoiTuong;
                $scope.DonThu.SoNguoi = data.SoNguoi;
                $scope.DonThu.HoTen = data.HoTen;
                $scope.DonThu.SoGiayTo = data.SoGiayTo;
                $scope.DonThu.GioiTinh = data.GioiTinh;
                $scope.DonThu.NgayCap = data.NgayCap;
                $scope.DonThu.NoiCap = data.NoiCap;
                $scope.DonThu.DiaChi = data.DiaChi;
                $scope.DonThu.IdPhuongXa = data.IdPhuongXa;
                $scope.DonThu.IdQuanHuyen = data.IdQuanHuyen;
                $scope.DonThu.IdTinhThanh = data.IdTinhThanh;
                $scope.DonThu.IdQuocTich = data.IdQuocTich;
                $scope.DonThu.IdDanToc = data.IdDanToc;

                $scope.DonThu.IdLoaiDonThu = data.IdLoaiVu;
                $scope.DonThu.IdLoaiKNTC = data.IdLoaiVuKNTC;
                $scope.DonThu.IdLoaiKNTCChiTiet = data.IdLoaiVuKNTCChiTiet;
                //$scope.DonThu.LanGiaiQuyet = data.LanGiaiQuyet;
                $scope.DonThu.LanTiepNhan = data.LanTiepNhan;
                $scope.DonThu.Pre_Status = data.Pre_Status;//
            }
            else
            {
                $scope.TiepDan.KieuTiepDan = 0;
            }

            if (data.Id == null || data.Id == "") {
                if (validate(data) == false) {
                    return;
                }
                create(data);
            }
            else {
                if (validate(data) == false) {
                    return;
                }
                update(data);
            }
        }

        var validate = function (data) {

            if (data.NgayTiep == null || data.NgayTiep == "") {
                $("#txtNgayTiep").notify("Nhập ngày tiếp công dân", "error");
                $("#txtNgayTiep").focus();
                return false;
            }

            if (data.IdPhienTCD == null || data.IdPhienTCD == -1) {
                $("#ddlPhienTCD").notify("Chọn phiên tiếp công dân", "error");
                $("#ddlPhienTCD").focus();
                return false;
            }

            if (data.IdDoiTuong == null || data.IdDoiTuong == -1) {
                $("#ddlLoaiDoiTuong").notify("Chọn loại đối tượng tiếp dân", "error");
                $("#ddlLoaiDoiTuong").focus();
                return false;
            }

            if (data.SoNguoi == null || data.SoNguoi == "") {
                $("#txtSoNguoi").notify("Nhập số người", "error");
                $("#txtSoNguoi").focus();
                return false;
            }

            if (data.HoTen == null || data.HoTen == "") {
                $("#txtHoTen").notify("Nhập họ tên", "error");
                $("#txtHoTen").focus();
                return false;
            }

            if (data.IdLoaiVu == null || data.IdLoaiVu == -1) {
                $("#ddlLoaiDonThu").notify("Chọn loại vụ", "error");
                $("#ddlLoaiDonThu").focus();
                return false;
            }

            //if (data.IdLoaiVu == 1 || data.IdLoaiVu == 2) {
            //    if (data.IdLoaiVuKNTC == null || data.IdLoaiVuKNTC == -1) {
            //        $("#ddlLoaiKNTC").notify("Chọn loại vụ khiếu nại tố cáo", "error");
            //        $("#ddlLoaiKNTC").focus();
            //        return false;
            //    }
            //}

            if (data.IdLoaiVuKNTC == null || data.IdLoaiVuKNTC == -1) {
                $("#ddlLoaiKNTC").notify("Chọn loại vụ khiếu nại tố cáo", "error");
                $("#ddlLoaiKNTC").focus();
                return false;
            }

            //if (data.LanTiepNhan == null) {
            //    $("#txtLanTiepNhan").notify("Nhập lần tiếp nhận", "error");
            //    $("#txtLanTiepNhan").focus();
            //    return false;
            //}

            if ($scope.CheckThongTinDonThu == true) {
                if ($scope.DonThu.NoiDungDonThu == null || $scope.DonThu.NoiDungDonThu == "") {
                    $("#txtNoiDungDonThu").notify("Nhập nội dung hồ sơ", "error");
                    $("#txtNoiDungDonThu").focus();
                    return false;
                }
            }
            else {
                if (data.NoiDungTiepDan == null || data.NoiDungTiepDan == "") {
                    $("#txtNoiDungTiep").notify("Nhập nội dung tiếp", "error");
                    $("#txtNoiDungTiep").focus();
                    return false;
                }

                if (data.KetQuaTiepDan == null || data.KetQuaTiepDan == "-1") {
                    $("#txtKetQuaTiep").notify("Nhập kết quả tiếp", "error");
                    $("#txtKetQuaTiep").focus();
                    return false;
                }
            }
        }

        // Hàm thực hiện Create
        var create = function (data) {
            //console.log(data); return;
            var dateSplit = "";
            if (data.NgayTiep != null && data.NgayTiep != "") {
                dateSplit = data.NgayTiep.split('/');
                data.NgayTiep = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
                $scope.DonThu.NgayNhap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.NgayCap != null && data.NgayCap != "") {
                dateSplit = data.NgayCap.split('/');
                data.NgayCap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
                $scope.DonThu.NgayCap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }            

            data.IdDonVi = $cookieStore.get("DeparmentId");
            data.IdNguoiTiep = $cookieStore.get("UserID");
            if(data.KieuTiepDan == 1) {
                data.NoiDungTiepDan = $scope.DonThu.NoiDungDonThu;
            }

            crudService.create("/TiepCongDan/Create", data)
                .success(function (data) {
                    $.notify("Tạo mới thông tin tiếp dân thành công!", "success");
                    $scope.TiepDan = data;
                    $scope.TiepDan.NgayTiep = parseDateV(data.NgayTiep);
                    $scope.TiepDan.NgayCap = parseDateV(data.NgayCap);

                    $scope.DonThu.IdTiepCongDan = data.Id;
                    $scope.DonThu.VuViecId = data.VuViecId;
                    $scope.DonThu.IdTrangThai = 0;
                    $scope.DonThu.IdDonViNhap = $cookieStore.get("DeparmentId");
                    $scope.DonThu.IdNguoiNhap = $cookieStore.get("UserID");
                    $scope.DonThu.IdDonViXuLy = $cookieStore.get("DeparmentId");

                    if ($scope.CheckDonThuXuLy == false) {
                        $scope.DonThu.IdTrangThai = 2; // Trạng thái xử lý hồ sơ
                    }
                    else {
                        $scope.DonThu.IdTrangThai = 99;
                    }

                    if ($scope.CheckThongTinDonThu == true) {
                        crudService.create("/DonThu/Create", $scope.DonThu)
                            .success(function (data) {
                                $.notify("Tạo mới thông tin hồ sơ thành công!", "success");
                                $scope.DonThu = data;

                                //Upload file sau khi tạo hồ sơ xong                    
                                if ($scope.fileList.length != 0) {
                                    var count = 0;
                                    for (var i = 0; i < $scope.fileList.length; i++) {
                                        fileUploadService.UploadedFile($scope.fileList[i].file, data.Id, $scope.FileType, data.IdDonThuGoc).then(function (data) {
                                            $scope.lstDataForPlugin.push(data);
                                        });
                                    }
                                    $scope.fileList = [];
                                }

                                $scope.XuLyDonThu = {};
                                $scope.XuLyDonThu.IdDonThu = data.Id;
                                $scope.XuLyDonThu.VuViecId = data.VuViecId;
                                $scope.XuLyDonThu.IdDonThuGoc = data.IdDonThuGoc;
                                $scope.XuLyDonThu.IdTrangThai = 1;
                                $scope.XuLyDonThu.IdDonVi = $cookieStore.get("DeparmentId");
                                $scope.XuLyDonThu.IdNguoiTao = $cookieStore.get("UserID");
                                crudService.create("/XuLyDonThu/Create", $scope.XuLyDonThu)
                            }).error(function (error) {
                                $.notify("Tạo mới thông tin hồ sơ thất bại!", "error");
                            });
                    }
                })
                .error(function (error) {
                    $.notify("Tạo mới thông tin tiếp dân thất bại!", "error");
                });
        }

        // Hàm thực hiện Update
        var update = function (data) {
            var dateSplit = "";
            if (data.NgayTiep != null && data.NgayTiep != "") {
                dateSplit = data.NgayTiep.split('/');
                data.NgayTiep = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
                $scope.DonThu.NgayNhap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }

            if (data.NgayCap != null && data.NgayCap != "") {
                dateSplit = data.NgayCap.split('/');
                data.NgayCap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
                $scope.DonThu.NgayCap = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];
            }
            data.IdDonVi = $cookieStore.get("DeparmentId");
            data.IdNguoiTiep = $cookieStore.get("UserID");
            crudService.update("/TiepCongDan/Update", data)
                .success(function (data) {
                    $.notify("Chỉnh sửa thông tin tiếp dân thành công!", "success");
                    $scope.TiepDan = data;
                    $scope.TiepDan.NgayTiep = parseDateV(data.NgayTiep);
                    $scope.TiepDan.NgayCap = parseDateV(data.NgayCap);

                    $scope.DonThu.IdTiepCongDan = data.Id;
                    $scope.DonThu.IdDonViNhap = $cookieStore.get("DeparmentId");
                    $scope.DonThu.IdNguoiNhap = $cookieStore.get("UserID");
                    $scope.DonThu.IdDonViXuLy = $cookieStore.get("DeparmentId");
                    $scope.DonThu.IdTrangThai = 2;
                    if ($scope.CheckThongTinDonThu == true) {
                        crudService.update("/DonThu/Update", $scope.DonThu)
                            .success(function (data) {
                                $.notify("Chỉnh sửa thông tin hồ sơ thành công!", "success");
                                $scope.DonThu = data;

                                //Upload file sau khi tạo hồ sơ xong                    
                                if ($scope.fileList.length != 0) {
                                    for (var i = 0; i < $scope.fileList.length; i++) {
                                        fileUploadService.UploadedFile($scope.fileList[i].file, data.Id, $scope.FileType, data.IdDonThuGoc).then(function (data) {
                                            $scope.lstDataForPlugin.push(data);
                                        });                                        
                                    }
                                    $scope.fileList = [];
                                }
                            }).error(function (error) {
                                $.notify("Chỉnh sửa thông tin hồ sơ thất bại!", "error");
                            });
                    }
                })
                .error(function (error) {
                    $.notify("Chỉnh sửa thông tin tiếp dân thất bại!", "error");
                });
        }

        // Chuyển trang tiếp dân
        $scope.GoSoTiepDan = function () {
            window.location.href = '../TiepCongDan/SoTiepDan';
        }

        //Load danh sách check hồ sơ
        var getData = function () {
            return $http({
                url: "/DonThu/SearchCheck",
                method: "GET",
                params: {
                    HoTen: $scope.HoTenSearch, CMTND: $scope.CMTNDSearch,
                    IdTinhThanh: $scope.IdTinhThanhSearch, IdQuanHuyen: $scope.IdQuanHuyenSearch, IdPhuongXa: $scope.IdPhuongXaSearch,
                    NoiDung: $scope.NoiDungSearch, order: $scope.Order, pageSize: $scope.pageSize, pageIndex: $scope.pageIndex
                }
            });
        }

        // show modal check hồ sơ
        $scope.ShowModalCheckDonThu = function (data) {
            if (data.HoTen != null) {
                $scope.HoTenSearch = data.HoTen;
            }
            else {
                $scope.HoTenSearch = '';
            }
            if (data.SoGiayTo != null) {
                $scope.CMTNDSearch = data.SoGiayTo;
            }
            else {
                $scope.CMTNDSearch = '';
            }
            if ($scope.DonThu.NoiDungDonThu != null) {
                $scope.NoiDungSearch = $scope.DonThu.NoiDungDonThu;
            }
            else {
                $scope.NoiDungSearch = '';
            }

            $scope.IdTinhThanhSearch = $scope.TiepDan.IdTinhThanh;
            if ($scope.IdTinhThanhSearch != -1) {
                $scope.getlstQuanHuyenSearch($scope.IdTinhThanhSearch);
            }

            $scope.lstDonThu = [];
        }

        // Hàm thực hiện check hồ sơ
        var InitCheck = function (data) {
            $("#divLoading").show();
            getData().success(function (data) {
                angular.forEach(data, function (item) {
                    item.NgayNhap = parseDate(item.NgayNhap);
                });
                $scope.lstDonThu = data;
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
               console.log(error);
              $("#divLoading").hide();
           });
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
                        item.NgayNhap = parseDate(item.NgayNhap);
                    });
                    $scope.lstDonThu = data;
                    $scope.totalRecords = data[0].TotalRecords;
                    $scope.pageCount = Math.ceil($scope.totalRecords / $scope.pageSize);
                })
                .error(function (error) {
                    $.notify("Không tìm thấy dữ liệu!", "error");
                });
            }
        }

        // Reload trang
        $scope.reloadCheck = function () {
            $scope.pageIndex = 1;
            $scope.currentPage.stt = 1;
            $scope.lstPage.splice(0);
            $scope.lstPage = [];
            InitCheck();
        }

        // upload file
        $scope.fileList = [];

        $scope.ImageProperty = {
            file: ''
        }
        $scope.FileType = 1;
        $scope.lstDataForPlugin = [];

        var ValidateSize = function (file) {
            var FileSize = file.size / 1024 / 1024; // in MB
            if (FileSize > 10) {
                alert('Hãy chọn file có kích cỡ < 10mb');
                return false;
            }
            return true;
        }

        $scope.selectFileforUpload = function (element) {
            var fileupload = element.files;
            for (var i = 0; i < fileupload.length; i++) {
                $scope.ImageProperty.file = fileupload[i];
                if (ValidateSize($scope.ImageProperty.file)) {
                    $scope.fileList.push($scope.ImageProperty);
                }
                $scope.ImageProperty = {};
                $scope.$apply();
            }
        }
        $scope.removeFile = function (delitemfile) {
            $scope.fileList.splice($scope.fileList.indexOf(delitemfile), 1);
        }

        $scope.deleteFile = function (id) {
            crudService.remove("/Files/DeleteFiles", id)
            .success(function (result) {
                $.notify("Xóa file thành công!", "success");
                getFileByIdDonThu($scope.DonThu.Id);
            })
            .error(function (error) {
                $.notify("Xóa file thất bại!", "error");
            });

        }
    }).factory('fileUploadService', function ($http, $q) {
        var fac = {};
        fac.UploadedFile = function (file, docId, type, idDonThuGoc) {
            var deferred = $q.defer();
            var formData = new FormData();
            formData.append("file", file);
            formData.append("objectId", docId);
            formData.append("type", type);
            formData.append("idDonThuGoc", idDonThuGoc);
            $http.post("/Files/CreateFile", formData,
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



