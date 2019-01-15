app.service("dataService",function dataService($http) {

    this.getmarkers = function() {
        return $http({
            url: '././device/allmarkers',
            method: 'GET',
            dataType: 'json',
            cache: false
        });
    };

    this.insertmarker = function(marker) {
        return $http({
            url: '././device/InsertNewMarker',
            method: 'POST',
            dataType: 'json',
            cache: false,
            data: marker
        });
    };

    this.getmarkerbyid = function(ObjectId) {
        return $http({
            url: '././device/GetMarker',
            method: 'GET',
            dataType: 'json',
            cache: false,
            params: { 'ObjectId': ObjectId }
        });
    };

    this.updatemarker = function(marker) {
        return $http({
            url: '././device/UpdateMarker',
            method: 'POST',
            dataType: 'json',
            cache: false,
            data: marker
        });
    };

    this.deletemarker = function(id) {
        return $http({
            url: '././device/DeleteMarker',
            method: 'POST',
            dataType: 'json',
            cache: false,
            data: { 'ObjectId': id }
        });
    };

}
);