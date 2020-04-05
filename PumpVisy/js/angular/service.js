app.service("dataService",function dataService($http) {

    this.getmarkers = function() {
        return $http({
            url: baseurl + 'device/allmarkers',
            method: 'GET',
            dataType: 'json',
            cache: false
        });
    };

    this.insertmarker = function(marker) {
        return $http({
            url: baseurl + 'device/InsertNewMarker',
            method: 'POST',
            dataType: 'json',
            cache: false,
            data: marker
        });
    };

    this.getmarkerbyid = function(ObjectId) {
        return $http({
            url: baseurl + 'device/GetMarker',
            method: 'GET',
            dataType: 'json',
            cache: false,
            params: { 'ObjectId': ObjectId }
        });
    };

    this.updatemarker = function(marker) {
        return $http({
            url: baseurl + 'device/UpdateMarker',
            method: 'POST',
            dataType: 'json',
            cache: false,
            data: marker
        });
    };

    this.deletemarker = function(id) {
        return $http({
            url: baseurl + 'device/DeleteMarker',
            method: 'POST',
            dataType: 'json',
            cache: false,
            data: { 'ObjectId': id }
        });
    };

}
);