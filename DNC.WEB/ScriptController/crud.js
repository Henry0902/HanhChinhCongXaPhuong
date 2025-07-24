angular.module("crud", [])
.service("crudService", function ($http) {
    this.getAll = function (url) {
        var response = $http({
            method: "GET",
            url: url
        });
        return response;
    }

    this.get = function (url, id) {
        return $http.get(url + id);
    }

    this.create = function (url, data) {
        var request = $http({
            method: "POST",
            url: url,
            data: JSON.stringify(data),
            dataType: "json"
        })
        return request;
    }

    this.update = function (url, data) {
        var request = $http({
            method: "POST",
            url: url,
            data: JSON.stringify(data),
            dataType: "json"
        })
        return request;
    }

    this.remove = function (url, id) {
        var request = $http({
            method: "POST",
            url: url + '?id=' + id
        })
        return request;
    }

    this.setCookie = function (key, value) {
        var d = new Date();
        d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toGMTString();
        document.cookie = key + "=" + value + ";" + expires + ";path=/";
    }

    this.removeCookie = function (key) {
        document.cookie = key + '=; expires=1;path=/';
    }

    this.getCookie = function (cname) {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
})

