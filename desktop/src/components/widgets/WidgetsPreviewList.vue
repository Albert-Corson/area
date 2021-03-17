<template>
  <div class="widgets-list">
    <widgets-preview-list-item
      v-for="widget in widgets"
      :key="widget.id"
      :widget="widget"
    />
  </div>
</template>

<script>
import WidgetsPreviewListItem from "./WidgetsPreviewListItem"

export default {
  name: "widgets-list",
  components: {
    WidgetsPreviewListItem
  },
  props: {
    widgets: Array
  }
}
</script>

<style lang="scss" scoped>
.widgets-list {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  margin-bottom: 8rem;
}
</style>
