<template>
  <div class="signout"></div>
</template>

<script>
export default {
  name: "signout",
  created() {
    this.$store.dispatch("Auth/signout").then(() => {
      window.localStorage.clear()
      this.$router.push("/auth/signin")
    })
  }
}
</script>
