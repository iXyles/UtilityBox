<template>
  <v-container fluid>

    <v-data-table 
      :headers="headers"
      :items="InstalledWindowsApps"
      item-key="name"
      class="elevation-1"
    >
      <template>
        <tr>
          <td></td>
          <td></td>
          <td></td>
        </tr>
      </template>
    </v-data-table>

  </v-container>
</template>

<script>
import dotnetify from 'dotnetify/vue';

export default {
  name: 'InstalledApps',
  created() {
    dotnetify.vue.connect("InstalledApps", this);
  },
  data() {
    return {
      InstalledWindowsApps: []
    }
  },
  computed: {
    headers () {
      return [
        { text: 'Name', value: 'Name' },
        { text: 'Description', value: 'Desc' },
        { text: 'Installed', value: 'Installed' },
      ]
    },
  }
}
</script>
