(function () {
    'use strict';

    // App Components
    angular.module('app')
        .component('zFace', {
            templateUrl: "/app/face.template.html",
        })
        .component('zChanges', {
            templateUrl: "/app/changes.template.html"
        });

    // App Controller
    angular
    	.module('app')
        .controller("MainController", ["$scope", function ($scope) {
            $scope.current = 'zFace';
            $scope.pages =  ['zFace', 'zChanges'];
            $scope.change = function (page) {
                console.log(page);
            	if($scope.pages.indexOf(page) > -1)
            		$scope.current = page;
            }
        }]);
})();
