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

<style lang="scss" scoped>
h2 {
  display: flex;
  justify-content: center;
  align-items: center;

  &::before,
  &::after {
    content: "";
    display: block;
    height: 3px;
    flex: 1 0;
  }

  &::before {
    background: linear-gradient(to right, transparent, #999999);
    margin-right: 4vh;
  }

  &::after {
    background: linear-gradient(to left, transparent, #999999);
    margin-left: 4vh;
  }
}
</style>
