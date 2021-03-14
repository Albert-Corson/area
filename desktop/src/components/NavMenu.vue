<template>
  <div class="nav-menu">
    <div class="profile">{{ username }}</div>
    <div class="links">
      <router-link to="/">
        Dashboard
      </router-link>
      <router-link to="/services">
        Services
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
      return this.$store.state.User.username.slice(
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
  width: 20vw;
  height: 100vh;
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
    display: block;
    padding: 1rem;
    transition: all 0.2s ease-out;
    box-shadow: inset 0px 0 0 0 #4886d8;

    &.selected,
    &:hover {
      background-color: #3b414d;
      box-shadow: inset 6px 0 0 0 #4886d8;
    }
  }
}
</style>
