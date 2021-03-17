<template>
  <div class="auth-callback">
    <div v-if="error">
      <h1>Login failed</h1>
      <p>{{ error }}</p>
      <router-link to="/auth/signin">
        Go back to login page
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: "auth-callback",
  data() {
    return {
      error: null
    }
  },
  created() {
    const params = new URLSearchParams(window.location.search)
    const error = params.get("error")
    if (error) {
      this.error = error
      return
    }
    const code = params.get("code")
    const state = params.get("state")
    this.$store
      .dispatch("Auth/exchangeAuthCode", { code })
      .then(() => this.$router.push(state))
  }
}
</script>
