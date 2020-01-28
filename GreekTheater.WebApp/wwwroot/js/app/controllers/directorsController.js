var DirectorsController = function (directorService) {

    var init = function () {
        directorService.getDirectors(success, fail);
    };

    var success = function (data) {
        $.each(data, function (i) {
            $("#directors").append("<tr><td>" + data[i].name + "</td><td>" + data[i].age + "</td><td>" + data[i].performances.length + "</td><td>" + getPerformances(data[i].performances) + "</td></tr>");
        });
    };

    var getPerformances = function (data) {
        var performances = "";

        $.each(data, function (i) {
            performances += "<pre>[" + data[i] + "]</pre> ";
        });

        return performances;
    };

    var fail = function () {
        alert('Something failed!');
    };

    return {
        init: init
    }
}(DirectorService);