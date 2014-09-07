var PUBLIC_BASE_URL = "";
var BASE_URL = "";
//var PUBLIC_BLOB_URL = "http://127.0.0.1:10000/devstoreaccount1/posters/";
//var PUBLIC_BLOB_URL = "https://moviemirchistorage1.blob.core.windows.net/posters/";
var PUBLIC_BLOB_URL = "https://moviemirchistorage.blob.core.windows.net/posters/";

var FormBuilder = function () {
    FormBuilder.prototype.GetTextField = function (id, placeholder, label) {
        var fieldContainer = $("<div/>");
        var lbl = $("<span/>").html(label);
        var txt = $("<input/>").attr("id", id).attr("placeholder", placeholder);
        $(fieldContainer).append(lbl);
        $(fieldContainer).append(txt);
        return fieldContainer;
    }

    FormBuilder.prototype.GetTextArea = function (id, placeholder, label) {
        var fieldContainer = $("<div/>");
        var lbl = $("<span/>").html(label);
        var txt = $("<textarea/>").attr("id", id).attr("placeholder", placeholder);
        $(fieldContainer).append(lbl);
        $(fieldContainer).append(txt);
        return fieldContainer;
    }

    FormBuilder.prototype.GetRadioButton = function (id, label, groupName, isChecked) {
        var fieldContainer = $("<div/>");
        var txt = $("<input/>").attr("type", "radio").attr("id", id).attr("value", label).attr("name", groupName).attr("checked", isChecked);
        var rbLabel = $("<label/>").attr("for", id).attr("style", "font-weight:normal").html(label);
        $(fieldContainer).append(txt);
        $(fieldContainer).append(rbLabel);
        return fieldContainer;
    }

    FormBuilder.prototype.GetCheckBox = function (id, label, isChecked) {
        var fieldContainer = $("<div/>");
        var txt = $("<input/>").attr("type", "checkbox").attr("id", id).attr("checked", isChecked);
        var lbl = $("<span/>").html(label);
        $(fieldContainer).append(txt);
        $(fieldContainer).append(lbl);
        return fieldContainer;
    }

    FormBuilder.prototype.GetFileUploadControl = function (id, placeholder, label) {
        var fieldContainer = $("<div/>");
        var lbl = $("<span/>").html(label);
        var file = $("<input/>").attr("id", id).attr("placeholder", placeholder).attr("type", "file");
        $(fieldContainer).append(lbl);
        $(fieldContainer).append(file);
        return fieldContainer;
    }
}

function CallHandler(queryString, OnComp) {
    $.ajax({
        url: BASE_URL + queryString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        responseType: "json",
        cache: false,
        success: OnComp,
        error: OnFail
    });
    return false;
}

function OnFail() { }

function CallController(queryString, paramName, data, OnComplete) {
    data = encodeURI(data);
    $.ajax({
        url: BASE_URL + queryString,
        data: { "data": data },
        type: 'POST',
        dataType: 'json',
        success: OnComplete,
        error: OnFail
    });

    return false;
}

function UploadSelectedFile(element, txtName, imgType) {

    var _URL = window.URL || window.webkitURL;
    var file = element.files[0];

    var sFileExtension = file.name.split('.')[file.name.split('.').length - 1];
    sFileExtension = sFileExtension.toLowerCase();
    if (sFileExtension == "jpg" || sFileExtension == "tif" || sFileExtension == "gif" || sFileExtension == "png" || sFileExtension == "jfif" || sFileExtension == "bmp") {
        var vid = element.id;
        vid = vid.replace("ctl00_ContentPlaceHolder1_fu_", "");

        if (file.size > 10000000) {
            alert('File too large!');
            return false;
        }

        var xhr = new XMLHttpRequest();
        var imageFetcher = new XMLHttpRequest();
        var maxId = 0;
        var type = "dynamic";
        var img;
        if ((file = element.files[0])) {
            img = new Image();
            img.onload = function () {
                xhr.onreadystatechange = function (event) {
                    var target = event.target ? event.target : event.srcElement;
                    if (target.readyState == 4) {
                        if (target.status == 200 || target.status == 304) {
                            //alert(target.responseText);

                            var ResJSON = [];
                            ResJSON = JSON.parse(target.responseText);
                            $("#imgProp").attr("orignal", ResJSON.FileUrl);

                            if (imgType == "reviewerPhotot") {
                                $(".single-poster").remove();
                                $(".search-result-container").children("ul").remove();

                                CallHandler("api/Reviewer", new Search().PopulateCriticsResult);
                            }

                            new Posters().AddSinglePoster(ResJSON.FileUrl, imgType);
                        }
                    }
                };

                xhr.open('POST', BASE_URL + 'Handler/UploadFile.ashx?name=' + $(txtName).val() + '&type=' + imgType, true);
                xhr.setRequestHeader('X-FILE-NAME', file.name);
                xhr.send(file);
            };

            //alert(_URL.createObjectURL(file));
            img.id = "UploadedImage";
            img.src = _URL.createObjectURL(file);

            /*$("#imgLocal").attr("src", _URL.createObjectURL(file));
            $("#localImg").show();

            $("#status").html("");
            $("#status").hide();*/
        }
    }
    else {
        $("#status").html("Please upload valid image file");
        $("#status").show();
    }
}

function IsValidURL(url) {
    var isValidURL = false;

    if (/^([a-z]([a-z]|\d|\+|-|\.)*):(\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?((\[(|(v[\da-f]{1,}\.(([a-z]|\d|-|\.|_|~)|[!\$&'\(\)\*\+,;=]|:)+))\])|((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=])*)(:\d*)?)(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*|(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)|((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)|((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)){0})(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(url)) {
        alert("valid url");
        isValidURL = true;
    } else {
        isValidURL = false;
        alert("invalid url");
    }

    return isValidURL;
}

function IsURLExists(url) {
    //var isUrlExist = false;
    var encodedURL = encodeURIComponent(url);

    var isUrlExist = true;

    /*$.ajax({
        url: "http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20html%20where%20url%3D%22" + encodedURL + "%22&format=json",
        type: "get",
        async: false,
        dataType: "json",
        success: function (data) {
            alert("success");
            isUrlExist = data.query.results != null;
        },
        error: function () {
            alert("fail");
            isUrlExist = false;
        }
    });*/

    return isUrlExist;
}