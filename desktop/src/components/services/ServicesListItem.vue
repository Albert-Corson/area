<template>
  <div class="services-list-item" :id="`service-${service.id}`">
    <h2>{{ service.name }}</h2>
    <widgets-list :widgets="serviceWidgets" />
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
    service: Object
  },
  computed: {
    serviceWidgets() {
      const widgets = this.$store.getters["Widget/serviceWidgets"](
        this.service.id
      )
      if (widgets.length === 0) {
        this.$store.dispatch("Widget/listWidgets")
      }
      return widgets
    }
  }
}
</script>

<style lang="scss" scoped></style>
