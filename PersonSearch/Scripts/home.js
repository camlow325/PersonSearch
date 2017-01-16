$(document).keypress(function (e) {
    if (e.which == 13) {
        searchUsingAjax();
    }
});

$(document).ready(function () {
    $('#search').click(function () {
        searchUsingAjax();
    });
    $('#name').focus();
    $('#status').hide();
});

function searchUsingAjax() {
    $('#status').text("Searching...").show();
    $("#search").attr("disabled", true);
    $("#name").attr("disabled", true);
    $("#delay").attr("disabled", true);
    $("#results tbody").empty();

    $.ajax({
        type: 'GET',
        url: '/search',
        data: { name: $('#name').val(), delay: $('#delay').val() },
        dataType: 'json',
        complete: function () {
            $("#search").attr("disabled", false);
            $("#name").attr("disabled", false);
            $("#delay").attr("disabled", false);
            $("#name").focus();
        },
        success: function (people) {
            $.each(people, function (i, val) {
                var row = $('<tr class="col-md-12 list-group">');
                if (people[i].Picture) {
                    row.append($('<td>').html(
                        "<img src='data:image/jpg;base64," +
                        people[i].Picture + "'/>"));
                } else {
                    row.append($('<td/>'));
                }
                row.append($('<td>').html(
                    "<label class=\"col-md-12 control-label\">" +
                    people[i].FirstName + " " +
                    people[i].LastName + "</label><p/>" +
                    "<span class=\"col-md-12\">" +
                    people[i].Address + "</span><br/>" +
                    "<span class=\"col-md-12\">" +
                    "Age: " + people[i].Age + "</span><br/>" +
                    "<span class=\"col-md-12\">" +
                    "Interests: " + people[i].Interests + "</span>"));
                $('#results tbody').append(row);
            });
            if (people.length < 1) {
                $('#status').text("No people found")
            } else {
                $('#status').hide();
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            $('#status').text("Search error: " + textStatus + ", " + errorThrown);
        }
    })
}