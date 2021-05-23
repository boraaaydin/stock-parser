// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    SetSearchText();
});
function SetSearchText() {
    $("#txtStockName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Stock/GetStockNameValueList/' + request.term,
                //data: JSON.stringify({ term: request.term }),
                dataType: "json",
                type: "POST",
                contentType: "application/json;charset=utf-8",
                //dataFilter:function (data) { return data; },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item, value1: item };
                        //return item;
                    }));
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
    });
}