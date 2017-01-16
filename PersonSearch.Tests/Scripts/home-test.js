/// <reference path="../../personsearch/scripts/jquery-1.10.2.js" />
/// <reference path="../../personsearch/scripts/home.js" />

QUnit.test('controls set properly during search', function () {
    var fixture = $("#qunit-fixture");
    fixture.append('<label id="status"/>');
    fixture.append('<input id="search" type="button"/>');
    fixture.append('<input id="name" value="Smith"/>');
    fixture.append('<input id="delay" value="10"/>');
    fixture.append('<table id="results"><tbody>' +
        '<tr>some old row</tr></tbody></table>');

    jQuery.ajax = function (param) { };

    searchUsingAjax();

    equal(fixture.children("#status").html(), "Searching...",
        "wrong status text");
    equal(fixture.children("#search").attr("disabled"), "disabled",
        "search button not disabled");
    equal(fixture.children("#name").attr("disabled"), "disabled",
        "name not disabled");
    equal(fixture.children("#delay").attr("disabled"), "disabled",
        "delay not disabled");
    equal(fixture.find("#results > tbody").html(), "", "results not empty");
});

QUnit.test('ajax call made with proper parameters', function () {
    var fixture = $("#qunit-fixture");
    fixture.append('<input id="name" value="Smith"/>')
    fixture.append('<input id="delay" value="10"/>');

    var options = null;
    jQuery.ajax = function (param) {
        options = param;
    };

    searchUsingAjax();

    deepEqual(options.data, { "delay": "10", "name": "Smith" },
        "ajax request data parameters");
    equal(options.dataType, "json", "wrong ajax request data type");
    equal(options.type, "GET", "wrong ajax request method");
    equal(options.url, "/search", "wrong ajax request endpoint");
});

QUnit.test('controls set properly after search', function () {
    var fixture = $("#qunit-fixture");
    fixture.append('<label id="status"/>');
    fixture.append('<input id="search" type="button"/>');
    fixture.append('<input id="name" value="Smith"/>');
    fixture.append('<input id="delay" value="10"/>');
    fixture.append('<table id="results"><tbody>' +
        '<tr>some old row</tr></tbody></table>');

    var options = null;
    jQuery.ajax = function (param) {
        options = param
    };

    searchUsingAjax();

    options.success([]);
    options.complete();

    equal(fixture.children("#status").html(), "No people found",
        "wrong status text");
    equal(fixture.children("#search").attr("disabled"), undefined,
        "search button not enabled");
    equal(fixture.children("#name").attr("disabled"), undefined,
        "name not enabled");
    equal(fixture.children("#delay").attr("disabled"), undefined,
        "delay not enabled");
    equal(fixture.find("#results > tbody").html(), "", "results not empty");
    equal($(document.activeElement).attr("id"), "name",
        "focus not set to name");
});

QUnit.test('successful ajax call fills in search results', function () {
    var fixture = $("#qunit-fixture");
    fixture.append('<label id="status"/>');
    fixture.append('<table id="results"><tbody></tbody></table>');

    var options = null;
    jQuery.ajax = function (param) {
        options = param;
    };

    searchUsingAjax();

    var people = [{
        "FirstName": "John", "LastName": "Doe",
        "Address": "1234 Anywhere Dr", "Age": 50, "Interests": "skydiving",
        "Picture": "ABDEFG"
    },
    {
        "FirstName": "Jane", "LastName": "Smith",
        "Address": "5678 Nowhere Ave", "Age": 37, "Interests": "golfing",
        "Picture": "HIJKLM"
    },
    {
        "FirstName": "Bob", "LastName": "Jones",
        "Address": "6275 Somewhere St", "Age": 58, "Interests": "kayaking",
        "Picture": "OPQRST"
    }];

    options.success(people);

    equal(fixture.children("#status").is(":visible"), false,
        "status not hidden");
    var results = fixture.find('#results > tbody > tr');
    equal(results.length, people.length,
        "wrong number of rows in results");
    results.each(function (index, row) {
        var person = people[index];
        var expectedRow = '<td><img src="data:image/jpg;base64,' +
            person.Picture + '\"></td><td>' +
            '<label class="col-md-12 control-label">' +
            person.FirstName + ' ' + person.LastName + '</label><p></p>' +
            '<span class="col-md-12">' + person.Address + '</span>' +
            '<br><span class="col-md-12">Age: ' + person.Age + '</span><br>' +
            '<span class="col-md-12">Interests: ' + person.Interests +
            '</span></td>';
        equal($(row).html(), expectedRow, "wrong content for row " + index);
    });
});

QUnit.test('error during ajax call fills in search results', function () {
    var fixture = $("#qunit-fixture");
    fixture.append('<label id="status"/>');
    fixture.append('<table id="results"><tbody></tbody></table>');

    var options = null;
    jQuery.ajax = function (param) {
        options = param;
    };

    searchUsingAjax();

    options.error({}, "some error status", "some error text");

    equal(fixture.children("#status").html(),
        "Search error: some error status, some error text",
        "wrong status text");
    equal(fixture.find("#results > tbody").html(), "", "results not empty");
});