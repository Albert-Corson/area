<template>
  <div class="account-details">
    <p>
      <b>username :</b> {{ user.username }}<br />
      <b>email :</b> {{ user.email }}<br />
    </p>

    <div v-if="!isAuthenticatedWithProvider">
      <h3>Change password</h3>
      <form @submit.prevent="changePassword">
        <table>
          <tr>
            <td>
              <label for="old_password">Old password: </label>
            </td>
            <td>
              <input type="password" id="old_password" name="old_password" />
            </td>
          </tr>
          <tr>
            <td>
              <label for="old_password">New password: </label>
            </td>
            <td>
              <input type="password" id="new_password" name="new_password" />
            </td>
          </tr>
          <tr>
            <td>
              <label for="old_password">Confirm new password: </label>
            </td>
            <td>
              <input
                type="password"
                id="confirm_new_password"
                name="confirm_new_password"
              />
            </td>
          </tr>
        </table>

        <button class="gradient" type="submit">change</button>
      </form>
    </div>
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
    },
    isAuthenticatedWithProvider() {
      return this.$store.getters["User/isAuthenticatedWithProvider"]
    }
  },
  methods: {
    changePassword({ target }) {
      const payload = { ...Object.fromEntries(new FormData(target).entries()) }
      if (payload.confirm_new_password !== payload.new_password) {
        alert("Passwords missmatch")
        return
      }
      delete payload.confirm_new_password
      this.$store
        .dispatch("Auth/changePassword", payload)
        .then(() => this.$router.go(0))
        .catch(() => alert("Failed to change password"))
    }
  }
}
</script>

<style lang="scss" scoped>
.account-details {
  display: inline-block;
  text-align: left;

  form {
    table td {
      padding: 0 0.5rem;
    }

    input {
      padding: 0.5rem 1rem;
      margin: 0;
      box-shadow: none;
      border-bottom: 1px solid #777;
      border-radius: 0;
    }

    button {
      margin-top: 1rem;
    }
  }
}
</style>
