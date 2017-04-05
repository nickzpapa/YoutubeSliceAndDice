(function () {
    'use strict';

    // App 
    angular.module('app', ['ngRoute', 'ngCookies'])
        .config(function ($routeProvider)
        {
            $routeProvider
                .when('/', {
                    templateUrl: '/app/face.template.html'
                })
                .when('/changes', {
                    templateUrl: "/app/changes.template.html"
                });
        })
        .controller("MainController", function ($scope, $location, $http, $window, $cookies)
        {
            //Values for form
            $scope.form = {
                url: $cookies.get('url') || '',
                artist: $cookies.get('artist') || '',
                album: $cookies.get('album') || '',
                tracklist: $cookies.get('tracklist') || ''
            };
            $scope.isDownloading = false;

            $scope.message = "Waiting for album...";

            $scope.saveForm = function () {
                $cookies.put('url', $scope.form.url);
                $cookies.put('artist', $scope.form.artist);
                $cookies.put('album', $scope.form.album);
                $cookies.put('tracklist', $scope.form.tracklist);
                console.log($scope.form);
            };

            $scope.resetForm = function () {
                $scope.form.artist = '';
                $scope.form.album = '';
                $scope.form.tracklist = '';
            }

            // page location
            $scope.location = '';            
            $scope.change = function (page) {
                $scope.location = page;
                $location.path(page);
            }

            $scope.download = function () {
                $scope.message = "Processing request. This might take a couple minutes."
                $scope.isDownloading = true;
                var request = {
                    method: 'POST',
                    url: '/',
                    headers : {
                        'Content-Type': 'application/json',
                        'Accept': 'application/zip'
                    },
                    data: {
                        'url': $scope.form.url,
                        'artist': $scope.form.artist,
                        'album': $scope.form.album,
                        'tracklist': $scope.form.tracklist
                    }
                };

                console.log("Downloading ", request);

                $http(request).then(function (response) {
                    if (response.data.Error == "none") {
                        var songInfo = response.data;
                        $window.open(songInfo.Loc);
                        $scope.message = "Album finished downloading...";
                    }
                   // else if (response.data.Error == "unknown")
                   //     $scope.message = "There was an unknown error. Please try again later. You might not be able to download this video."
                    else
                        $scope.message = response.data.Error;
                    $scope.isDownloading = false;
                });


            }
        })
})();
