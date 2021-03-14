<template>
  <div class="services-home">
    <services-nav />
    <main>
      <h1>Services</h1>
      <services-list :services="sortedServices" />
    </main>
  </div>
</template>

<script>
import ServicesList from "@/components/services/ServicesList"
import ServicesNav from "@/components/services/ServicesNav"

export default {
  name: "services-home",
  components: {
    ServicesList,
    ServicesNav
  },
  created() {
    if (this.$store.state.Service.services.length === 0) {
      this.$store.dispatch("Service/listServices")
    }
    this.$store.dispatch("Service/listMyServices")
  },
  computed: {
    sortedServices() {
      return this.$store.getters["Service/sortedServices"]
    },
    myServices() {
      return this.$store.state.Service.myServices
    }
  }
}
</script>

<style lang="scss" scoped>
@import "@/styles/vars";

.services-home {
  padding-left: 20vw;

  .services-nav {
    width: 20vw;
    height: 100vh;
    position: fixed;
    left: 0;
    top: 0;
    box-shadow: $upShadow;
  }

  .services-list {
    text-align: left;
  }
}
</style>
