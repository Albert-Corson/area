<template>
  <div class="home">
    <h1>My dashboard</h1>
    <services-list :services="mySortedServices" :widgets="myWidgets" />
  </div>
</template>

<script>
import ServicesList from "@/components/services/ServicesList"

export default {
  name: "home",
  components: {
    ServicesList
  },
  created() {
    if (this.$store.state.Service.myServices.length === 0) {
      this.$store.dispatch("Service/listMyServices")
    }
    this.$store.dispatch("Widget/listMyWidgets")
  },
  computed: {
    mySortedServices() {
      return this.$store.getters["Service/mySortedServices"]
    },
    myWidgets() {
      return this.$store.state.Widget.myWidgets
    }
  }
}
</script>
