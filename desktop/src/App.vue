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

.fade-enter,
.fade-leave-to {
  opacity: 0;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.site {
  margin-left: 20vw;
}
</style>
