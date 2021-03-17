<template>
  <div class="devices-list">
    <table>
      <tr>
        <th>Last used</th>
        <th>Country</th>
        <th>Type</th>
        <th>Browser</th>
        <th>OS</th>
      </tr>
      <devices-list-item
        v-for="(device, index) in devices"
        :key="index"
        :device="device"
        :isCurrent="device.id === currentDevice.id"
      />
    </table>
  </div>
</template>

<script>
import DevicesListItem from "./DevicesListItem"

export default {
  name: "devices-list",
  components: {
    DevicesListItem
  },
  props: {
    devices: Array,
    currentDevice: Object
  }
}
</script>

<style lang="scss" scoped>
.devices-list {
  display: inline-block;
  th,
  td {
    padding: 0.3rem 1rem;
    text-align: left;
  }
}
</style>
