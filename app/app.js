(function () {
    'use strict';

    // App 
    angular.module('app')
        .config(function ($routeProvider) {
            $routeProvider
                .when('/', {
                    templateUrl: '/app/face.template.html'
                })
                .when('/changes', {
                    templateUrl: "/app/changes.template.html"
                });
        })
        .controller("MainController", function ($scope, $location) {
            //Values for form
            $scope.form = {
                url: '',
                artist: '',
                album: '',
                tracklist: ''
            };

            //Page location
            $scope.location = '';

            //Change the page location
            $scope.change = function (page) {
                $scope.location = page;
                $location.path(page);
            }
        })
})();
