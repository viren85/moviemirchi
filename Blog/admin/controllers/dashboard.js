angular.module('blogAdmin').controller('DashboardController', ["$rootScope", "$scope", "$location", "$log", "dataService", function ($rootScope, $scope, $location, $log, dataService) {
    $scope.stats = {};
    $scope.draftposts = [];
    $scope.draftpages = [];
    $scope.recentcomments = [];
    $scope.trash = [];
    $scope.packages = [];
    $scope.logItems = [];
    $scope.itemToPurge = {};
    $scope.security = $rootScope.security;

    $scope.openLogFile = function () {
        dataService.getItems('/api/logs/getlog/file')
        .success(function (data) {
            angular.copy(data, $scope.logItems);
            $("#modal-log-file").modal();
            return false;
        })
        .error(function (data) {
            toastr.error($rootScope.lbl.errorGettingLogFile);
        });
    }

    $scope.purgeLog = function () {
        dataService.updateItem('/api/logs/purgelog/file', $scope.itemToPurge)
        .success(function (data) {
            $scope.logItems = [];
            $("#modal-log-file").modal('hide');
            toastr.success($rootScope.lbl.purged);
            return false;
        })
        .error(function (data) {
            toastr.error($rootScope.lbl.errorPurging);
        });
    }

    $scope.purge = function (id) {
        if (id) {
            $scope.itemToPurge = findInArray($scope.trash.Items, "Id", id);
        }
        dataService.updateItem('/api/trash/purge/' + id, $scope.itemToPurge)
        .success(function (data) {
            $scope.loadTrash();
            toastr.success($rootScope.lbl.purged);
            return false;
        })
        .error(function (data) {
            toastr.error($rootScope.lbl.errorPurging);
        });
    }

    $scope.purgeAll = function () {
        dataService.updateItem('/api/trash/purgeall/all')
        .success(function (data) {
            $scope.loadTrash();
            toastr.success($rootScope.lbl.purged);
            return false;
        })
        .error(function (data) {
            toastr.error($rootScope.lbl.errorPurging);
        });
    }

    $scope.restore = function (id) {
        if (id) {
            $scope.itemToPurge = findInArray($scope.trash.Items, "Id", id);
        }
        dataService.updateItem('/api/trash/restore/' + id, $scope.itemToPurge)
        .success(function (data) {
            $scope.loadTrash();
            toastr.success($rootScope.lbl.restored);
            return false;
        })
        .error(function (data) {
            toastr.error($rootScope.lbl.errorRestoring);
        });
    }

    $scope.load = function () {
        if ($rootScope.security.showTabDashboard === false) {
            window.location = "../Account/Login.aspx";
        }
        $("#versionMsg").hide();
        spinOn();

        $scope.loadPackages();

        dataService.getItems('/api/stats')
            .success(function (data) { angular.copy(data, $scope.stats); })
            .error(function (data) { toastr.success($rootScope.lbl.errorGettingStats); });

        dataService.getItems('/api/posts', { take: 3, skip: 0, filter: "IsPublished == false" })
            .success(function (data) { angular.copy(data, $scope.draftposts); })
            .error(function () { toastr.error($rootScope.lbl.errorLoadingDraftPosts); });

        dataService.getItems('/api/pages', { take: 3, skip: 0, filter: "IsPublished == false" })
            .success(function (data) { angular.copy(data, $scope.draftpages); })
            .error(function () { toastr.error($rootScope.lbl.errorLoadingDraftPages); });

        dataService.getItems('/api/comments', { type: 5, take: 5, skip: 0, filter: "IsDeleted == false", order: "DateCreated descending" })
            .success(function (data) { angular.copy(data, $scope.recentcomments); })
            .error(function () { toastr.error($rootScope.lbl.errorLoadingRecentComments); });

        dataService.getItems('/api/logs/getlog/file')
            .success(function (data) {
                angular.copy(data, $scope.logItems);
                if ($scope.logItems.length > 0) { $('#tr-log-spinner').hide(); }
                else { $('#div-log-spinner').html($rootScope.lbl.empty); }
            })
            .error(function (data) { toastr.error($rootScope.lbl.errorGettingLogFile); });

        $scope.loadTrash();
    }

    $scope.loadPackages = function () {
        if (!$scope.security.showTabCustom) {
            return;
        }
        dataService.getItems('/api/packages', { take: 5, skip: 0 })
        .success(function (data) {
            angular.copy(data, $scope.packages);
            $scope.checkNewVersion();
            if ($scope.packages.length > 0) { $('#tr-gal-spinner').hide(); }
            else { $('#div-gal-spinner').html(BlogAdmin.i18n.empty); }
        })
        .error(function () {
            toastr.error($rootScope.lbl.errorLoadingPackages);
        });
    }

    $scope.checkNewVersion = function () {
        if (!$scope.security.showTabCustom) {
            return;
        }
        var version = SiteVars.Version.substring(15, 22);
        $.ajax({
            url: SiteVars.ApplicationRelativeWebRoot + "api/setup?version=" + version,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data && data.length > 0) {
                    $("#vNumber").html(data);
                    $("#versionMsg").show();
                }
            }
        });
    }

    $scope.loadTrash = function () {
        dataService.getItems('/api/trash', { type: 0, take: 5, skip: 0 })
            .success(function (data) { angular.copy(data, $scope.trash); })
            .error(function () { toastr.error($rootScope.lbl.errorLoadingTrash); });
    }

    $scope.load();
}]);