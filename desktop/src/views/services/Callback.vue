<template>
  <div class="service-auth-callback">
    <div v-if="error">
      <h1>Login failed</h1>
      <p>{{ error }}</p>
      <router-link :to="state">
        Go back
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: "service-auth-callback",
  data() {
    return {
      error: null,
      state: null
    }
  },
  created() {
    const params = new URLSearchParams(window.location.search)
    this.state = params.get("state")
    this.error = params.get("error")
    if (this.error) {
      return
    }
    this.$router.push(this.state)
  }
}
</script>
