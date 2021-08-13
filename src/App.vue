<template>
  <v-app id="app">
    <v-navigation-drawer clipped permanent app :class="scrollbarTheme">
      <AppNavigator />
    </v-navigation-drawer>

    <v-app-bar app clipped-left fixed id="app-bar">
      <v-toolbar-title id="app-title">UtilityBox</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-menu auto>
        <template v-slot:activator="{ on, attrs }">
          <v-btn
            icon
            v-bind="attrs"
            v-on="on"
          >
            <v-icon>mdi-dots-vertical</v-icon>
          </v-btn>
        </template>

        <v-list dense>
          <v-list-item @click="exit">
            <v-list-item-title>Exit</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>

    <v-main>
      <fade-transition origin="center" mode="out-in" :duration="250">
        <router-view/>
      </fade-transition>
    </v-main>
  </v-app>
</template>

<script>
import dotnetify from 'dotnetify/vue';
import { FadeTransition } from 'vue2-transitions';
import AppNavigator from './components/AppNavigator';

export default {
  components: {
    FadeTransition,
    AppNavigator
  },
  created() {
    this.vm = dotnetify.vue.connect("App", this);
  },
  mounted() {
    const theme = localStorage.getItem('darkMode');
    this.$vuetify.theme.dark = theme && theme === 'false' ? false : true;
  },
  methods: {
    exit() {
      this.vm.$dispatch({ Exit: true });
      window.close();
    }
  },
  computed: {
    scrollbarTheme() {
      return this.$vuetify.theme.dark ? 'darkscroll' : 'lightscroll';
    }
  },
  destroyed() {
    this.vm.$destroy();
  }
};
</script>

<style lang="scss">
html {
  overflow-y: auto;
}
#app-title {
  -webkit-user-select: none;
}
#app-bar {
  button {
    -webkit-app-region: no-drag;
  }
  -webkit-app-region: drag;
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