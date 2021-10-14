// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $(".boxClick").click(function () {
        debugger;
        var advertId = this.getAttribute("data-id");

        $.ajax({
            type: 'GET',
            //url: 'api/' + advertId,
            url: document.location.origin + '/api/' + advertId,
            dataType: "json",
            success: function (data) {
                var baseImageUrl = $("#hidImagesBaseUrl").val();
                if (data.imagePath) {
                    $("#detailImage").attr("src", "https://d2ichmjesuudq2.cloudfront.net/" + data.imagePath);
                }
                $("#detailTile").text(data.title);
                $("#detailDesc").text(data.description);
                $("#detailPrice").text("Price $" + data.title);

                $("#detailModal").modal();
            }
        });
    });
});

