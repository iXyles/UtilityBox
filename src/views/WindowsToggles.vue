<template>
  <v-container fluid>
    <v-data-table v-if="RegistryToggles.length > 0"
      :headers="headers"
      :items="RegistryToggles"
      :search="search"
      item-key="name"
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar
          flat
        >
          <v-toolbar-title>Windows Toggles</v-toolbar-title>
          <v-divider
            class="mx-4"
            inset
            vertical
          ></v-divider>
          <v-btn
            color="blue darken-1"
            @click="refresh"
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
          <td></td>
          <td></td>
        </tr>
      </template>
      <template v-slot:[`item.actions`]="{ item }">
        <v-icon
          v-if="item.Active"
          @click="toggle(item)"
        >
          mdi-toggle-switch-off
        </v-icon>
        <v-icon 
          v-else
          @click="toggle(item)"
        >
          mdi-toggle-switch
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
            Getting available windows toggles...
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
  </v-container>
</template>

<script>
import dotnetify from 'dotnetify/vue';

export default {
  name: 'WindowsToggles',
  created() {
    this.vm = dotnetify.vue.connect("WindowsToggles", this);
  },
  data() {
    return {
      search: '',
      RegistryToggles: []
    }
  },
  destroyed() {
    this.vm.$destroy();
  },
  methods: {
    toggle(item) {
      this.vm.$dispatch({
        Toggle: item.Name
      });
    },
    refresh(){
      this.vm.$dispatch({RefreshRegistryKeys:null});
    }
  },
  computed: {
    headers () {
      return [
        { text: 'Name', value: 'Name' },
        { text: 'Description', value: 'Description', sortable: false },
        { text: 'Information', value: 'Message', sortable: false },
        { text: 'Checked', value: 'Active', sortable: false },
        { text: 'Action', value: 'actions', sortable: false },
      ]
    }
  }
}
</script>
