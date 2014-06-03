var PUBLIC_BASE_URL = "http://127.0.0.1:81";
var BASE_URL = "http://127.0.0.1:8080/";

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
        var txt = $("<input/>").attr("type", "radio").attr("id", id).attr("text", label).attr("name", groupName).attr("checked", isChecked);
        $(fieldContainer).append(txt);
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