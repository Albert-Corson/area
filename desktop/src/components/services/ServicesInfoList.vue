<template>
  <div class="services-info-list">
    <div v-for="(service, index) in services" :key="index">
      <services-info-list-item
        v-if="serviceWidgets(service).length > 0"
        :service="service"
        :widgets="serviceWidgets(service)"
        v-on="$listeners"
      />
    </div>
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
