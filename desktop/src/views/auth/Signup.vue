<template>
  <div class="view" id="signup">
    <div>
      <h1>Register</h1>
      <form @submit.prevent="submit">
        <input name="email" type="email" placeholder="Email..." />
        <input name="username" type="text" placeholder="Username..." />
        <input name="password" type="password" placeholder="Password..." />
        <input
          name="confirm"
          type="password"
          placeholder="Confirm password..."
        />
        <button class="gradient" type="submit">Sign up</button>
        <provider-buttons />
        <router-link to="/auth/signin">
          <button type="button">Already registered ?</button>
        </router-link>
      </form>
    </div>
  </div>
</template>

<script>
import ProviderButtons from "@/components/auth/ProviderButtons"

export default {
  name: "signup",
  components: {
    ProviderButtons
  },
  created() {
    if (this.$store.getters["Auth/isAuthenticated"]) {
      this.$router.push("/")
    }
  },
  methods: {
    async submit({ target }) {
      const payload = { ...Object.fromEntries(new FormData(target).entries()) }
      if (payload.password !== payload.confirm) {
        throw new Error("password do not match")
      }
      delete payload.confirm
      const signup = await this.$store.dispatch("User/signup", payload)
      if (signup.successful) {
        const signin = await this.$store.dispatch("Auth/localSignin", {
          identifier: payload.email,
          password: payload.password
        })
        if (signin.successful) {
          this.$router.push("/auth/signin")
        }
      }
    }
  }
}
</script>

<style lang="scss" scoped>
#signup {
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
