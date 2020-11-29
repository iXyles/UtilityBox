import Vue from 'vue';
import AsyncComputed from 'vue-async-computed';

import App from './App.vue';
import router from './router';
import vuetify from './plugins/vuetify';
import store from './store';

import 'roboto-fontface/css/roboto/roboto-fontface.css';
import '@mdi/font/css/materialdesignicons.css';

Vue.config.productionTip = false;

Vue.use(AsyncComputed);

new Vue({
  router,
  vuetify,
  store,
  render: h => h(App)
}).$mount('#app');
