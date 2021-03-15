<template>
  <div class="services-info-list">
    <services-info-list-item
      v-for="service in services"
      :key="service.id"
      :service="service"
      :widgets="serviceWidgets(service)"
    />
  </div>
</template>

<script>
import ServicesInfoListItem from "@/components/services/ServicesInfoListItem"

export default {
  name: "services-info-list",
  components: {
    ServicesInfoListItem
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
