<template>
  <div class="services-list">
    <services-list-item
      v-for="service in services"
      :key="service.id"
      :service="service"
      :widgets="serviceWidgets(service)"
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
    services: Array,
    widgets: Array
  },
  methods: {
    serviceWidgets(service) {
      return this.widgets.filter(widget => widget.service.id === service.id)
    }
  }
}
</script>

<style lang="scss" scoped>
.services-list {
  padding: 2rem;
}
</style>
