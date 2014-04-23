
var BASE_URL = "http://127.0.0.1:81/";

function autoCompleteTextBox1(searchText, url, targetDiv, targetUL, targetText, hiddenFeild) {
    //Here we are using ajax get method to fetch data from the list based on the user entered value in the textbox.
    //We are sending query i.e textbox as data.
    var fullUrl = BASE_URL + url;
    //var fullUrl = url;

    $.ajax({
        //url: '@Url.Action("AutoComplete", "Movie")',
        url: fullUrl,
        //url: "",
        data: { "query": searchText },
        type: 'POST',
        dataType: 'json',
        success: function (response) { PopulateSearchResult1(response, targetDiv, targetUL, targetText, hiddenFeild); },
        error: function (xhr, status, error) {
        }
    });
}

function PopulateSearchResult1(response, targetDiv, targetUL, targetText, hiddenFeild) {
    if (response != null) {
        if ($("#" + targetUL) != undefined) {
            //If the UL element is not null or undefined we are clearing it, so that the result is appended in new UL every next time.
            $("#" + targetUL).remove();
        }
        //assigning json response data to local variable. It is basically list of values.
        data = response;

        if (data.length < 1 || data.length == undefined) {
            $("#" + targetDiv).append($("<ul id='" + targetUL + "' style='display:none;'></ul>"));
        }
        else {
            //appending an UL element to show the values.
            $("#" + targetDiv).append($("<ul id='" + targetUL + "' class='targetAutoUL' style='display:block;'></ul>"));
            //Removing previously added li elements to the list.
            $("#" + targetUL).find("li").remove();
            //We are iterating over the list returned by the json and for each element we are creating a li element and appending the li element to ul element.
            $.each(data, function (i, value) {
                //On click of li element we are calling a method.                
                $("#" + targetUL).append($("<li class='targetLI' title='click to select " + value.name + "' onclick='javascript:appendTextToTextBox1(this,\"" + targetText + "\",\"" + targetUL + "\", \"" + hiddenFeild + "\")' itemId='" + value.id + "'>" + value.name + "</li>"));
            });
        }
    }
    else {
        //If data is null the we are removing the li and ul elements.
        $("#" + targetUL).find("li").remove();
        $("#" + targetUL).remove();
    }
}
//This method appends the text oc clicked li element to textbox.
function appendTextToTextBox1(e, targetText, targetUL, hiddenFeild) {
    //Getting the text of selected li element.
    var textToappend = e.innerText;
    //setting the value attribute of textbox with selected li element.
    $("#" + targetText).val(textToappend);
    //Removing the ul element once selected element is set to textbox.
    $("#" + targetUL).remove();
    $("#" + targetUL).css("display", "none");

    $("#" + hiddenFeild).val($(e).attr("itemid"));
}