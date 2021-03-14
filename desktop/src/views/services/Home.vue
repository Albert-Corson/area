<template>
  <div class="services-home">
    <main>
      <h1>Services</h1>
      <services-list :services="sortedServices" :widgets="widgets" />
    </main>
  </div>
</template>

<script>
import ServicesList from "@/components/services/ServicesList"

export default {
  name: "services-home",
  components: {
    ServicesList
  },
  created() {
    if (this.$store.state.Service.services.length === 0) {
      this.$store.dispatch("Service/listServices")
    }
    if (this.$store.state.Widget.widgets.length === 0) {
      this.$store.dispatch("Widget/listWidgets")
    }
  },
  computed: {
    sortedServices() {
      return this.$store.getters["Service/sortedServices"]
    },
    myServices() {
      return this.$store.state.Service.myServices
    },
    widgets() {
      return this.$store.state.Widget.widgets
    }
  }
}
</script>

<style lang="scss" scoped>
.services-home {
  .services-list {
    text-align: left;
  }
}
</style>
