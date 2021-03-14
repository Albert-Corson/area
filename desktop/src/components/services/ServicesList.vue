<template>
  <div class="services-list">
    <service-card
      v-for="service in services"
      :key="service.id"
      :service="service"
    />
  </div>
</template>

<script>
import ServiceCard from "./ServiceCard"

export default {
  name: "services-list",
  props: {
    services: Array
  },
  components: {
    ServiceCard
  }
}
</script>

<style lang="scss" scoped>
.services-list {
  display: flex;
  flex-wrap: wrap;
  padding: 1.5rem;

  .service-card {
    margin: 0.3rem;
  }
}
</style>
