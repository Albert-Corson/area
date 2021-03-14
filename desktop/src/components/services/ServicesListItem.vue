<template>
  <div class="services-list-item" :id="`service-${service.id}`">
    <h2>{{ service.name }}</h2>
    <widgets-list :widgets="widgets" />
  </div>
</template>

<script>
import WidgetsList from "@/components/widgets/WidgetsList"

export default {
  name: "services-list-item",
  components: {
    WidgetsList
  },
  props: {
    service: Object,
    widgets: Array
  }
}
</script>

<style lang="scss" scoped></style>
