var HttpService = {
    get: function (url, value) {
        value = (value == undefined) ? '' : value;
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "GET",
                url: url + value,
                success: function(data) {
                    resolve(data);
                },
                error: function(err) {
                    reject(err);
                }
            });
        });
    },
    post: function(url, value) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "POST",
                url: url,
                data: value,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function(data) {
                    resolve(data);
                },
                error: function(err) {
                    reject(err);
                }
            });
        });
    }
}
