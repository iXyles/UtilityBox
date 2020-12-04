<template>
  <v-container fluid>
    <v-data-table v-if="AvailableToggles.length > 0"
      :headers="headers"
      :items="AvailableToggles"
      :search="search"
      item-key="name"
      class="elevation-1"
    >
      <template v-slot:top>
        <v-toolbar
          flat
        >
          <v-toolbar-title>Service Toggles</v-toolbar-title>
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
        <v-progress-circular
         v-if="item.InProgress"
          indeterminate
          color="primary"
        ></v-progress-circular>
        <v-icon
          v-else-if="item.Enabled"
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
            Getting available service toggles...
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
  name: 'ServiceToggles',
  created() {
    this.vm = dotnetify.vue.connect("ServiceToggles", this);
  },
  data() {
    return {
      search: '',
      AvailableToggles: []
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
      this.vm.$dispatch({RefreshServices:null});
    }
  },
  computed: {
    headers () {
      return [
        { text: 'Name', value: 'Name' },
        { text: 'Description', value: 'Description', sortable: false },
        { text: 'Active', value: 'Enabled', sortable: false },
        { text: 'Action', value: 'actions', sortable: false },
      ]
    }
  }
}
</script>
