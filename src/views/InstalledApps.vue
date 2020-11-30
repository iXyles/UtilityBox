<template>
  <v-container fluid>
    <v-data-table v-if="InstalledWindowsApps.length > 0"
      :headers="headers"
      :items="InstalledWindowsApps"
      :search="search"
      item-key="name"
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar
          flat
        >
          <v-toolbar-title>Installed Apps</v-toolbar-title>
          <v-divider
            class="mx-4"
            inset
            vertical
          ></v-divider>
          <v-btn
            color="blue darken-1"
            @click="refresh"
            :loading="InProgress"
          >
            Refresh
          </v-btn>
          <v-spacer></v-spacer>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            label="Search.."
            single-line
            hide-details
          ></v-text-field>
        </v-toolbar>
      </template>
      <template>
        <tr>
          <td></td>
          <td></td>
          <td></td>
        </tr>
      </template>
      <template v-slot:[`item.actions`]="{ item }">
        <v-icon v-if="item.Installed"
          small
          @click="editItem(item)"
        >
          mdi-wrench
        </v-icon>
      </template>
    </v-data-table>
  <v-card v-else>
    <v-container style="height: 400px;">
      <v-row
        class="fill-height"
        align-content="center"
        justify="center"
      >
        <v-col
          class="subtitle-1 text-center"
          cols="12"
        >
          Getting installed apps...
        </v-col>
        <v-col cols="6">
          <v-progress-linear
            indeterminate
            color="cyan"
            rounded
            height="6"
          ></v-progress-linear>
        </v-col>
      </v-row>
    </v-container>
  </v-card>

  <v-dialog v-model="isInstalled" max-width="600px" :persistent="InProgress">
    <v-card>
      <v-card-title class="headline">Do you want to uninstall {{ currentDialogItem.Name }}?</v-card-title>
      <v-card-actions>
        <v-spacer></v-spacer>

          <v-btn
            color="blue darken-1"
            block
            @click="uninstall"
            :loading="InProgress"
          >
            Uninstall
          </v-btn>

        <v-spacer></v-spacer>
      </v-card-actions>
    </v-card>
  </v-dialog>

  </v-container>
</template>

<script>
import dotnetify from 'dotnetify/vue';

export default {
  name: 'InstalledApps',
  created() {
    this.vm = dotnetify.vue.connect("InstalledApps", this);
  },
  data() {
    return {
      search: '',
      currentDialogId: -1,
      currentDialogItem: {},
      InstalledWindowsApps: [],
      InProgress: false
    }
  },
  destroyed() {
    this.vm.$destroy();
  },
  methods: {
    editItem(item) {
      this.currentDialogId = this.InstalledWindowsApps.indexOf(item);
      this.currentDialogItem = item;
    },
    uninstall() {
      this.vm.$dispatch({
        UninstallApp: this.currentDialogItem.Name
      });
    },
    refresh(){
      this.vm.$dispatch({RefreshApps:null});
    }
  },
  computed: {
    headers () {
      return [
        { text: 'Name', value: 'Name' },
        { text: 'Description', value: 'Desc' },
        { text: 'Installed', value: 'Installed' },
        { text: 'Action', value: 'actions', sortable: false },
      ]
    },
    isInstalled: {
      get() {
        if (!this.currentDialogItem.Name) return false;
        return this.InstalledWindowsApps[this.currentDialogId].Installed;
      },
      set() {
        this.currentDialogItem = {};
        this.currentDialogId = 0;
      }
    }
  }
}
</script>
