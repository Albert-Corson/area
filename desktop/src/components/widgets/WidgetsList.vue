<template>
  <div class="widgets-list">
    <widgets-list-item
      v-for="widget in widgets"
      :key="widget.id"
      :widget="widget"
    />
  </div>
</template>

<script>
import WidgetsListItem from "./WidgetsListItem"

export default {
  name: "widgets-list",
  components: {
    WidgetsListItem
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

  .widgets-list-item:last-child {
    position: relative;

    &::after {
      display: block;
      width: 11px;
      height: 1px;
      position: absolute;
      top: 0;
      left: 100%;
      visibility: hidden;
      content: "";
    }
  }
}
</style>
