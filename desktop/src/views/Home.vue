<template>
  <div class="home">
    <h1>Welcome {{ username }}</h1>

    <router-link to="/signout">
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
  },
  computed: {
    username() {
      return this.$store.state.User.username
    },
    isAuthenticated() {
      return this.$store.getters["Auth/isAuthenticated"]
    }
  }
})
</script>
