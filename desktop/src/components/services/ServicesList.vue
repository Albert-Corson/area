<template>
  <div class="services-list">
    <div v-for="(service, index) in services" :key="index">
      <services-list-item
        v-if="serviceWidgets(service).length > 0"
        :service="service"
        :widgets="serviceWidgets(service)"
      />
    </div>
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
