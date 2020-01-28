var DirectorService = function () {

    var getDirectors = function (success, fail) {
        $.ajax({
            url: "http://localhost:51045/api/directors",
            method: "GET"
        })
            .then(success)
            .fail(fail);
    };

    return {
        getDirectors: getDirectors
    }
}();