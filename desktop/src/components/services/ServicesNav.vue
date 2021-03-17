<template>
  <div class="services-nav">
    <h2>Services</h2>
    <services-nav-item
      v-for="service in sortedServices"
      :key="service.id"
      :service="service"
    />
  </div>
</template>

<script>
import ServicesNavItem from "./ServicesNavItem.vue"

export default {
  components: {
    ServicesNavItem
  },
  name: "services-nav",
  computed: {
    sortedServices() {
      return this.$store.getters["Service/sortedServices"]
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.services-nav {
  width: 20vw;
  height: 100vh;
  position: fixed;
  left: 0;
  top: 0;
  box-shadow: $upShadow;
}
</style>
