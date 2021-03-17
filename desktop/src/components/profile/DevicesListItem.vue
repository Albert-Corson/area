<template>
  <tr class="devices-list-item">
    <td>
      {{ lastUsed }}
    </td>
    <td>
      {{ device.country }}
    </td>
    <td>
      {{ device.device }}
    </td>
    <td>
      {{ device.browser }}
    </td>
    <td>
      {{ device.os }}
    </td>
    <td v-if="!isCurrent">
      <button class="revoke-button" @click="revoke">Revoke</button>
    </td>
  </tr>
</template>

<script>
export default {
  name: "devices-list-item",
  props: {
    device: Object,
    isCurrent: Boolean
  },
  computed: {
    lastUsed() {
      const pad = (str, padder = "0", padding = 2) => {
        return `${padder.repeat(padding)}${str}`.slice(-padding)
      }
      const date = new Date(this.device.last_used * 1000)
      return `${pad(date.getDate())}/${pad(
        date.getMonth() + 1
      )}/${date.getFullYear()} ${pad(date.getHours())}:${pad(
        date.getMinutes()
      )}:${pad(date.getSeconds())}`
    }
  },
  methods: {
    revoke() {
      this.$store
        .dispatch("User/forgetDevice", this.device.id)
        .then(() => this.$router.go(0))
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.devices-list-item {
  .revoke-button {
    font-weight: normal;
    font-size: 1rem;
    background: rgb(214, 116, 105);
    color: $bgColor;
    padding: 0 0.7rem;
  }
}
</style>
