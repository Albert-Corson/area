<template>
  <div class="services-list">
    <services-list-item
      v-for="service in services"
      :key="service.id"
      :service="service"
    />
  </div>
</template>

<script>
import ServicesListItem from "@/components/services/ServicesListItem"

export default {
  name: "services-list",
  components: {
    ServicesListItem
  },
  props: {
    services: Array
  }
}
</script>

<style lang="scss" scoped>
.services-list {
  padding: 1.5rem;
}
</style>
