<template>
  <div class="account-details">
    <ul>
      <li><b>username :</b> {{ user.username }}</li>
      <li><b>email :</b> {{ user.email }}</li>
    </ul>
  </div>
</template>

<script>
export default {
  name: "account-details",
  created() {
    if (this.$store.state.User.username === null) {
      this.$store.dispatch("User/fetchInfo")
    }
  },
  computed: {
    user() {
      return {
        id: this.$store.state.User.id,
        username: this.$store.state.User.username,
        email: this.$store.state.User.email
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.account-details {
  display: inline-block;
  text-align: left;

  ul {
    list-style: none;
  }
}
</style>
