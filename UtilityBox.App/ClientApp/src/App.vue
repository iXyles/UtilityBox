<template>
  <v-app id="app">
    <v-navigation-drawer v-model="drawer" clipped app :class="scrollbarTheme">
      <AppNavigator />
    </v-navigation-drawer>

    <v-app-bar app clipped-left>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>UtilityBox</v-toolbar-title>
    </v-app-bar>

    <v-main>
      <fade-transition origin="center" mode="out-in" :duration="250">
        <router-view/>
      </fade-transition>
    </v-main>
  </v-app>
</template>

<script>
import { FadeTransition } from 'vue2-transitions';
import AppNavigator from './components/AppNavigator';

export default {
  components: {
    FadeTransition,
    AppNavigator
  },
  data: () => ({ drawer: null }),
  mounted() {
    const theme = localStorage.getItem('darkMode');
    this.$vuetify.theme.dark = theme && theme === 'false';
  },
  computed: {
    scrollbarTheme() {
      return this.$vuetify.theme.dark ? 'darkscroll' : 'lightscroll';
    }
  }
};
</script>

<style lang="scss">
html {
  overflow-y: auto;
}
.lightscroll::-webkit-scrollbar, 
.lightscroll .v-navigation-drawer__content::-webkit-scrollbar {
  width: 15px;
}
.lightscroll::-webkit-scrollbar-track, 
.lightscroll .v-navigation-drawer__content::-webkit-scrollbar-track {
  background: #e6e6e6;
  border-left: 1px solid #dadada;
}
.lightscroll::-webkit-scrollbar-thumb, 
.lightscroll .v-navigation-drawer__content::-webkit-scrollbar-thumb {
  background: #b0b0b0;
  border: solid 3px #e6e6e6;
  border-radius: 7px;
}
.lightscroll::-webkit-scrollbar-thumb:hover, 
.lightscroll .v-navigation-drawer__content::-webkit-scrollbar-thumb:hover {
  background: #404040;
}
.darkscroll::-webkit-scrollbar, 
.darkscroll .v-navigation-drawer__content::-webkit-scrollbar {
  width: 15px;
}
.darkscroll::-webkit-scrollbar-track, 
.darkscroll .v-navigation-drawer__content::-webkit-scrollbar-track {
  background: #202020;
  border-left: 1px solid #2c2c2c;
}
.darkscroll::-webkit-scrollbar-thumb, 
.darkscroll .v-navigation-drawer__content::-webkit-scrollbar-thumb {
  background: #3e3e3e;
  border: solid 3px #202020;
  border-radius: 7px;
}
.darkscroll::-webkit-scrollbar-thumb:hover, 
.darkscroll .v-navigation-drawer__content::-webkit-scrollbar-thumb:hover {
  background:#8d8d8d;
}
</style>