
    var margin = { top: 30, right: 0, bottom: 30, left: 70 },
       width = 1000,
       height = 300;

    var parseDate = d3.time.format("%X").parse;

    var x = d3.time.scale()
        .range([0, width]);

    var y = d3.scale.linear()
        .range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom")
        .ticks(d3.time.minute, 15)
        .tickFormat(d3.time.format("%H:%M"));
        

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .innerTickSize(-width)
        .outerTickSize(0)
        .tickPadding(10);

    var line = d3.svg.line()
        .x(function (d) { return x(d.RecvDate); })
        .y(function (d) { return y(d.Value); })
        .interpolate("linear");



    var svg = d3.select("#AmperageGraph")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform", "translate(" + margin.left/2 + "," + margin.top/2 + ")");

    var ln = svg.append("path").attr("class", "line");
    


    var x_axis = svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")");
        

    var y_axis = svg.append("g")
            .attr("class", "y axis");


    x_axis.call(xAxis);
    y_axis.call(yAxis);



var unicom = angular.module("unicom", []);

unicom.controller('UnicomController', function UnicomController($scope,$interval,requests) {
    $scope.EWdata = {
        startDate: new Date(),
        endDate: new Date(),
        data: [],
        dataGraph: [],
        timeDiff:0
    };

    $scope.graphParam = 'CurrentElectricPower';

    $scope.updatedTime = null;

    // рассчиываю расход воды часовой на основе данных за посл 20 мин
    $scope.WaterByHour = function () {
        if ($scope.EWdata.data.length == 0) {
            return 0;
        }
        var to_ = moment($scope.EWdata.data[0].RecvDate);
        var from_ = moment($scope.EWdata.data[0].RecvDate).subtract(20, 'minutes');

       // выбираю записи из интервала
        var dataCalculate = $scope.EWdata.data.filter(function (e,i) {
            return e.RecvDate >= from_ && e.RecvDate <= to_ && e.TotalWaterRate>0;
        });

        var substractValue = 0;
        var secondsInterval = 0;
        // если данных > чем 2 записи, то рассчитываю на основе первого и последнего значения из интервала
        if (dataCalculate.length > 0) {
            // разность показаний
            substractValue = dataCalculate[0].TotalWaterRate - dataCalculate[dataCalculate.length - 1].TotalWaterRate;
            // разность в секундах
            secondsInterval = moment(dataCalculate[0].RecvDate).diff(moment(dataCalculate[dataCalculate.length - 1].RecvDate), "seconds");

            if (secondsInterval != 0) {
                return substractValue * 60 * 60 / secondsInterval;
            } else
                return 0;
            // нет данных за последние 20 мин
        } else {
            return 0;
        }
        
    };


    // удельный расход электроэнергии на основе показаний эл и воды за посл 20 мин
    $scope.YdelEnergy = function () {
        if ($scope.EWdata.data.length == 0) {
            return 0;
        }
        var to_ = moment($scope.EWdata.data[0].RecvDate);
        var from_ = moment($scope.EWdata.data[0].RecvDate).subtract(20, 'minutes');

        // выбираю записи из интервала
        var dataCalculate = $scope.EWdata.data.filter(function (e, i) {
            return e.RecvDate >= from_ && e.RecvDate <= to_ && e.TotalEnergy>0&&e.TotalWaterRate > 0;
        });

        var energ = 0;
        var water = 0;
        
        // если данных > чем 2 записи, то рассчитываю на основе первого и последнего значения из интервала
        if (dataCalculate.length > 0) {
            // разность показаний
            energ = dataCalculate[0].TotalEnergy - dataCalculate[dataCalculate.length - 1].TotalEnergy;
            // разность в секундах
            water = dataCalculate[0].TotalWaterRate-dataCalculate[dataCalculate.length - 1].TotalWaterRate;

            if (water != 0) {
                return energ / water;
            } else
                return 0;
            // нет данных за последние 20 мин
        } else {
            return 0;
        }

       
    }

 
    // посчитаный расход воды по табличным данным
    $scope.WaterRate = function () {
        var filtered = $scope.EWdata.data.filter(function (e, i) {
            return e.TotalWaterRate > 0;
        });
        if (filtered.length > 0) {
            return filtered[0].TotalWaterRate - filtered[filtered.length - 1].TotalWaterRate;
        } else {
            return 0;
        }
    };
    
    $scope.EnergyRate = function () {
        if ($scope.EWdata.data.length > 0) {
            return $scope.EWdata.data[0].TotalEnergy - $scope.EWdata.data[$scope.EWdata.data.length - 1].TotalEnergy;
        } else {
            return 0;
        }
    };

    // время
    $scope.intervalData = function() {
        if ($scope.EWdata.data.length > 0) {
            return moment.duration($scope.EWdata.data[0].RecvDate - $scope.EWdata.data[$scope.EWdata.data.length - 1].RecvDate).humanize();
        } else {
            return "0";
        }
    };

       
    // последние показания
    $scope.lastData = function() {
        if ($scope.EWdata.data.length > 0) {
        return $scope.EWdata.data[0];
        } else {
            return {
                
                    Id:null,
                    RecvDate: null,
                    TotalEnergy: null,
                    Amperage1: null,
                    Amperage2: null,
                    Amperage3: null,
                    Voltage1: null,
                    Voltage2: null,
                    Voltage3: null,
                    CurrentElectricPower: null,
                    TotalWaterRate: null,
                    WaterEnergy: null,
                    Errors: null,
                    Alarm: null,
                    Presure: null
            };
    }
    };

    // время, прошедшее после получения последних данных (сек)
    $scope.loseDataTime = function() {
        return ($scope.updatedTime - $scope.lastData().RecvDate) / 1000;
    };
   
    $scope.init = function (identity) {
        $scope.loadData(identity);
        
       
        $interval(function () { $scope.loadData(identity); }, 60000);
              
    };

    $scope.isShowAlert=function() {
        return $scope.lastData().RecvDate != null && $scope.lastData().Alarm != null && $scope.lastData().Alarm>0;
    }

    // обновить график
    $scope.updateGraph = function() {
        $scope.EWdata.dataGraph = [];
        $scope.EWdata.data.map(function (e, i) {
            $scope.EWdata.dataGraph.push({
                RecvDate: e.RecvDate,
                Value: +e[$scope.graphParam]
            });
        });


        x.domain([$scope.EWdata.startDate, $scope.EWdata.endDate]);
        y.domain([d3.min($scope.EWdata.dataGraph, function (d) { return d.Value; }) * 0.95, d3.max($scope.EWdata.dataGraph, function (d) { return d.Value; }) * 1.05]);  // d3.extent($scope.EWdata.dataGraph, function (d) { return d.Value; }));

        x_axis.call(xAxis);
        y_axis.call(yAxis);




        ln.datum($scope.EWdata.dataGraph)
           .attr("d", line);

        var dots = svg.selectAll("circle.dot").data($scope.EWdata.dataGraph);



        dots.enter()
            .append("circle")
            .attr("class", "dot")
            .attr("cx", function (d) { return x(d.RecvDate); })
            .attr("cy", function (d) { return y(d.Value); })
            .attr("r", 0.9);

        dots.attr("cx", function (d) { return x(d.RecvDate); })
            .attr("cy", function (d) { return y(d.Value); });

        dots.exit().remove();
    }
    // запрос данных
    $scope.loadData = function (identity) {
        showLoading(true);
        requests.getLastData(identity)
            .success(function (data) {
                $scope.EWdata.startDate = new Date(parseInt(data.StartDate.substr(6)));
                $scope.EWdata.endDate = new Date(parseInt(data.EndDate.substr(6)));
                $scope.EWdata.timeDiff = parseInt(data.TimeDiff);
                $scope.EWdata.data = [];
                data.DataTable.map(function(e, i) {
                    $scope.EWdata.data.push({
                        Id: e.Id,
                        RecvDate: new Date(parseInt(e.RecvDate.substr(6))),
                        TotalEnergy: e.TotalEnergy,
                        Amperage1: e.Amperage1,
                        Amperage2: e.Amperage2,
                        Amperage3: e.Amperage3,
                        Voltage1: e.Voltage1,
                        Voltage2: e.Voltage2,
                        Voltage3: e.Voltage3,
                        CurrentElectricPower: e.CurrentElectricPower,
                        TotalWaterRate: e.TotalWaterRate,
                        WaterEnergy: e.WaterEnergy,
                        Errors: e.Errors,
                        Alarm: e.Alarm,
                        Presure:e.Presure
                    });
                });

                $scope.updateGraph();
                
                
            }).error(function (error) { console.log(error); })
            .finally(function () {
                $scope.updatedTime = new Date();
                showLoading(false);
            });
    };


    $scope.getClassRowError = function(elem) {
        if (elem != null && elem.length > 0) {
            return 'bg-danger';
        }
        return '';
    };

});


unicom.service("requests", function ($http) {
    this.getLastData = function(identity) {
        return $http({
            url: '../GetDataBySmallPeriod',
            method: 'GET',
            dataType: 'json',
            cache: false,
            params: { 'identity': identity }
        });
    };
});



unicom.directive("alert", function () {
    return {
        restrict: 'E',
        transclude:true,
        template: '<div class="alert_ col-md-12 col-sm-12 col-xs-12 {{cls}}" ng-transclude></div>',
        scope: {
            cls: "@"
        },
        link: function (scope, element, attrs) {
            
        }
    }
});