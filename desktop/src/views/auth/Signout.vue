<template>
  <div class="signout"></div>
</template>

<script>
export default {
  name: "signout",
  created() {
    this.$store
      .dispatch("Auth/signout")
      .then(() => this.$router.push("/auth/signin"))
  }
}
</script>
