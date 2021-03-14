<template>
  <div class="services-home">
    <h1>Services</h1>
    <services-list :services="servicesList" />
    <services-list :services="myServices" />
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
    this.$store.dispatch("Service/listMyServices")
  },
  computed: {
    servicesList() {
      return this.$store.state.Service.services
    },
    myServices() {
      return this.$store.state.Service.myServices
    }
  }
}
</script>
