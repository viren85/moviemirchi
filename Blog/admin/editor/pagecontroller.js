﻿angular.module('blogAdmin').controller('PageEditorController', ["$rootScope", "$scope", "$location", "$filter", "$log", "dataService", function ($rootScope, $scope, $location, $filter, $log, dataService) {
    $scope.id = editVars.id;
    $scope.page = newPage;
    $scope.lookups = [];
    $scope.selectedParent = {};
    $scope.fullScreen = false;
    $scope.security = $rootScope.security;
    $scope.UserVars = UserVars;
    $scope.root = $rootScope.SiteVars.ApplicationRelativeWebRoot;

    $scope.load = function () {
        var lookupsUrl = '/api/lookups';
        dataService.getItems(lookupsUrl)
        .success(function (data) {
            angular.copy(data, $scope.lookups);

            $scope.lookups.PageList.unshift({ OptionName: '-- none --', OptionValue: 'none' });

            if ($scope.id) {
                $scope.loadPage();
            }
            else {
                $scope.page.Parent = { OptionName: '-- none --', OptionValue: 'none' };
                $scope.selectedParent = selectedOption($scope.lookups.PageList, $scope.page.Parent.OptionValue);
            }
        })
        .error(function () {
            toastr.error("Error loading lookups");
        });
    }

    $scope.loadPage = function () {
        spinOn();
        var url = '/api/pages/' + $scope.id;
        dataService.getItems(url)
        .success(function (data) {
            angular.copy(data, $scope.page);
            // add "none" option
            if ($scope.page.Parent == null) {
                $scope.page.Parent = { OptionName: '-- none --', OptionValue: 'none' };
            }
            // remove self to avoid self-parenting
            $scope.removeSelf();

            $scope.selectedParent = selectedOption($scope.lookups.PageList, $scope.page.Parent.OptionValue);
            var x = $scope.selectedParent;

            editorSetHtml($scope.page.Content);
            spinOff();
        })
        .error(function () {
            toastr.error($rootScope.lbl.errorLoadingPage);
            spinOff();
        });
    }

    $scope.save = function () {
        if (!$('#form').valid()) {
            return false;
        }
        spinOn();
        $scope.page.Content = editorGetHtml();
        $scope.page.Parent = $scope.selectedParent;

        if ($scope.page.Slug.length == 0) {
            $scope.page.Slug = toSlug($scope.page.Title);
        }

        if ($scope.page.Id) {
            dataService.updateItem('/api/pages/update/foo', $scope.page)
           .success(function (data) {
               $scope.load();
               toastr.success("Page updated");
               $("#modal-form").modal('hide');
               spinOff();
           })
           .error(function () {
               toastr.error("Update failed");
               spinOff();
           });
        }
        else {
            dataService.addItem('/api/pages', $scope.page)
           .success(function (data) {
               toastr.success("Page added");
               $log.log(data);
               if (data.Id) {
                   angular.copy(data, $scope.page);
                   var x = $scope.page.Id;
               }
               $("#modal-form").modal('hide');
               spinOff();
           })
           .error(function () {
               toastr.error("Failed adding new page");
               spinOff();
           });
        }
    }

    $scope.uploadFile = function (action, files) {
        var fd = new FormData();
        fd.append("file", files[0]);

        dataService.uploadFile("/api/upload?action=" + action, fd)
        .success(function (data) {
            toastr.success("Uploaded");
            if (action === "image") {
                insertAtCursor('<img src=' + data + ' />');
            }
            if (action === "video") {
                insertAtCursor('<p>[video src=' + data + ']</p>');
            }
            if (action === "file") {
                var res = data.split("|");
                if (res.length === 2) {
                    insertAtCursor('<a href="' + res[0].replace('"', '') + '">' + res[1].replace('"', '') + '</a>');
                }
            }
        })
        .error(function () { toastr.error("Import failed"); });
    }

    $scope.load();

    $(document).ready(function () {
        $.validator.addMethod(
            "dateFormatted",
            function (value, element) {
                var re = /^\d{4}-\d{1,2}-\d{1,2}\s([0-9]|[0-1][0-9]|[2][0-3]):([0-5][0-9])$/;
                return (this.optional(element) && value == "") || re.test(value);
            },
            "yyyy-mm-dd hh:mm"
        );
        $('#form').validate({
            rules: {
                txtTitle: { required: true }
            }
        });
    });

    $scope.removeSelf = function () {
        for (var i = 0; i < $scope.lookups.PageList.length; i++) {
            if ($scope.lookups.PageList[i].OptionValue === $scope.id) {
                $scope.lookups.PageList.splice(i, 1);
            }
        }
    }
}]);

var newPage = {
    "Id": "",
    "Title": "",
    "Content": "",
    "DateCreated": moment().format("YYYY-MM-DD HH:mm"),
    "Slug": "",
    "ShowInList": true,
    "IsPublished": true
}