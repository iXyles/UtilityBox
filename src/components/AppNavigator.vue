<template>
  <v-container class="uba-navigationcontainer">
    <v-spacer>
      <v-sheet class="pa-2">
        <v-text-field
          v-model="search"
          label="Search..."
          flat
          solo-inverted
          hide-details
          clearable
          clear-icon="mdi-close"
        />
      </v-sheet>

      <v-list 
        v-for="item in filteredItems" :key="item.name"
        dense
        nav
      >
        
        <v-list-item :to="item.to" v-if="!item.children && item.to">
          <v-list-item-icon v-if="item.icon">
            <v-icon>{{item.icon}}</v-icon>
          </v-list-item-icon>

          <v-list-item-title>{{item.name}}</v-list-item-title>
        </v-list-item>

        <v-list-item :to="item.to" v-if="!item.children && item.click" @click.prevent="mapFunctionCall(item.click)">
          <v-list-item-icon v-if="item.icon">
            <v-icon>{{item.icon}}</v-icon>
          </v-list-item-icon>

          <v-list-item-title>{{item.name}}</v-list-item-title>
        </v-list-item>

        <v-list-group v-if="item.children"
          :value="false"
          color="text">

          <template v-slot:activator>
            <v-list-item-icon v-if="item.icon">
              <v-icon>{{item.icon}}</v-icon>
            </v-list-item-icon>
            <v-list-item-title>{{item.name}}</v-list-item-title>
          </template>

          <v-list-item v-for="child in filteredChild(item)" :key="child.name" :to="child.to">
            <v-list-item-icon v-if="item.icon">
              <v-icon>{{child.icon}}</v-icon>
            </v-list-item-icon>
            <v-list-item-title>{{child.name}}</v-list-item-title>
          </v-list-item>

        </v-list-group>

      </v-list>
    </v-spacer>

    <v-divider></v-divider>

    <v-sheet class="pa-2">
      <v-btn block text v-on:click="toogleDarkMode">
        <v-icon>mdi-theme-light-dark</v-icon>{{ toogleDarkModeText }}
      </v-btn>
    </v-sheet>

  </v-container>
</template>

<script>
import { isFunction } from '@/plugins/utils';

export default {
  name: 'app-header',
  computed: {
    toogleDarkModeText: function() {
      return this.$vuetify.theme.dark ? "LIGHT MODE" : "DARK MODE"
    },
    filteredItems() {
      return this.items.filter((item) => {
        if (!this.isAvailable(item)) return false;
        if (item.children) {
          for (var child in item.children) {
            if (item.children[child].name.toLowerCase().indexOf(this.search) > -1)
              if (this.isAvailable(item.children[child]))
                return true;
            else if (item.children[child].tabs) {
              for (var tab in item.children[child].tabs) {
                if (item.children[child].tabs[tab].toLowerCase().indexOf(this.search) > -1)
                  if (this.isAvailable(item.children[child]))
                    return true;
              }
            }
          }
          return false;
        } else {
          return item.name.toLowerCase().indexOf(this.search) > -1
        }
      });
    }
  },
  methods: {
    toogleDarkMode() {
      this.$vuetify.theme.dark = !this.$vuetify.theme.dark;
      //ElectronStore.set("darkMode", this.$vuetify.theme.dark);
    },
    isAvailable(item) {
      return !(isFunction(item.available) && !this.mapFunctionCall(item.available()) || !item.available);
    },
    filteredChild(item) {
      const result = [];
      for (var child in item.children) {
        if (!this.isAvailable(item.children[child])) continue;
        if (item.children[child].name.toLowerCase().indexOf(this.search) > -1) {
          result.push(item.children[child]);
        } else if (item.children[child].tabs) {
          for (let i = 0; i < item.children[child].tabs.length; i++) {
            if (item.children[child].tabs[i].toLowerCase().indexOf(this.search) > -1) {
              result.push(item.children[child]);
              break;
            }
          }
        }
      }
      return result;
    },
    mapFunctionCall(name) {
      return this[name]();
    }
  },
  data: () => ({
    search: '',
    items: [
      {
        name: 'Home',
        icon: 'mdi-home',
        to: '/',
        available: true
      },
      {
        name: 'Windows Toggles',
        icon: 'mdi-toggle-switch',
        to: '/windowstoggles',
        available: true
      },
      {
        name: 'Installed Apps',
        icon: 'mdi-apps',
        to: '/installedapps',
        available: true
      },
      {
        name: 'System Usage',
        icon: 'mdi-chart-line',
        to: '/systemusage',
        available: true
      },
      {
        name: 'About',
        icon: 'mdi-information-outline',
        to: '/about',
        available: true
      }
    ]
  })
};

</script>

<style>
.uba-navigationcontainer {
  margin: 0px !important;
  padding: 0px !important;
  height: 100%;
  display: flex;
  flex-direction: column;
}
</style>