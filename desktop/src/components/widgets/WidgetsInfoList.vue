<template>
  <div class="widgets-info-list">
    <widgets-info-list-item
      v-for="widget in widgets"
      :key="widget.id"
      :widget="widget"
      v-on="$listeners"
    />
  </div>
</template>

<script>
import WidgetsInfoListItem from "./WidgetsInfoListItem"

export default {
  name: "widgets-info-list",
  components: {
    WidgetsInfoListItem
  },
  props: {
    widgets: Array
  }
}
</script>

<style lang="scss" scoped>
.widgets-info-list {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  margin-bottom: 8rem;
}
</style>
