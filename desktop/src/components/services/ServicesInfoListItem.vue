<template>
  <div class="services-info-list-item" :id="`service-${service.id}`">
    <h2>{{ service.name }}</h2>
    <widgets-info-list :widgets="widgets" v-on="$listeners" />
  </div>
</template>

<script>
import WidgetsInfoList from "@/components/widgets/WidgetsInfoList"

export default {
  name: "services-info-list-item",
  components: {
    WidgetsInfoList
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
