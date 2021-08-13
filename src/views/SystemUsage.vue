<template>
  <v-container fluid>
    
    <v-card elevation="2">
      <v-card-title dense>System usage</v-card-title>
      <v-list-item>
        <canvas id="usageGraph"></canvas>
      </v-list-item>
    </v-card>

  </v-container>
</template>

<style>
#usageGraph {
  overflow-y: visible;
}
</style>

<script>
import dotnetify from 'dotnetify/vue';
import Chart from 'chart.js/auto';
import 'chartjs-adapter-luxon';
import ChartStreaming from 'chartjs-plugin-streaming';

export default {
  name: 'SystemUsage',
  created() {
    this.vm = dotnetify.vue.connect("SystemUsage", this);
  },
  mounted() {
    Chart.register(ChartStreaming);
    this.lineChart = this.createLineChart(document.getElementById("usageGraph"));
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
            backgroundColor: 'rgb(255, 99, 132, 0.5)',
            borderColor: 'rgb(255, 99, 132)',
            fill: false,
            cubicInterpolationMode: 'monotone',
            borderDash: [8, 0],
            data: []
          },
          {
            label: 'Memory Usage %',
            backgroundColor: 'rgb(54, 162, 235, 0.5)',
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
            x: {
                type: 'realtime',
                realtime: {
                  duration: 60000,
                  refresh: 1000,
                  delay: 2000,
                  onRefresh: this.onRefresh.bind(this)
                }
              },
            y: {
              scaleLabel: {
                display: true,
                labelString: 'value'
              }
            }
          },
          tooltips: {
            mode: 'nearest',
            intersect: false
          },
          hover: {
            mode: 'nearest',
            intersect: false
          },
          plugins: {
            streaming: {
              frameRate: 30
            }
          }
        }
      };
      return new Chart(elem.getContext('2d'), chartData, {responsive: true, maintainAspectRatio: false});
    },
    onRefresh: function() {
      if (!this.lineChart) return;
      this.lineChart.config.data.datasets[0].data.push({
        x: Date.now(),
        y: this.CurrentCpuUsage
      });
      this.lineChart.config.data.datasets[1].data.push({
        x: Date.now(),
        y: this.CurrentMemoryUsage
      });
    }
  }
}
</script>
