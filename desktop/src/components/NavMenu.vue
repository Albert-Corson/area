<template>
  <div class="nav-menu">
    <div class="links">
      <router-link to="/">
        Dashboard
      </router-link>
      <router-link to="/auth/signout">
        Sign out
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: "nav-menu",
  computed: {
    username() {
      return this.$store.state.User.username?.slice(
        0,
        this.$store.state.User.username.indexOf("@")
      )
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.nav-menu {
  z-index: 99;
  line-height: 4em;
  width: 100%;
  height: 4em;
  position: fixed;
  background: #414956;
  left: 0;
  top: 0;
  box-shadow: $upShadow;
  color: #fdfdfd;
  font-size: 1.2em;

  .profile {
    height: 4rem;
    padding: 1rem;
    background: #282828;
  }

  .links > * {
    color: inherit;
    text-decoration: none;
    padding: 1em;
  }
}
</style>
