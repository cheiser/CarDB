$(function () {
    $("#Year").blur(function () {
        var yearVal = $("#Year").val();
        if (yearVal.match(/[0-9]/g).length == 4) {
            $("#Year").val(yearVal + "-01-01"); // quick ugly hack to bypass the
            // DateTime dependency without changing the type to int and also to
            // test jQuery together with MVC.
        }
    });
});