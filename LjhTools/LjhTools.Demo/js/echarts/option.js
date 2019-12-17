﻿function GetOptions(myseries, mycategories) {
    // 指定图表的配置项和数据
    var option = {
        title: {
            //text: 'ECharts 入门示例'
        },
        tooltip: {},
        legend: {
            data: ['']
        },
        dataZoom: [{
            type: 'inside',
            //start: 80,
            // 结束位置的百分比，0 - 100
            //end: 100,
            // 开始位置的数值
            startValue: 0,
            // 结束位置的数值
            endValue: 5
        }, {
            type: 'slider',
        }],

        xAxis: {
            type: 'category',
            data: ["04-01", "05-01", "06-01", "07-01", "08-01", "09-01", "05-01", "06-01", "07-01", "08-01", "09-01", "05-01", "06-01", "07-01", "08-01", "09-01", "05-01", "06-01", "07-01", "08-01", "09-01", "05-01", "06-01", "07-01", "08-01", "09-01"]
        },
        yAxis: {},
        series: [{
            //name: '销量',
            type: 'line',
            data: [500000, 200000, 360000, 100000, 100000, 200000, 200000, 360000, 100000, 100000, 200000, 200000, 360000, 100000, 100000, 200000, 200000, 360000, 100000, 100000, 200000, 200000, 360000, 100000, 100000, 200000],
            smooth: true
        }]
    };