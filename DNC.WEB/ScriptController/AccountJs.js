var myApp = angular.module("myApp", ["ngRoute", "crud", 'ui.bootstrap', 'ngCookies']);
myApp.controller("AccountJs",
function ($location, $scope, $routeParams, myService, $http, $cookieStore) {
    var password = document.getElementById("password");
    var username = document.getElementById("username");
    var lbler = document.getElementById("lblerror");
    var lblus = document.getElementById("lbluse");
    var lblps = document.getElementById("lblpass");
    $('#username').change(function () {
        if (username.value === "") {
            $("#divLoading").hide();
            $("#lbluse").show();
            lblus.textContent = "Tài khoản không để trống";
            username.focus();
            return false;
        } else {
            $("#lbluse").hide();
            return true;
        }
    });
    $('#password').change(function () {
        if (password.value === "") {
            $("#divLoading").hide();
            $("#lblpass").show();
            lblps.textContent = "Mật khẩu không để trống";
            password.focus();
            return false;
        } else {
            $("#lblpass").hide();
            return true;
        }
    });
    var checklogin = function () {
        var user = {
            username: $scope.username,
            password: $scope.password
        };
        $("#divLoading").show();
        var getData = myService.UsersLogin(user);
        getData.then(function (msg) {
            $("#divLoading").hide();
            if (msg.data === "4") {
                $("#lblerror").show();
                lbler.textContent = "Tài khoản hoặc mật khẩu chưa đúng. Vui lòng nhập lại!";
                return false;
            }
            //if (msg.data === "5") {
            if (msg.data === "2" || msg.data === "11") {
                $("#divLoading").hide();
                $("#lblerror").show();
                lbler.textContent = "Tài khoản này đang bị khoá. Vui lòng liên hệ quản trị hệ thống!";
                return false;
            } 
            if (msg.data === "6") {
                $("#divLoading").hide();
                $("#lblerror").show();
                lbler.textContent = "Lỗi hệ thống. Vui lòng liên hệ quản trị hệ thống!";
                return false;
            }
            if (msg.data === "2") {
                $("#divLoading").hide();
                $("#lbluse").show();
                lblus.textContent = "Tài khoản không để trống";
                return false;
            }
            if (msg.data === "3") {
                $("#divLoading").hide();
                $("#lblpass").show();
                lblps.textContent = "Mật khẩu không để trống";
                return false;
            }

            $("#divLoading").hide();
            $("#lblerror").hide();
            $("#lbluse").hide();
            $("#lblpass").hide();

            if (msg.data === "11") {
                //window.location.href = "/dang-ky-tiep-dan/danh-sach";
                return;
            }
            else if (msg.data === "10") {
                window.location.href = "/dang-ky-tiep-dan/danh-sach";
            }
            else
                if (getParameterByName('url_redirect') == null || getParameterByName('url_redirect') == "")
                window.location.href = "/Home/Index";
            else
                window.location.href = getParameterByName('url_redirect');
        });
    }

    var getParameterByName = function(name) {
        var url = window.location.href;
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    $scope.doSomething = function () {
        if (event.keyCode === 13) {
            checklogin();
        }
    }
    $scope.LoginCheck = function () {
        $cookieStore.remove("ListFuction"); 
        $cookieStore.remove("SuperUser"); 
        checklogin();
    }
});
myApp.service("myService", function ($http) {
    this.UsersLogin = function (user) {
        var response = $http({
            method: "post",
            url: "/Account/SignIn",
            data: JSON.stringify(user),
            dataType: "json"
        });
        return response;
    }
});