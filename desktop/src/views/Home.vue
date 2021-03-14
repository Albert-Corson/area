<template>
  <div class="home">
    <h1>Welcome {{ username }}</h1>

    <pre style="text-align: left">
      {{ JSON.stringify(devices, null, 4) }}
    </pre>
    <router-link to="/auth/signout">
      Sign out
    </router-link>
  </div>
</template>

<script lang="ts">
import Vue from "vue"

export default Vue.extend({
  name: "Home",
  created() {
    this.$store.dispatch("User/fetchInfo")
    this.$store.dispatch("User/listDevices")
  },
  computed: {
    username() {
      return this.$store.state.User.username
    },
    devices() {
      return this.$store.state.User.devices
    }
  }
})
</script>
