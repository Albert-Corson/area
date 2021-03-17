<template>
  <div class="nav-menu">
    <div class="nav-left">
      <h2 class="nav-button">AREA</h2>
    </div>
    <div class="nav-center"></div>
    <div class="nav-right">
      <div class="nav-button">
        {{ username }}
      </div>
      <router-link to="/auth/signout" class="nav-button">
        Sign out
      </router-link>
    </div>
  </div>
</template>

<script>
export default {
  name: "nav-menu",
  created() {
    if (this.$store.state.User.username === null) {
      this.$store.dispatch("User/fetchInfo")
    }
  },
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
  overflow: hidden;
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
  display: flex;

  .nav-center {
    flex: 1;
  }

  .nav-button {
    cursor: pointer;
    display: inline-block;
    height: 4em;
    margin: 0;
    padding: 0 2rem;

    box-shadow: inset 0 0 0 0 $accentColor;
    transition: box-shadow 0.2s ease-out;
    &:hover {
      box-shadow: inset 0 -8px 0 0 $accentColor;
    }

    &,
    * {
      color: inherit;
      text-decoration: none;
    }
  }
}
</style>
