<template>
  <div class="nav-menu">
    <div class="nav-left">
      <h2 class="nav-button">
        <router-link to="/">
          AREA
        </router-link>
      </h2>
    </div>
    <div class="nav-center"></div>
    <div class="nav-right">
      <router-link to="/users/me" class="nav-button">
        {{ username }}
      </router-link>
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
      const username = this.$store.state.User.username
      if (username && username.indexOf("@") === -1) {
        return username
      }
      return username?.slice(0, username.indexOf("@"))
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

  .nav-item,
  .nav-button {
    display: inline-block;
    height: 4em;
    margin: 0;
    padding: 0 2rem;

    &,
    * {
      color: inherit;
      text-decoration: none;
    }
  }

  .nav-button {
    cursor: pointer;

    box-shadow: inset 0 0 0 0 $accentColor;
    transition: box-shadow 0.2s ease-out;
    &:hover {
      box-shadow: inset 0 -8px 0 0 $accentColor;
    }
  }
}
</style>
