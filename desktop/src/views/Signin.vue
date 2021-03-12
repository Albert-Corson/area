<template>
  <div class="view" id="signin">
    <div>
      <h1>Area</h1>
      <form @submit.prevent="submit">
        <input
          name="identifier"
          type="text"
          placeholder="Email or username..."
        />
        <input name="password" type="password" placeholder="Password..." />
        <button class="gradient" type="submit">Sign in</button>
        <provider-buttons />
        <router-link to="signup">
          <button type="button">Register</button>
        </router-link>
      </form>
    </div>
  </div>
</template>

<script>
import ProviderButtons from "@/components/ProviderButtons"

export default {
  name: "signin",
  components: {
    ProviderButtons
  },
  created() {
    if (this.$store.getters["Auth/isAuthenticated"]) {
      this.$router.push("/")
    }
  },
  methods: {
    submit({ target }) {
      const payload = { ...Object.fromEntries(new FormData(target).entries()) }
      this.$store.dispatch("Auth/signin", payload).then(response => {
        if (response.successful) {
          this.$router.push("/")
        } else {
          // TODO
        }
      })
    }
  }
}
</script>

<style lang="scss" scoped>
#signin {
  display: flex;
  align-items: center;
  justify-content: center;

  form {
    max-width: 300px;
    margin: auto;

    button,
    input {
      width: 100%;
      margin: 0.5rem auto;
    }
  }
}
</style>
