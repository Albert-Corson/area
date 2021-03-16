<template>
  <div id="app">
    <div v-if="isAuthenticated" class="site">
      <nav-menu />
      <div class="page">
        <transition name="fade" mode="out-in">
          <router-view />
        </transition>
      </div>
    </div>
    <div v-else>
      <div class="page">
        <transition name="fade" mode="out-in">
          <router-view />
        </transition>
      </div>
    </div>
  </div>
</template>

<script>
import NavMenu from "@/components/NavMenu"

export default {
  name: "app",
  components: {
    NavMenu
  },
  computed: {
    isAuthenticated() {
      return this.$store.getters["Auth/isAuthenticated"]
    }
  }
}
</script>

<style lang="scss">
@import "@/styles/defaults.scss";
@import "@/styles/transitions.scss";

.site {
  margin-left: 20vw;
}
</style>
