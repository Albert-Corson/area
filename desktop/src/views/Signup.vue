<template>
  <div class="view" id="signup">
    <div>
      <h1>Register</h1>
      <form @submit.prevent="submit">
        <input type="text" placeholder="Email..." />
        <input type="text" placeholder="Username..." />
        <input type="text" placeholder="Password..." />
        <input type="text" placeholder="Confirm password..." />
        <button class="gradient" type="submit">Sign up</button>
        <router-link to="signin">
          <button type="button">Already registered ?</button>
        </router-link>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  name: "signup",
  methods: {
    submit() {
      console.log("submit")
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
