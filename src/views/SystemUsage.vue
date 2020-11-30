<template>
  <v-container fluid>
    
    <v-card elevation="2">
      <v-card-title>System usage (2 seconds delay)</v-card-title>
     <canvas v-vmOn="{CurrentCpuUsage: updateLineChart}"></canvas>
    </v-card>

  </v-container>
</template>

<script>
import dotnetify from 'dotnetify/vue';
import Chart from 'chart.js';
import '@taeuk-gang/chartjs-plugin-streaming';

export default {
  name: 'SystemUsage',
  created() {
    this.vm = dotnetify.vue.connect("SystemUsage", this);
  },
  data() {
    return {
      CurrentCpuUsage: 0,
      CurrentMemoryUsage: 0
    }
  },
  destroyed() {
    this.vm.$destroy();
  },
  methods: {
    createLineChart: function (elem) {
      const chartData = {
        type: 'line',
        data: {
          labels: [],
          datasets: [{
            label: 'CPU Usage %',
            backgroundColor: Chart.helpers.color('rgb(255, 99, 132)').alpha(0.5).rgbString(),
            borderColor: 'rgb(255, 99, 132)',
            fill: false,
            cubicInterpolationMode: 'monotone',
            borderDash: [8, 4],
          },
          {
            label: 'Memory Usage %',
            backgroundColor: Chart.helpers.color('rgb(54, 162, 235)').alpha(0.5).rgbString(),
            borderColor: 'rgb(54, 162, 235)',
            fill: false,
            cubicInterpolationMode: 'monotone',
            data: []
          }]
        },
        options: {
          title: {
            display: true,
            text: 'System usage graphs'
          },
          scales: {
            xAxes: [
              {
                type: 'realtime',
                realtime: {
                  duration: 20000,
                  refresh: 1000,
                  delay: 2000
                }
              }
            ],
            yAxes: [{
              scaleLabel: {
                display: true,
                labelString: 'value'
              }
            }]
          },
          tooltips: {
            mode: 'nearest',
            intersect: false
          },
          hover: {
            mode: 'nearest',
            intersect: false
          }
        }
      };
      const chartOptions = { responsive: true };
      return new Chart(elem.getContext('2d'), chartData, chartOptions);
    },
    updateLineChart: function (element) {
      if (!this.lineChart) {
        this.lineChart = this.createLineChart(element);
      } else {
        this.onRefresh(this.lineChart, this.CurrentCpuUsage, this.CurrentMemoryUsage);
      }
    },
    onRefresh(chart, cpu, memory) {
      chart.config.data.datasets[0].data.push({
        x: Date.now(),
        y: cpu
      });

      chart.config.data.datasets[1].data.push({
        x: Date.now(),
        y: memory
      });
      
      chart.update();
    }
  }
}
</script>
