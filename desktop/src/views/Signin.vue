<template>
  <div class="view" id="signin">
    <div>
      <h1>Area</h1>
      <form @submit.prevent="submit">
        <input type="text" placeholder="Email or username..." />
        <input type="text" placeholder="Password..." />
        <button class="gradient" type="submit">Sign in</button>
        <router-link to="signup">
          <button type="button">Register</button>
        </router-link>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  name: "signin",
  methods: {
    submit() {
      console.log("submit")
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
